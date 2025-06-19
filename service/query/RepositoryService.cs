using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using CodingRep.App_Code;

namespace CodingRep.service.query
{
    public class RepositoryService : IDisposable
    {
        private readonly ModelDb _context;

        public RepositoryService()
        {
            _context = new ModelDb();
        }

        public RepositoryService(ModelDb context)
        {
            _context = context;
        }

        #region 仓库创建和管理

        /// <summary>
        /// 创建新仓库
        /// </summary>
        public CreateRepositoryResult CreateRepository(int userId, string name, string description, bool isPrivate)
        {
            try
            {
                // 验证输入
                var validationResult = ValidateRepositoryInput(userId, name);
                if (!validationResult.IsValid)
                {
                    return new CreateRepositoryResult
                    {
                        Success = false,
                        Message = validationResult.ErrorMessage
                    };
                }

                // 检查仓库名称是否已存在
                if (IsRepositoryNameExists(userId, name))
                {
                    return new CreateRepositoryResult
                    {
                        Success = false,
                        Message = "Repository name already exists"
                    };
                }

                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        // 创建仓库
                        var repository = new repositories
                        {
                            userId = userId,
                            name = name,
                            description = string.IsNullOrWhiteSpace(description) ? null : description,
                            isPrivate = isPrivate,
                            createdAt = DateTime.Now
                        };

                        _context.repositories.Add(repository);
                        _context.SaveChanges();

                        // 创建默认分支
                        CreateDefaultBranch(repository.id);

                        transaction.Commit();

                        return new CreateRepositoryResult
                        {
                            Success = true,
                            Message = "Repository created successfully",
                            RepositoryId = repository.id
                        };
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("创建仓库事务失败: " + ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                return new CreateRepositoryResult
                {
                    Success = false,
                    Message = "Failed to create repository: " + ex.Message
                };
            }
        }

        /// <summary>
        /// 验证仓库输入
        /// </summary>
        private ValidationResult ValidateRepositoryInput(int userId, string name)
        {
            // 验证用户ID
            if (userId <= 0)
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "Invalid user" };
            }

