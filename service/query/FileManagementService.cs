using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using CodingRep.App_Code;
using ICSharpCode.SharpZipLib.Zip;

namespace CodingRep.service.query
{
  public class FileManagementService : IDisposable
    {
        private ModelDb _context;
        private readonly string _baseUploadPath;

        public FileManagementService()
        {
            _context = new ModelDb();
            _baseUploadPath = HttpContext.Current.Server.MapPath("~/App_Data/Repositories/");
            
            // 确保上传目录存在
            if (!Directory.Exists(_baseUploadPath))
            {
                Directory.CreateDirectory(_baseUploadPath);
            }
        }

        #region 文件上传功能

        /// <summary>
        /// 单文件上传
        /// </summary>
        public ServiceResult UploadSingleFile(int repoId, int userId, HttpPostedFile file, string branchName)
        {
            try
            {
                // 验证文件
                var validationResult = ValidateFile(file);
                if (!validationResult.Success)
                {
                    return validationResult;
                }

                // 验证仓库和分支
                var repo = _context.repositories.FirstOrDefault(r => r.id == repoId);
                if (repo == null)
                {
                    return new ServiceResult { Success = false, Message = "仓库不存在" };
                }

                var branch = _context.branches.FirstOrDefault(b => b.repoId == repoId && b.name == branchName);
                if (branch == null)
                {
                    return new ServiceResult { Success = false, Message = "分支不存在" };
                }

                // 保存文件
                string fileName = Path.GetFileName(file.FileName);
                string relativePath = fileName;
                string fullPath = Path.Combine(GetRepoPath(repoId), relativePath);
                
                // 确保目录存在
                string directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 保存物理文件
                file.SaveAs(fullPath);

                // 读取文件内容
                byte[] fileContent = File.ReadAllBytes(fullPath);
                string contentHash = CalculateHash(fileContent);

                // 创建提交记录
                var commit = new commits
                {
                    repoId = repoId,
                    userId = userId,
                    message = $"Upload file: {fileName}",
                    timestamp = DateTime.Now
                };
                _context.commits.Add(commit);
                _context.SaveChanges();

                // 创建文件快照
                var fileSnapshot = new fileSnapshots
                {
                    commitId = commit.id,
                    path = relativePath,
                    content = fileContent,
                    contentHash = contentHash,
                    fileMode = 644, // 普通文件权限
                    createdAt = DateTime.Now
                };
                _context.fileSnapshots.Add(fileSnapshot);

                // 更新分支指针
                branch.commitId = commit.id;
                _context.SaveChanges();

                return new ServiceResult 
                { 
                    Success = true, 
                    Message = $"文件 {fileName} 上传成功" 
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult 
                { 
                    Success = false, 
                    Message = $"上传失败: {ex.Message}" 
                };
            }
        }

        /// <summary>
        /// 批量文件上传
        /// </summary>
        public ServiceResult UploadMultipleFiles(int repoId, int userId, HttpFileCollection files, string branchName)
        {
            try
            {
                var results = new List<string>();
                int successCount = 0;
                int failCount = 0;

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        var result = UploadSingleFile(repoId, userId, file, branchName);
                        if (result.Success)
                        {
                            successCount++;
                            results.Add($"✓ {Path.GetFileName(file.FileName)}");
                        }
                        else
                        {
                            failCount++;
                            results.Add($"✗ {Path.GetFileName(file.FileName)}: {result.Message}");
                        }
                    }
                }

                return new ServiceResult
                {
                    Success = successCount > 0,
                    Message = $"批量上传完成: 成功 {successCount} 个，失败 {failCount} 个"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult 
                { 
                    Success = false, 
                    Message = $"批量上传失败: {ex.Message}" 
                };
            }
        }

