using System;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Linq;
using System.Data.Entity;
using CodingRep.App_Code;
using CodingRep.service.query;

namespace CodingRep.src.views
{
    public partial class Repo : System.Web.UI.Page
    {
        private RepositoryService _repoService;
        private FileManagementService _fileService;
        private int _repoId;

        protected void Page_Load(object sender, EventArgs e)
        {
            _repoService = new RepositoryService();
            _fileService = new FileManagementService();
            
            // 获取仓库ID
            if (!GetRepoId())
            {
                HandleInvalidRepo();
                return;
            }
            
            if (!IsPostBack)
            {
                LoadAllData();
            }
        }

        #region 仓库ID获取和验证

        /// <summary>
        /// 获取仓库ID - 支持多种方式
        /// </summary>
        private bool GetRepoId()
        {
            try
            {
                // 方式1: 从URL查询参数获取 (推荐)
                if (TryGetRepoIdFromQueryString())
                {
                    return true;
                }

                // 方式2: 从Session获取 (用于页面间跳转)
                if (TryGetRepoIdFromSession())
                {
                    return true;
                }

                // 方式3: 开发环境的默认值
                // if (IsDebugMode())
                // {
                //     _repoId = 1; // 开发时的默认仓库
                //     return true;
                // }

                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("获取仓库ID失败: " + ex.Message);
                return false;
            }
        }