            // 验证仓库名称不能为空
            if (string.IsNullOrWhiteSpace(name))
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "Repository name is required" };
            }

            // 验证仓库名称长度
            if (name.Length > 100)
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "Repository name is too long (maximum 100 characters)" };
            }

            // 验证仓库名称格式
            if (!Regex.IsMatch(name, @"^[a-zA-Z0-9._-]+$"))
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "Repository name can only contain letters, numbers, dots, hyphens and underscores" };
            }

            // 验证用户是否存在
            var userExists = _context.users.Any(u => u.id == userId);
            if (!userExists)
            {
                return new ValidationResult { IsValid = false, ErrorMessage = "User does not exist" };
            }

            return new ValidationResult { IsValid = true };
        }

        /// <summary>
        /// 检查仓库名称是否已存在
        /// </summary>
        private bool IsRepositoryNameExists(int userId, string name)
        {
            try
            {
                return _context.repositories.Any(r => 
                    r.userId == userId && 
                    r.name.ToLower() == name.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception("检查仓库名称失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 创建默认分支
        /// </summary>
        private void CreateDefaultBranch(int repositoryId)
        {
            try
            {
                var defaultBranch = new branches
                {
                    repoId = repositoryId,
                    name = "main",
                    createdAt = DateTime.Now
                };

                _context.branches.Add(defaultBranch);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception("创建默认分支失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取用户的仓库列表
        /// </summary>
        public List<repositories> GetUserRepositories(int userId)
        {
            try
            {
                return _context.repositories
                    .Where(r => r.userId == userId)
                    .OrderByDescending(r => r.createdAt)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("获取用户仓库列表失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 删除仓库
        /// </summary>
        public bool DeleteRepository(int repositoryId, int userId)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        var repository = _context.repositories
                            .FirstOrDefault(r => r.id == repositoryId && r.userId == userId);

                        if (repository == null)
                        {
                            return false;
                        }

                        // 删除相关的文件快照
                        var commits = _context.commits.Where(c => c.repoId == repositoryId).ToList();
                        foreach (var commit in commits)
                        {
                            var fileSnapshots = _context.fileSnapshots.Where(fs => fs.commitId == commit.id).ToList();
                            _context.fileSnapshots.RemoveRange(fileSnapshots);
                        }

                        // 删除提交记录
                        _context.commits.RemoveRange(commits);

                        // 删除分支
                        var branches = _context.branches.Where(b => b.repoId == repositoryId).ToList();
                        _context.branches.RemoveRange(branches);

                        // 删除仓库
                        _context.repositories.Remove(repository);

                        _context.SaveChanges();
                        transaction.Commit();

                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("删除仓库事务失败: " + ex.Message, ex);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("删除仓库失败: " + ex.Message, ex);
            }
        }

        #endregion

        #region 仓库基本信息

        /// <summary>
        /// 获取仓库信息
        /// </summary>
        public repositories GetRepository(int repoId)
        {
            try
            {
                return _context.repositories
                    .Include("users")
                    .FirstOrDefault(r => r.id == repoId);
            }
            catch (Exception ex)
            {
                throw new Exception("获取仓库信息失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取仓库的提交总数
        /// </summary>
        public int GetRepositoryCommitCount(int repoId)
        {
            try
            {
                return _context.commits.Count(c => c.repoId == repoId);
            }
            catch (Exception ex)
            {
                throw new Exception("获取提交数失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取仓库的主要编程语言
        /// </summary>
        public string GetRepositoryMainLanguage(int repoId)
        {
            try
            {
                var latestCommit = _context.commits
                    .Where(c => c.repoId == repoId)
                    .OrderByDescending(c => c.timestamp)
                    .FirstOrDefault();

                if (latestCommit == null) return "Unknown";

                var files = _context.fileSnapshots
                    .Where(fs => fs.commitId == latestCommit.id)
                    .ToList();

                var languageGroups = files
                    .GroupBy(f => GetLanguageFromPath(f.path))
                    .Where(g => !string.IsNullOrEmpty(g.Key))
                    .Select(g => new { Language = g.Key, Size = g.Sum(f => f.content != null ? f.content.Length : 0) })
                    .OrderByDescending(l => l.Size)
                    .FirstOrDefault();

                return languageGroups != null ? languageGroups.Language : "Unknown";
            }
            catch (Exception ex)
            {
                throw new Exception("获取主要语言失败: " + ex.Message, ex);
            }
        }

        #endregion

        #region 分支管理

        /// <summary>
        /// 获取仓库的所有分支
        /// </summary>
        public List<branches> GetRepositoryBranches(int repoId)
        {
            try
            {
                return _context.branches
                    .Where(b => b.repoId == repoId)
                    .OrderByDescending(b => b.name == "main" || b.name == "master")
                    .ThenBy(b => b.name)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("获取分支列表失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取分支信息
        /// </summary>
        public branches GetBranch(int repoId, string branchName)
        {
            try
            {
                return _context.branches
                    .Include("commits")
                    .FirstOrDefault(b => b.repoId == repoId && b.name == branchName);
            }
            catch (Exception ex)
            {
                throw new Exception("获取分支信息失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取分支的提交数量
        /// </summary>
        public int GetBranchCommitCount(int repoId, string branchName)
        {
            try
            {
                var branch = _context.branches
                    .FirstOrDefault(b => b.repoId == repoId && b.name == branchName);

                if (branch == null || branch.commitId == null) return 0;

                return CountCommitsFromCommit(branch.commitId.Value);
            }
            catch (Exception ex)
            {
                throw new Exception("获取分支提交数失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 检查分支是否为默认分支
        /// </summary>
        public bool IsBranchDefault(string branchName)
        {
            return branchName == "main" || branchName == "master";
        }

        #endregion

        #region 文件管理

        /// <summary>
        /// 获取指定分支的文件快照列表
        /// </summary>
        public List<fileSnapshots> GetBranchFiles(int repoId, string branchName)
        {
            try
            {
                var branch = _context.branches
                    .FirstOrDefault(b => b.repoId == repoId && b.name == branchName);

                if (branch == null || branch.commitId == null) return new List<fileSnapshots>();

                return _context.fileSnapshots
                    .Include("commits")
                    .Include("commits.users")
                    .Where(fs => fs.commitId == branch.commitId.Value)
                    .OrderBy(fs => fs.path)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("获取文件列表失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取文件快照
        /// </summary>
        public fileSnapshots GetFileSnapshot(int fileSnapshotId)
        {
            try
            {
                return _context.fileSnapshots
                    .Include("commits")
                    .Include("commits.users")
                    .FirstOrDefault(fs => fs.id == fileSnapshotId);
            }
            catch (Exception ex)
            {
                throw new Exception("获取文件快照失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取文件内容的文本形式
        /// </summary>
        public string GetFileContentAsText(fileSnapshots fileSnapshot)
        {
            try
            {
                if (fileSnapshot == null || fileSnapshot.content == null) return string.Empty;
                return Encoding.UTF8.GetString(fileSnapshot.content);
            }
            catch (Exception ex)
            {
                throw new Exception("获取文件文本内容失败: " + ex.Message, ex);
            }
        }

        #endregion

        #region 提交记录

        /// <summary>
        /// 获取指定分支的提交记录
        /// </summary>
        public List<commits> GetBranchCommits(int repoId, string branchName, int count)
        {
            try
            {
                var branch = _context.branches
                    .FirstOrDefault(b => b.repoId == repoId && b.name == branchName);

                if (branch == null || branch.commitId == null) return new List<commits>();

                return GetCommitHistory(branch.commitId.Value, count);
            }
            catch (Exception ex)
            {
                throw new Exception("获取提交记录失败: " + ex.Message, ex);
            }
        }
        
        /// <summary>
        /// 获取所有分支的提交记录，并标记所属分支
        /// </summary>
        public List<CommitWithBranchInfo> GetAllBranchesCommits(int repoId)
        {
            try
            {
                var allCommits = _context.commits
                    .Where(c => c.repoId == repoId)
                    .Include("users")
                    .OrderByDescending(c => c.timestamp)
                    .ToList();

                var result = new List<CommitWithBranchInfo>();

                foreach (var commit in allCommits)
                {
                    // 查找哪些分支指向这个提交
                    var branchesPointingToThisCommit = _context.branches
                        .Where(b => b.repoId == repoId && b.commitId == commit.id)
                        .ToList();

                    if (branchesPointingToThisCommit.Any())
                    {
                        foreach (var branch in branchesPointingToThisCommit)
                        {
                            result.Add(new CommitWithBranchInfo
                            {
                                Commit = commit,
                                BranchName = branch.name,
                                IsDefaultBranch = IsBranchDefault(branch.name),
                                IsHeadCommit = true // 当前提交就是该分支的 head
                            });
                        }
                    }
                    else
                    {
                        // 如果没有分支指向该提交，则标记为 "orphaned" 或匿名提交
                        result.Add(new CommitWithBranchInfo
                        {
                            Commit = commit,
                            BranchName = "[detached]",
                            IsDefaultBranch = false,
                            IsHeadCommit = false
                        });
                    }
                }

                return result.OrderByDescending(c => c.Commit.timestamp).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("获取所有分支提交记录失败: " + ex.Message, ex);
            }
        }
        
        #region 辅助类
        /// <summary>
        /// 带分支信息的提交记录
        /// </summary>
        public class CommitWithBranchInfo
        {
            public commits Commit { get; set; }
            public string BranchName { get; set; }
            public bool IsDefaultBranch { get; set; }
            public bool IsHeadCommit { get; set; } // 是否是该分支的最新提交
        }
        #endregion

        /// <summary>
        /// 获取提交信息
        /// </summary>
        public commits GetCommit(int commitId)
        {
            try
            {
                return _context.commits
                    .Include("users")
                    .Include("repositories")
                    .FirstOrDefault(c => c.id == commitId);
            }
            catch (Exception ex)
            {
                throw new Exception("获取提交信息失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 获取提交的文件变更
        /// </summary>
        public List<fileSnapshots> GetCommitFiles(int commitId)
        {
            try
            {
                return _context.fileSnapshots
                    .Where(fs => fs.commitId == commitId)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("获取提交文件失败: " + ex.Message, ex);
            }
        }

        #endregion

        #region 语言统计

        /// <summary>
        /// 获取仓库的语言统计（返回字典：语言名 -> 字节数）
        /// </summary>
        public Dictionary<string, long> GetRepositoryLanguageStats(int repoId)
        {
            try
            {
                var latestCommit = _context.commits
                    .Where(c => c.repoId == repoId)
                    .OrderByDescending(c => c.timestamp)
                    .FirstOrDefault();

                if (latestCommit == null) return new Dictionary<string, long>();

                var files = _context.fileSnapshots
                    .Where(fs => fs.commitId == latestCommit.id)
                    .ToList();

                var result = new Dictionary<string, long>();
                var languageGroups = files
                    .GroupBy(f => GetLanguageFromPath(f.path))
                    .Where(g => !string.IsNullOrEmpty(g.Key));

                foreach (var group in languageGroups)
                {
                    var totalSize = group.Sum(f => f.content != null ? (long)f.content.Length : 0L);
                    result[group.Key] = totalSize;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("获取语言统计失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 计算语言百分比
        /// </summary>
        public Dictionary<string, double> GetLanguagePercentages(Dictionary<string, long> languageStats)
        {
            var result = new Dictionary<string, double>();
            var total = 0L;
            
            foreach (var kvp in languageStats)
            {
                total += kvp.Value;
            }
            
            if (total == 0) return result;

            foreach (var kvp in languageStats)
            {
                result[kvp.Key] = Math.Round((double)kvp.Value / total * 100, 1);
            }

            return result;
        }

        #endregion

        #region 文件操作

        /// <summary>
        /// 上传文件到仓库
        /// </summary>
        public bool UploadFile(int repoId, int userId, string branchName, string filePath, byte[] content, string commitMessage)
        {
            try
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    // 1. 获取父提交ID
                    var parentCommitId = GetLatestCommitId(repoId, branchName);

                    // 2. 创建新的提交
                    var newCommit = new commits
                    {
                        repoId = repoId,
                        userId = userId,
                        message = commitMessage ?? "Upload file",
                        timestamp = DateTime.Now,
                        parentId = parentCommitId
                    };

                    _context.commits.Add(newCommit);
                    _context.SaveChanges();

                    // 3. 复制父提交的所有文件（如果存在）
                    if (parentCommitId.HasValue)
                    {
                        var parentFiles = _context.fileSnapshots
                            .Where(fs => fs.commitId == parentCommitId.Value && fs.path != filePath)
                            .ToList();

                        foreach (var parentFile in parentFiles)
                        {
                            var newFileSnapshot = new fileSnapshots
                            {
                                commitId = newCommit.id,
                                path = parentFile.path,
                                content = parentFile.content,
                                contentHash = parentFile.contentHash,
                                fileMode = parentFile.fileMode,
                                createdAt = DateTime.Now
                            };
                            _context.fileSnapshots.Add(newFileSnapshot);
                        }
                    }

                    // 4. 添加新文件或更新现有文件
                    var fileSnapshot = new fileSnapshots
                    {
                        commitId = newCommit.id,
                        path = filePath,
                        content = content,
                        contentHash = CalculateHash(content),
                        fileMode = 644,
                        createdAt = DateTime.Now
                    };

                    _context.fileSnapshots.Add(fileSnapshot);

                    // 5. 更新分支指向新提交
                    var branch = _context.branches
                        .FirstOrDefault(b => b.repoId == repoId && b.name == branchName);

                    if (branch != null)
                    {
                        branch.commitId = newCommit.id;
                    }
                    else
                    {
                        // 创建新分支
                        var newBranch = new branches
                        {
                            repoId = repoId,
                            name = branchName,
                            commitId = newCommit.id,
                            createdAt = DateTime.Now
                        };
                        _context.branches.Add(newBranch);
                    }

                    _context.SaveChanges();
                    transaction.Commit();

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("上传文件失败: " + ex.Message, ex);
            }
        }

        /// <summary>
        /// 生成仓库下载URL
        /// </summary>
        public string GenerateDownloadUrl(repositories repo, string branchName)
        {
            try
            {
                return "/Download.ashx?repo=" + repo.name + "&branch=" + branchName + "&repoId=" + repo.id;
            }
            catch (Exception ex)
            {
                throw new Exception("生成下载链接失败: " + ex.Message, ex);
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 获取文件图标
        /// </summary>
        public string GetFileIcon(string filePath)
        {
            var extension = Path.GetExtension(filePath).ToLower();
            var fileName = Path.GetFileName(filePath).ToLower();

            // 特殊文件名
            if (fileName == "readme.md" || fileName == "readme.txt") return "📄";
            if (fileName == "package.json") return "⚙️";
            if (fileName == "dockerfile") return "🐳";

            // 按扩展名
            var iconMap = new Dictionary<string, string>();
            iconMap[".js"] = "📜";
            iconMap[".ts"] = "📘";
            iconMap[".html"] = "🌐";
            iconMap[".css"] = "🎨";
            iconMap[".json"] = "⚙️";
            iconMap[".xml"] = "📋";
            iconMap[".md"] = "📄";
            iconMap[".txt"] = "📄";
            iconMap[".jpg"] = "🖼️";
            iconMap[".png"] = "🖼️";
            iconMap[".gif"] = "🖼️";
            iconMap[".svg"] = "🎨";
            iconMap[".pdf"] = "📕";
            iconMap[".doc"] = "📘";
            iconMap[".docx"] = "📘";
            iconMap[".zip"] = "📦";
            iconMap[".rar"] = "📦";
            iconMap[".7z"] = "📦";

            return iconMap.ContainsKey(extension) ? iconMap[extension] : "📄";
        }

        /// <summary>
        /// 获取分支显示名称
        /// </summary>
        public string GetBranchDisplayName(branches branch)
        {
            var icons = new Dictionary<string, string>();
            icons["main"] = "🌟";
            icons["master"] = "🌟";
            icons["develop"] = "🚀";
            icons["dev"] = "🚀";

            if (branch.name.StartsWith("feature/")) return "✨ " + branch.name;
            if (branch.name.StartsWith("hotfix/")) return "🔧 " + branch.name;
            if (branch.name.StartsWith("release/")) return "🎯 " + branch.name;

            return (icons.ContainsKey(branch.name) ? icons[branch.name] : "🌿") + " " + branch.name;
        }

        /// <summary>
        /// 获取语言颜色
        /// </summary>
        public string GetLanguageColor(string language)
        {
            var colorMap = new Dictionary<string, string>();
            colorMap["JavaScript"] = "#f1e05a";
            colorMap["TypeScript"] = "#2b7489";
            colorMap["HTML"] = "#e34c26";
            colorMap["CSS"] = "#563d7c";
            colorMap["C#"] = "#239120";
            colorMap["Java"] = "#b07219";
            colorMap["Python"] = "#3572A5";
            colorMap["C++"] = "#f34b7d";
            colorMap["PHP"] = "#4F5D95";
            colorMap["Ruby"] = "#701516";
            colorMap["Go"] = "#00ADD8";
            colorMap["Rust"] = "#dea584";

            return colorMap.ContainsKey(language) ? colorMap[language] : "#586069";
        }

        /// <summary>
        /// 格式化文件大小
        /// </summary>
        public string FormatFileSize(long bytes)
        {
            if (bytes < 1024) return bytes + " B";
            if (bytes < 1024 * 1024) return (bytes / 1024.0).ToString("F1") + " KB";
            if (bytes < 1024 * 1024 * 1024) return (bytes / (1024.0 * 1024)).ToString("F1") + " MB";
            return (bytes / (1024.0 * 1024 * 1024)).ToString("F1") + " GB";
        }

        /// <summary>
        /// 格式化相对时间
        /// </summary>
        public string FormatRelativeTime(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;
            
            if (timeSpan.TotalMinutes < 1) return "刚刚";
            if (timeSpan.TotalMinutes < 60) return ((int)timeSpan.TotalMinutes) + " 分钟前";
            if (timeSpan.TotalHours < 24) return ((int)timeSpan.TotalHours) + " 小时前";
            if (timeSpan.TotalDays < 30) return ((int)timeSpan.TotalDays) + " 天前";
            if (timeSpan.TotalDays < 365) return ((int)(timeSpan.TotalDays / 30)) + " 个月前";
            return ((int)(timeSpan.TotalDays / 365)) + " 年前";
        }

        #endregion

        #region 私有方法

        private string GetLanguageFromPath(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            var languageMap = new Dictionary<string, string>();
            languageMap[".js"] = "JavaScript";
            languageMap[".ts"] = "TypeScript";
            languageMap[".html"] = "HTML";
            languageMap[".css"] = "CSS";
            languageMap[".cs"] = "C#";
            languageMap[".java"] = "Java";
            languageMap[".py"] = "Python";
            languageMap[".cpp"] = "C++";
            languageMap[".c"] = "C";
            languageMap[".php"] = "PHP";
            languageMap[".rb"] = "Ruby";
            languageMap[".go"] = "Go";

            return languageMap.ContainsKey(extension) ? languageMap[extension] : null;
        }

        private int CountCommitsFromCommit(int commitId)
        {
            var visited = new HashSet<int>();
            return CountCommitsRecursive(commitId, visited);
        }

        private int CountCommitsRecursive(int commitId, HashSet<int> visited)
        {
            if (visited.Contains(commitId)) return 0;
            visited.Add(commitId);

            var commit = _context.commits.FirstOrDefault(c => c.id == commitId);
            if (commit == null) return 0;

            int count = 1;
            if (commit.parentId.HasValue)
            {
                count += CountCommitsRecursive(commit.parentId.Value, visited);
            }

            return count;
        }

        private List<commits> GetCommitHistory(int commitId, int maxCount)
        {
            var result = new List<commits>();
            var current = _context.commits
                .Include("users")
                .FirstOrDefault(c => c.id == commitId);

            while (current != null && result.Count < maxCount)
            {
                result.Add(current);
                current = current.parentId.HasValue ? 
                    _context.commits.Include("users").FirstOrDefault(c => c.id == current.parentId.Value) : null;
            }

            return result;
        }

        private int? GetLatestCommitId(int repoId, string branchName)
        {
            var branch = _context.branches
                .FirstOrDefault(b => b.repoId == repoId && b.name == branchName);
            return branch != null ? branch.commitId : null;
        }

        private string CalculateHash(byte[] content)
        {
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                var hash = sha1.ComputeHash(content);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        #endregion

        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
            }
        }
    }

    #region 结果类

    /// <summary>
    /// 创建仓库结果
    /// </summary>
    public class CreateRepositoryResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public int RepositoryId { get; set; }
    }

    /// <summary>
    /// 验证结果
    /// </summary>
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
    }

    #endregion
}