        /// <summary>
        /// 文件夹上传（处理带目录结构的文件）
        /// </summary>
        public ServiceResult UploadFolder(int repoId, int userId, HttpFileCollection files, string branchName)
        {
            try
            {
                var results = new List<string>();
                int successCount = 0;
                int failCount = 0;

                // 创建一个提交来包含所有文件夹文件
                var commit = new commits
                {
                    repoId = repoId,
                    userId = userId,
                    message = "Upload folder",
                    timestamp = DateTime.Now
                };
                _context.commits.Add(commit);
                _context.SaveChanges();

                for (int i = 0; i < files.Count; i++)
                {
                    var file = files[i];
                    if (file != null && file.ContentLength > 0)
                    {
                        try
                        {
                            // 验证文件
                            var validationResult = ValidateFile(file);
                            if (!validationResult.Success)
                            {
                                failCount++;
                                results.Add($"✗ {file.FileName}: {validationResult.Message}");
                                continue;
                            }

                            // 保持原始的文件夹结构
                            string relativePath = file.FileName.Replace("\\", "/");
                            string fullPath = Path.Combine(GetRepoPath(repoId), relativePath.Replace("/", "\\"));
                            
                            // 确保目录存在
                            string directory = Path.GetDirectoryName(fullPath);
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }

                            // 保存物理文件
                            file.SaveAs(fullPath);

                            // 读取文件内容
                            byte[] fileContent = File.ReadAllBytes(fullPath);
                            string contentHash = CalculateHash(fileContent);

                            // 创建文件快照
                            var fileSnapshot = new fileSnapshots
                            {
                                commitId = commit.id,
                                path = relativePath,
                                content = fileContent,
                                contentHash = contentHash,
                                fileMode = 644,
                                createdAt = DateTime.Now
                            };
                            _context.fileSnapshots.Add(fileSnapshot);

                            successCount++;
                            results.Add($"✓ {relativePath}");
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            results.Add($"✗ {file.FileName}: {ex.Message}");
                        }
                    }
                }

                if (successCount > 0)
                {
                    // 更新分支指针
                    var branch = _context.branches.FirstOrDefault(b => b.repoId == repoId && b.name == branchName);
                    if (branch != null)
                    {
                        branch.commitId = commit.id;
                    }
                    _context.SaveChanges();
                }

                return new ServiceResult
                {
                    Success = successCount > 0,
                    Message = $"文件夹上传完成: 成功 {successCount} 个，失败 {failCount} 个"
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult 
                { 
                    Success = false, 
                    Message = $"文件夹上传失败: {ex.Message}" 
                };
            }
        }

        #endregion

        #region 文件下载功能