        private bool TryGetRepoIdFromQueryString()
        {
            // 尝试从 ?id=123
            string repoIdParam = Request.QueryString["id"];
            if (!string.IsNullOrEmpty(repoIdParam))
            {
                if (int.TryParse(repoIdParam, out _repoId) && _repoId > 0)
                {
                    return true;
                }
            }

            // 尝试从 ?repoId=123
            repoIdParam = Request.QueryString["repoId"];
            if (!string.IsNullOrEmpty(repoIdParam))
            {
                if (int.TryParse(repoIdParam, out _repoId) && _repoId > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool TryGetRepoIdFromSession()
        {
            if (Session["CurrentRepoId"] != null)
            {
                if (int.TryParse(Session["CurrentRepoId"].ToString(), out _repoId) && _repoId > 0)
                {
                    return true;
                }
            }

            return false;
        }

        private bool IsDebugMode()
        {
            #if DEBUG
                return true;
            #else
                return HttpContext.Current.IsDebuggingEnabled;
            #endif
        }

        private void HandleInvalidRepo()
        {
            if (IsDebugMode())
            {
                ShowTestingInfo();
            }
            else
            {
                Response.Redirect("~/Default.aspx?error=invalid_repo");
            }
        }

        private void ShowTestingInfo()
        {
            string testingHtml = @"
                <div style='padding: 20px; background: #f8f9fa; border: 1px solid #dee2e6; margin: 20px;'>
                    <h3>🛠️ 开发测试模式</h3>
                    <p>请选择一个测试仓库：</p>
                    <ul>
                        <li><a href='Repo.aspx?id=1'>测试仓库 1</a></li>
                        <li><a href='Repo.aspx?id=2'>测试仓库 2</a></li>
                        <li><a href='Repo.aspx?id=3'>测试仓库 3</a></li>
                    </ul>
                </div>";
            
            Response.Write(testingHtml);
            Response.End();
        }

        #endregion

        #region 数据加载

        private void LoadAllData()
        {
            try
            {
                // 获取仓库信息
                var repo = GetRepositoryWithValidation();
                if (repo == null) return;

                // 设置仓库基本信息
                SetRepositoryBasicInfo(repo);

                // 加载分支数据
                LoadBranchesData();

                // 设置侧边栏信息
                SetSidebarInfo(repo);

                // 开发模式下显示调试信息
                // if (IsDebugMode())
                // {
                //     ShowDebugInfo();
                // }
            }
            catch (Exception ex)
            {
                ShowMessage("加载数据失败: " + ex.Message, "danger");
            }
        }

        private repositories GetRepositoryWithValidation()
        {
            using (var context = new ModelDb())
            {
                var repo = context.repositories
                    .Include(r => r.users)
                    .FirstOrDefault(r => r.id == _repoId);
                
                if (repo == null)
                {
                    ShowMessage("仓库不存在 (ID: " + _repoId + ")", "danger");
                    return null;
                }
                
                return repo;
            }
        }

        private void SetRepositoryBasicInfo(repositories repo)
        {
            litOwnerName.Text = repo.users.userName;
            litRepoName.Text = repo.name;
            barLitOwnerName.Text = repo.users.userName;
            barLitRepoName.Text = repo.name;
            litRepoDescription.Text = repo.description ?? "No description provided";
            
            lblRepoStatus.Text = repo.isPrivate ? "Private" : "Public";
            lblRepoStatus.CssClass = repo.isPrivate ? "repo-status private" : "repo-status public";
            
            litCreateDate.Text = repo.createdAt.ToString("yyyy-MM-dd");
            
            // 获取提交数和主要语言
            using (var context = new ModelDb())
            {
                int commitCount = context.commits.Count(c => c.repoId == _repoId);
                litCommitCount.Text = commitCount.ToString();
                
                // 简化的主要语言检测
                var mainLanguage = GetMainLanguage(_repoId);
                litMainLanguage.Text = mainLanguage;
            }
        }

        private void LoadBranchesData()
        {
            using (var context = new ModelDb())
            {
                var branches = context.branches
                    .Where(b => b.repoId == _repoId)
                    .OrderBy(b => b.name)
                    .ToList();

                if (branches.Count > 0)
                {
                    // 绑定分支下拉框
                    var branchList = new List<object>();
                    foreach (var branch in branches)
                    {
                        branchList.Add(new { 
                            Text = GetBranchDisplayName(branch.name), 
                            Value = branch.name 
                        });
                    }

                    ddlBranches.DataSource = branchList;
                    ddlBranches.DataTextField = "Text";
                    ddlBranches.DataValueField = "Value";
                    ddlBranches.DataBind();

                    // 设置默认分支
                    var defaultBranch = branches.FirstOrDefault(b => b.name == "main") ?? branches.First();
                    ddlBranches.SelectedValue = defaultBranch.name;
                    
                    LoadBranchData(defaultBranch.name);
                }
                else
                {
                    // 没有分支时创建默认分支
                    CreateDefaultBranch();
                }
            }
        }

        private void CreateDefaultBranch()
        {
            using (var context = new ModelDb())
            {
                var defaultBranch = new branches
                {
                    repoId = _repoId,
                    name = "main",
                    createdAt = DateTime.Now
                };
                
                context.branches.Add(defaultBranch);
                context.SaveChanges();
                
                // 重新加载分支数据
                LoadBranchesData();
            }
        }

        private void LoadBranchData(string branchName)
        {
            try
            {
                using (var context = new ModelDb())
                {
                    var branch = context.branches
                        .FirstOrDefault(b => b.repoId == _repoId && b.name == branchName);
                    
                    if (branch != null && branch.commitId.HasValue)
                    {
                        // 加载文件列表
                        var files = context.fileSnapshots
                            .Include(f => f.commits)
                            .Include(f => f.commits.users)
                            .Where(f => f.commitId == branch.commitId.Value)
                            .OrderBy(f => f.path)
                            .ToList();

                        LoadFilesList(files);
                        
                        // 加载提交记录
                        LoadCommitHistory(branchName);
                        
                        // 更新分支提交数
                        int branchCommitCount = context.commits
                            .Count(c => c.repoId == _repoId);
                        litBranchCommits.Text = branchCommitCount.ToString();
                    }
                    else
                    {
                        // 空分支
                        ShowEmptyState();
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMessage("加载分支数据失败: " + ex.Message, "danger");
            }
        }

        private void LoadFilesList(List<fileSnapshots> files)
        {
            if (files.Count > 0)
            {
                var fileDisplayList = new List<object>();
                foreach (var f in files)
                {
                    fileDisplayList.Add(new {
                        FileId = f.id,
                        FileName = Path.GetFileName(f.path),
                        FilePath = f.path,
                        FileIcon = GetFileIcon(f.path),
                        LastCommitMessage = f.commits != null ? f.commits.message : "No commit message",
                        LastCommitAuthor = f.commits != null && f.commits.users != null ? f.commits.users.userName : "Unknown",
                        LastCommitTime = f.commits != null ? f.commits.timestamp : DateTime.Now
                    });
                }

                rptFiles.DataSource = fileDisplayList;
                rptFiles.DataBind();
                rptFiles.Visible = true;
                pnlNoFiles.Visible = false;
            }
            else
            {
                ShowEmptyState();
            }
        }

        private void LoadCommitHistory(string branchName)
        {
            using (var context = new ModelDb())
            {
                var commits = context.commits
                    .Include(c => c.users)
                    .Where(c => c.repoId == _repoId)
                    .OrderByDescending(c => c.timestamp)
                    .Take(10)
                    .ToList();

                if (commits.Count > 0)
                {
                    var commitDisplayList = new List<object>();
                    foreach (var c in commits)
                    {
                        commitDisplayList.Add(new {
                            CommitMessage = c.message,
                            CommitAuthor = c.users.userName,
                            CommitTime = c.timestamp
                        });
                    }

                    rptCommits.DataSource = commitDisplayList;
                    rptCommits.DataBind();
                    rptCommits.Visible = true;
                    pnlNoCommits.Visible = false;
                }
                else
                {
                    rptCommits.Visible = false;
                    pnlNoCommits.Visible = true;
                }
            }
        }

        private void ShowEmptyState()
        {
            rptFiles.Visible = false;
            pnlNoFiles.Visible = true;
            rptCommits.Visible = false;
            pnlNoCommits.Visible = true;
        }

        private void SetSidebarInfo(repositories repo)
        {
            try
            {
                // 基本信息
                litSidebarDescription.Text = repo.description ?? "No description provided";
                litSidebarOwner.Text = repo.users.userName;
                litOwnerInitial.Text = repo.users.userName.Substring(0, 1).ToUpper();
                litSidebarCreateDate.Text = repo.createdAt.ToString("MMM dd, yyyy");
                litSidebarStatus.Text = repo.isPrivate ? "Private" : "Public";

                using (var context = new ModelDb())
                {
                    // 分支数量
                    int branchCount = context.branches.Count(b => b.repoId == _repoId);
                    litBranchCount.Text = branchCount.ToString();

                    // 总提交数
                    int totalCommits = context.commits.Count(c => c.repoId == _repoId);
                    litTotalCommits.Text = totalCommits.ToString();

                    // 最后更新时间
                    var lastCommit = context.commits
                        .Where(c => c.repoId == _repoId)
                        .OrderByDescending(c => c.timestamp)
                        .FirstOrDefault();
                    
                    if (lastCommit != null)
                    {
                        litLastUpdate.Text = FormatRelativeTime(lastCommit.timestamp);
                    }
                    else
                    {
                        litLastUpdate.Text = FormatRelativeTime(repo.createdAt);
                    }

                    // 语言统计
                    LoadLanguageStats();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("设置侧边栏信息失败: " + ex.Message, "warning");
            }
        }

        private void LoadLanguageStats()
        {
            try
            {
                using (var context = new ModelDb())
                {
                    var latestCommit = context.commits
                        .Where(c => c.repoId == _repoId)
                        .OrderByDescending(c => c.timestamp)
                        .FirstOrDefault();

                    if (latestCommit != null)
                    {
                        var files = context.fileSnapshots
                            .Where(fs => fs.commitId == latestCommit.id)
                            .ToList();

                        var languageStats = new Dictionary<string, long>();
                        
                        foreach (var file in files)
                        {
                            string language = GetLanguageFromPath(file.path);
                            if (!string.IsNullOrEmpty(language))
                            {
                                long size = file.content != null ? file.content.Length : 0;
                                if (languageStats.ContainsKey(language))
                                {
                                    languageStats[language] += size;
                                }
                                else
                                {
                                    languageStats[language] = size;
                                }
                            }
                        }

                        if (languageStats.Count > 0)
                        {
                            var total = languageStats.Values.Sum();
                            var languageDisplayList = new List<object>();
                            
                            foreach (var kvp in languageStats.OrderByDescending(x => x.Value))
                            {
                                double percentage = total > 0 ? Math.Round((double)kvp.Value / total * 100, 1) : 0;
                                languageDisplayList.Add(new {
                                    LanguageName = kvp.Key,
                                    LanguageColor = GetLanguageColor(kvp.Key),
                                    LanguagePercentage = percentage
                                });
                            }

                            rptLanguages.DataSource = languageDisplayList;
                            rptLanguages.DataBind();
                            rptLanguages.Visible = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("加载语言统计失败: " + ex.Message);
            }
        }

        #endregion

        #region 文件管理事件处理

        /// <summary>
        /// 单文件上传按钮点击事件
        /// </summary>
        protected void btnUploadSingle_Click(object sender, EventArgs e)
        {
            try
            {
                if (fileUploadSingle.HasFile)
                {
                    var result = _fileService.UploadSingleFile(
                        _repoId, 
                        GetCurrentUserId(), 
                        fileUploadSingle.PostedFile, 
                        ddlBranches.SelectedValue
                    );
                    
                    ShowMessage(result.Message, result.Success ? "success" : "danger");
                    
                    if (result.Success)
                    {
                        LoadBranchData(ddlBranches.SelectedValue);
                    }
                }
                else
                {
                    ShowMessage("请选择要上传的文件", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("上传失败: " + ex.Message, "danger");
            }
        }

        /// <summary>
        /// 多文件上传按钮点击事件
        /// </summary>
        protected void btnUploadMultiple_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.Files.Count > 0)
                {
                    var result = _fileService.UploadMultipleFiles(
                        _repoId, 
                        GetCurrentUserId(), 
                        Request.Files, 
                        ddlBranches.SelectedValue
                    );
                    
                    ShowMessage(result.Message, result.Success ? "success" : "danger");
                    
                    if (result.Success)
                    {
                        LoadBranchData(ddlBranches.SelectedValue);
                    }
                }
                else
                {
                    ShowMessage("请选择要上传的文件", "warning");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("批量上传失败: " + ex.Message, "danger");
            }
        }

        /// <summary>
        /// 下载仓库按钮点击事件
        /// </summary>
        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                var result = _fileService.DownloadRepository(_repoId, ddlBranches.SelectedValue);
                
                if (result.Success)
                {
                    var data = result.Data as dynamic;
                    if (data != null)
                    {
                        byte[] zipData = data.ZipData;
                        string fileName = data.FileName;
                        SendZipFile(zipData, fileName);
                    }
                }
                else
                {
                    ShowMessage(result.Message, "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("下载失败: " + ex.Message, "danger");
            }
        }

        /// <summary>
        /// 分支选择改变事件
        /// </summary>
        protected void ddlBranches_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadBranchData(ddlBranches.SelectedValue);
        }

        /// <summary>
        /// 文件名点击事件
        /// </summary>
        protected void lnkFileName_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (LinkButton)sender;
                int fileId = int.Parse(btn.CommandArgument);
                
                var result = _fileService.DownloadSingleFile(fileId);
                
                if (result.Success)
                {
                    var data = result.Data as dynamic;
                    if (data != null)
                    {
                        byte[] content = data.Content;
                        string fileName = data.FileName;
                        string contentType = data.ContentType;
                        SendSingleFile(content, fileName, contentType);
                    }
                }
                else
                {
                    ShowMessage(result.Message, "danger");
                }
            }
            catch (Exception ex)
            {
                ShowMessage("文件访问失败: " + ex.Message, "danger");
            }
        }

        /// <summary>
        /// 删除文件按钮点击事件
        /// </summary>
        protected void btnDeleteFile_Click(object sender, EventArgs e)
        {
            try
            {
                var btn = (Button)sender;
                string filePath = btn.CommandArgument;
                
                var result = _fileService.DeleteFile(_repoId, GetCurrentUserId(), filePath, ddlBranches.SelectedValue);
                
                ShowMessage(result.Message, result.Success ? "success" : "danger");
                
                if (result.Success)
                {
                    LoadBranchData(ddlBranches.SelectedValue);
                }
            }
            catch (Exception ex)
            {
                ShowMessage("删除文件失败: " + ex.Message, "danger");
            }
        }

        #endregion

        #region 辅助方法

        private void SendZipFile(byte[] zipData, string fileName)
        {
            Response.Clear();
            Response.ContentType = "application/zip";
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", fileName));
            Response.AddHeader("Content-Length", zipData.Length.ToString());
            Response.BinaryWrite(zipData);
            Response.Flush();
            Response.End();
        }

        private void SendSingleFile(byte[] content, string fileName, string contentType)
        {
            Response.Clear();
            Response.ContentType = contentType;
            Response.AddHeader("Content-Disposition", string.Format("attachment; filename=\"{0}\"", fileName));
            Response.AddHeader("Content-Length", content.Length.ToString());
            Response.BinaryWrite(content);
            Response.Flush();
            Response.End();
        }

        private int GetCurrentUserId()
        {
            // 这里应该从Session或认证系统获取当前用户ID
            if (Session["UserId"] != null)
            {
                return (int)Session["UserId"];
            }
            return 1; // 默认用户ID
        }

        private string GetFileIcon(string filePath)
        {
            string extension = Path.GetExtension(filePath).ToLower();
            switch (extension)
            {
                case ".txt": case ".md": return "📄";
                case ".js": case ".ts": return "📜";
                case ".html": case ".htm": return "🌐";
                case ".css": return "🎨";
                case ".json": return "⚙️";
                case ".xml": return "📋";
                case ".jpg": case ".jpeg": case ".png": case ".gif": return "🖼️";
                case ".pdf": return "📕";
                case ".zip": case ".rar": return "📦";
                case ".cs": case ".aspx": return "💻";
                default: return "📄";
            }
        }

        private string GetMainLanguage(int repoId)
        {
            using (var context = new ModelDb())
            {
                var files = context.fileSnapshots
                    .Where(f => f.commits.repoId == repoId)
                    .ToList();

                var languageCount = new Dictionary<string, int>();
                
                foreach (var file in files)
                {
                    string extension = Path.GetExtension(file.path).ToLower();
                    string language = GetLanguageByExtension(extension);
                    
                    if (languageCount.ContainsKey(language))
                        languageCount[language]++;
                    else
                        languageCount[language] = 1;
                }

                var topLanguage = languageCount.OrderByDescending(kvp => kvp.Value).FirstOrDefault();
                return topLanguage.Key ?? "Text";
            }
        }

        private string GetLanguageByExtension(string extension)
        {
            switch (extension)
            {
                case ".cs": return "C#";
                case ".js": return "JavaScript";
                case ".ts": return "TypeScript";
                case ".html": case ".htm": return "HTML";
                case ".css": return "CSS";
                case ".json": return "JSON";
                case ".xml": return "XML";
                case ".sql": return "SQL";
                case ".py": return "Python";
                case ".java": return "Java";
                default: return "Text";
            }
        }

        private string GetLanguageFromPath(string path)
        {
            string extension = Path.GetExtension(path).ToLower();
            return GetLanguageByExtension(extension);
        }

        private string GetLanguageColor(string language)
        {
            switch (language)
            {
                case "JavaScript": return "#f1e05a";
                case "TypeScript": return "#2b7489";
                case "HTML": return "#e34c26";
                case "CSS": return "#563d7c";
                case "C#": return "#239120";
                case "Java": return "#b07219";
                case "Python": return "#3572A5";
                case "JSON": return "#292929";
                case "XML": return "#0060ac";
                default: return "#586069";
            }
        }

        private string GetBranchDisplayName(string branchName)
        {
            switch (branchName)
            {
                case "main":
                case "master":
                    return "🌟 " + branchName;
                case "develop":
                case "dev":
                    return "🚀 " + branchName;
                default:
                    if (branchName.StartsWith("feature/"))
                        return "✨ " + branchName;
                    else if (branchName.StartsWith("hotfix/"))
                        return "🔧 " + branchName;
                    else if (branchName.StartsWith("release/"))
                        return "🎯 " + branchName;
                    else
                        return "🌿 " + branchName;
            }
        }

        private string FormatRelativeTime(DateTime dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;
            
            if (timeSpan.TotalMinutes < 1)
                return "just now";
            else if (timeSpan.TotalMinutes < 60)
                return string.Format("{0} minutes ago", (int)timeSpan.TotalMinutes);
            else if (timeSpan.TotalHours < 24)
                return string.Format("{0} hours ago", (int)timeSpan.TotalHours);
            else if (timeSpan.TotalDays < 30)
                return string.Format("{0} days ago", (int)timeSpan.TotalDays);
            else
                return dateTime.ToString("MMM dd, yyyy");
        }

        private void ShowMessage(string message, string type)
        {
            string script = string.Format(@"
                if (typeof showMessage === 'function') {{
                    showMessage('{0}', '{1}');
                }} else {{
                    alert('{0}');
                }}", message.Replace("'", "\\'"), type);
            ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", script, true);
        }

        private void ShowDebugInfo()
        {
            string debugInfo = string.Format(@"
                <div style='position: fixed; top: 10px; right: 10px; background: #007bff; color: white; padding: 10px; border-radius: 5px; font-size: 12px; z-index: 9999;'>
                    🛠️ Debug: RepoID = {0}
                </div>", _repoId);
            
            Response.Write(debugInfo);
        }

        #endregion

        protected override void OnUnload(EventArgs e)
        {
            if (_repoService != null)
            {
                _repoService.Dispose();
            }
            if (_fileService != null)
            {
                _fileService.Dispose();
            }
            base.OnUnload(e);
        }
    }
}