        /// <summary>
        /// 下载单个文件
        /// </summary>
        public ServiceResult DownloadSingleFile(int fileId)
        {
            try
            {
                var fileSnapshot = _context.fileSnapshots.FirstOrDefault(f => f.id == fileId);
                if (fileSnapshot == null)
                {
                    return new ServiceResult { Success = false, Message = "文件不存在" };
                }

                string fileName = Path.GetFileName(fileSnapshot.path);
                string contentType = GetContentType(fileName);

                return new ServiceResult
                {
                    Success = true,
                    Message = "文件下载成功",
                    Data = new
                    {
                        Content = fileSnapshot.content,
                        FileName = fileName,
                        ContentType = contentType
                    }
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult 
                { 
                    Success = false, 
                    Message = $"下载文件失败: {ex.Message}" 
                };
            }
        }

        /// <summary>
        /// 下载整个仓库 - 使用SharpZipLib
        /// </summary>
        public ServiceResult DownloadRepository(int repoId, string branchName)
        {
            try
            {
                var repo = _context.repositories.FirstOrDefault(r => r.id == repoId);
                if (repo == null)
                {
                    return new ServiceResult { Success = false, Message = "仓库不存在" };
                }

                var branch = _context.branches.FirstOrDefault(b => b.repoId == repoId && b.name == branchName);
                if (branch == null || !branch.commitId.HasValue)
                {
                    return new ServiceResult { Success = false, Message = "分支不存在或无提交记录" };
                }

                // 获取分支最新提交的所有文件
                var files = _context.fileSnapshots
                    .Where(f => f.commitId == branch.commitId.Value)
                    .ToList();

                if (!files.Any())
                {
                    return new ServiceResult { Success = false, Message = "分支中没有文件" };
                }

                // 使用SharpZipLib创建ZIP
                using (var memoryStream = new MemoryStream())
                {
                    using (var zipStream = new ZipOutputStream(memoryStream))
                    {
                        zipStream.SetLevel(6); // 压缩级别 0-9

                        foreach (var file in files)
                        {
                            // 创建ZIP条目
                            var entry = new ZipEntry(file.path)
                            {
                                DateTime = file.createdAt,
                                Size = file.content.Length
                            };

                            zipStream.PutNextEntry(entry);
                            zipStream.Write(file.content, 0, file.content.Length);
                            zipStream.CloseEntry();
                        }

                        zipStream.Finish();
                        zipStream.Close();

                        byte[] zipData = memoryStream.ToArray();

                        return new ServiceResult
                        {
                            Success = true,
                            Message = "仓库下载成功",
                            Data = new
                            {
                                ZipData = zipData,
                                FileName = $"{repo.name}_{branchName}.zip"
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult 
                { 
                    Success = false, 
                    Message = $"下载仓库失败: {ex.Message}" 
                };
            }
        }

        /// <summary>
        /// 下载指定文件夹 - 使用SharpZipLib
        /// </summary>
        public ServiceResult DownloadFolder(int repoId, string branchName, string folderPath)
        {
            try
            {
                var repo = _context.repositories.FirstOrDefault(r => r.id == repoId);
                if (repo == null)
                {
                    return new ServiceResult { Success = false, Message = "仓库不存在" };
                }

                var branch = _context.branches.FirstOrDefault(b => b.repoId == repoId && b.name == branchName);
                if (branch == null || !branch.commitId.HasValue)
                {
                    return new ServiceResult { Success = false, Message = "分支不存在或无提交记录" };
                }

                // 获取指定文件夹下的所有文件
                var files = _context.fileSnapshots
                    .Where(f => f.commitId == branch.commitId.Value && f.path.StartsWith(folderPath))
                    .ToList();

                if (!files.Any())
                {
                    return new ServiceResult { Success = false, Message = "文件夹中没有文件" };
                }

                using (var memoryStream = new MemoryStream())
                {
                    using (var zipStream = new ZipOutputStream(memoryStream))
                    {
                        zipStream.SetLevel(6);

                        foreach (var file in files)
                        {
                            // 移除文件夹前缀，保持相对路径
                            string relativePath = file.path.Substring(folderPath.Length).TrimStart('/');
                            
                            var entry = new ZipEntry(relativePath)
                            {
                                DateTime = file.createdAt,
                                Size = file.content.Length
                            };

                            zipStream.PutNextEntry(entry);
                            zipStream.Write(file.content, 0, file.content.Length);
                            zipStream.CloseEntry();
                        }

                        zipStream.Finish();
                        zipStream.Close();

                        byte[] zipData = memoryStream.ToArray();
                        string folderName = Path.GetFileName(folderPath.TrimEnd('/'));

                        return new ServiceResult
                        {
                            Success = true,
                            Message = "文件夹下载成功",
                            Data = new
                            {
                                ZipData = zipData,
                                FileName = $"{folderName}.zip"
                            }
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                return new ServiceResult 
                { 
                    Success = false, 
                    Message = $"下载文件夹失败: {ex.Message}" 
                };
            }
        }

        #endregion

        #region 文件删除功能

        /// <summary>
        /// 删除文件
        /// </summary>
        public ServiceResult DeleteFile(int repoId, int userId, string filePath, string branchName)
        {
            try
            {
                var branch = _context.branches.FirstOrDefault(b => b.repoId == repoId && b.name == branchName);
                if (branch == null || !branch.commitId.HasValue)
                {
                    return new ServiceResult { Success = false, Message = "分支不存在" };
                }

                // 检查文件是否存在
                var existingFile = _context.fileSnapshots
                    .FirstOrDefault(f => f.commitId == branch.commitId.Value && f.path == filePath);
                
                if (existingFile == null)
                {
                    return new ServiceResult { Success = false, Message = "文件不存在" };
                }

                // 创建删除提交
                var commit = new commits
                {
                    repoId = repoId,
                    userId = userId,
                    message = $"Delete file: {Path.GetFileName(filePath)}",
                    parentId = branch.commitId,
                    timestamp = DateTime.Now
                };
                _context.commits.Add(commit);
                _context.SaveChanges();

                // 复制其他文件到新提交（除了要删除的文件）
                var otherFiles = _context.fileSnapshots
                    .Where(f => f.commitId == branch.commitId.Value && f.path != filePath)
                    .ToList();

                foreach (var file in otherFiles)
                {
                    var newFileSnapshot = new fileSnapshots
                    {
                        commitId = commit.id,
                        path = file.path,
                        content = file.content,
                        contentHash = file.contentHash,
                        fileMode = file.fileMode,
                        createdAt = DateTime.Now
                    };
                    _context.fileSnapshots.Add(newFileSnapshot);
                }

                // 更新分支指针
                branch.commitId = commit.id;
                _context.SaveChanges();

                // 删除物理文件
                string physicalPath = Path.Combine(GetRepoPath(repoId), filePath);
                if (File.Exists(physicalPath))
                {
                    File.Delete(physicalPath);
                }

                return new ServiceResult 
                { 
                    Success = true, 
                    Message = $"文件 {Path.GetFileName(filePath)} 删除成功" 
                };
            }
            catch (Exception ex)
            {
                return new ServiceResult 
                { 
                    Success = false, 
                    Message = $"删除文件失败: {ex.Message}" 
                };
            }
        }

        #endregion

        #region 辅助方法

        private ServiceResult ValidateFile(HttpPostedFile file)
        {
            if (file == null || file.ContentLength == 0)
            {
                return new ServiceResult { Success = false, Message = "请选择文件" };
            }

            // 文件大小限制 (10MB)
            const int maxFileSize = 10 * 1024 * 1024;
            if (file.ContentLength > maxFileSize)
            {
                return new ServiceResult { Success = false, Message = "文件大小不能超过10MB" };
            }

            // 文件名验证
            string fileName = Path.GetFileName(file.FileName);
            if (string.IsNullOrEmpty(fileName))
            {
                return new ServiceResult { Success = false, Message = "文件名无效" };
            }

            // 危险文件类型检查
            string[] dangerousExtensions = { ".exe", ".bat", ".cmd", ".scr", ".pif", ".com" };
            string extension = Path.GetExtension(fileName).ToLower();
            if (dangerousExtensions.Contains(extension))
            {
                return new ServiceResult { Success = false, Message = "不允许上传此类型的文件" };
            }

            return new ServiceResult { Success = true };
        }

        private string GetRepoPath(int repoId)
        {
            string repoPath = Path.Combine(_baseUploadPath, repoId.ToString());
            if (!Directory.Exists(repoPath))
            {
                Directory.CreateDirectory(repoPath);
            }
            return repoPath;
        }

        private string CalculateHash(byte[] content)
        {
            using (var sha1 = SHA1.Create())
            {
                byte[] hash = sha1.ComputeHash(content);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        private string GetContentType(string fileName)
        {
            string extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".txt": return "text/plain";
                case ".html": case ".htm": return "text/html";
                case ".css": return "text/css";
                case ".js": return "application/javascript";
                case ".json": return "application/json";
                case ".xml": return "application/xml";
                case ".pdf": return "application/pdf";
                case ".jpg": case ".jpeg": return "image/jpeg";
                case ".png": return "image/png";
                case ".gif": return "image/gif";
                case ".zip": return "application/zip";
                case ".cs": return "text/plain";
                case ".aspx": return "text/plain";
                case ".config": return "text/xml";
                default: return "application/octet-stream";
            }
        }

        #endregion

        public void Dispose()
        {
            _context?.Dispose();
        }
    }

    /// <summary>
    /// 服务结果类
    /// </summary>
    public class ServiceResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}