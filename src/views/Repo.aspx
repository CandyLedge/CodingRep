    <%@ Page Title="" Language="C#" MasterPageFile="~/src/motherboard/AppHeaderGeneralBar.master" AutoEventWireup="true" CodeBehind="Repo.aspx.cs" Inherits="CodingRep.src.views.Repo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* ========== 调整整体容器大小 - 更居中 ========== */
        .repo-container { 
            max-width: none !important;   
            width: 78% !important;        
            margin: 0 auto;           
            padding: 25px;            
        }
        
        /* ========== 简化头部 ========== */
        .repo-header { 
            background: white; 
            border: 1px solid #d1d5da; 
            border-radius: 6px; 
            padding: 20px;            
            margin-bottom: 20px;      
        }
        
        .repo-title { 
            font-size: 24px;
            font-weight: 600; 
            color: #0366d6; 
            text-decoration: none; 
        }
        
        .repo-meta { 
            color: #586069; 
            font-size: 16px;          
            margin-top: 10px; 
        }
        
        .repo-stats { 
            display: flex; 
            gap: 20px;                
            margin-top: 12px; 
            font-size: 14px;          
            color: #586069; 
        }
        
        /* ========== 动态状态标签样式 ========== */
        .repo-status {
            padding: 4px 10px; 
            border-radius: 12px; 
            font-size: 12px; 
            margin-left: 10px;
            font-weight: 500;
        }
        
        .repo-status.public {
            background: #f1f8ff; 
            color: #0366d6;
        }
        
        .repo-status.private {
            background: #fff5f5; 
            color: #d73a49;
        }
        
        /* ========== 简化导航栏 ========== */
        .repo-nav { 
            background: white; 
            border: 1px solid #d1d5da; 
            border-radius: 6px; 
            margin-bottom: 20px;      
        }
        
        .nav-tabs { 
            display: flex; 
            border-bottom: 1px solid #d1d5da; 
        }
        
        .nav-tab { 
            padding: 12px 20px;       
            border-bottom: 2px solid transparent; 
            color: #586069; 
            text-decoration: none; 
            font-size: 16px;          
        }
        
        .nav-tab.active { 
            border-bottom-color: #f9826c; 
            color: #24292e; 
            font-weight: 600; 
        }
        
        .nav-tab:hover { 
            color: #24292e; 
        }
        
        /* ========== 文件上传区域样式 ========== */
        .upload-section {
            background: white;
            border: 1px solid #d1d5da;
            border-radius: 6px;
            padding: 20px;
            margin-bottom: 20px;
            display: none; /* 默认隐藏，点击上传按钮时显示 */
        }
        
        .upload-section.show {
            display: block;
        }
        
        .upload-tabs {
            display: flex;
            border-bottom: 1px solid #d1d5da;
            margin-bottom: 15px;
        }
        
        .upload-tab {
            padding: 10px 15px;
            border-bottom: 2px solid transparent;
            cursor: pointer;
            color: #586069;
            font-weight: 500;
        }
        
        .upload-tab.active {
            border-bottom-color: #0366d6;
            color: #0366d6;
        }
        
        .upload-content {
            display: none;
        }
        
        .upload-content.active {
            display: block;
        }
        
        .file-upload-area {
            border: 2px dashed #d1d5da;
            border-radius: 6px;
            padding: 30px;
            text-align: center;
            margin-bottom: 15px;
            transition: all 0.2s ease;
        }
        
        .file-upload-area:hover {
            border-color: #0366d6;
            background-color: #f6f8fa;
        }
        
        .file-upload-area.dragover {
            border-color: #0366d6;
            background-color: #e6f3ff;
        }
        
        .upload-icon {
            font-size: 48px;
            color: #586069;
            margin-bottom: 10px;
        }
        
        .upload-text {
            color: #586069;
            margin-bottom: 15px;
        }
        
        .file-input {
            margin-bottom: 10px;
        }
        
        .upload-buttons {
            display: flex;
            gap: 10px;
            justify-content: center;
            margin-top: 15px;
        }
        
        /* ========== 文件浏览器 ========== */
        .file-browser { 
            background: white; 
            border: 1px solid #d1d5da; 
            border-radius: 6px; 
            margin-bottom: 20px;      
        }
        
        .file-header { 
            padding: 20px;            
            border-bottom: 1px solid #d1d5da; 
            display: flex; 
            justify-content: space-between; 
            align-items: center; 
            flex-wrap: wrap;          
            gap: 15px;                
        }
        
        .file-item { 
            padding: 12px 20px;       
            border-bottom: 1px solid #eaecef; 
            display: flex; 
            justify-content: space-between; 
            align-items: center; 
        }
        
        .file-item:hover { 
            background-color: #f6f8fa; 
        }
        
        .file-name { 
            color: #0366d6; 
            text-decoration: none; 
            font-weight: 600; 
            font-size: 16px;          
        }
        
        .file-name:hover { 
            text-decoration: underline; 
        }
        
        .file-meta { 
            color: #586069; 
            font-size: 14px;          
        }
        
        .file-actions {
            display: flex;
            gap: 8px;
            opacity: 0;
            transition: opacity 0.2s ease;
        }
        
        .file-item:hover .file-actions {
            opacity: 1;
        }
        
        .file-action-btn {
            padding: 4px 8px;
            border: 1px solid #d1d5da;
            border-radius: 4px;
            background: white;
            color: #586069;
            font-size: 12px;
            cursor: pointer;
            text-decoration: none;
        }
        
        .file-action-btn:hover {
            background: #f6f8fa;
            text-decoration: none;
        }
        
        .file-action-btn.delete {
            color: #d73a49;
            border-color: #d73a49;
        }
        
        .file-action-btn.delete:hover {
            background: #ffeef0;
        }
        
        /* ========== 数据绑定的分支选择器 ========== */
        .branch-selector {
            position: relative;
            display: inline-block;
        }
        
        .branch-dropdown {
            appearance: none;
            -webkit-appearance: none;
            -moz-appearance: none;
            background: linear-gradient(to bottom, #ffffff 0%, #f8f9fa 100%);
            border: 1px solid #d1d5da;
            border-radius: 6px;
            padding: 10px 35px 10px 15px;   
            font-size: 16px;              
            color: #24292e;
            cursor: pointer;
            min-width: 160px;              
            font-weight: 500;
            box-shadow: 0 1px 3px rgba(0,0,0,0.1);
            transition: all 0.2s ease;
        }
        
        .branch-dropdown:hover {
            background: linear-gradient(to bottom, #f8f9fa 0%, #e9ecef 100%);
            border-color: #adb5bd;
            box-shadow: 0 2px 6px rgba(0,0,0,0.15);
        }
        
        .branch-dropdown:focus {
            outline: none;
            border-color: #0366d6;
            box-shadow: 0 0 0 3px rgba(3, 102, 214, 0.2);
            background: white;
        }
        
        /* ========== 左侧信息区域 ========== */
        .file-header-left {
            display: flex;
            align-items: center;
            gap: 15px;                
            flex: 1;                   
            min-width: 0;              
        }
        
        .file-header-right {
            display: flex;
            gap: 12px;                
            flex-shrink: 0;            
        }
        
        .commit-info {
            color: #586069; 
            font-size: 15px;          
            white-space: nowrap;       
        }
        
        /* ========== 侧边栏 ========== */
        .sidebar { 
            background: white; 
            border: 1px solid #d1d5da; 
            border-radius: 6px; 
            padding: 20px;            
        }
        
        .sidebar-section { 
            margin-bottom: 25px;      
        }
        
        .sidebar-title { 
            font-size: 16px;          
            font-weight: 600; 
            margin-bottom: 10px; 
        }
        
        /* ========== 主要布局控制 ========== */
        .main-content { 
            display: grid; 
            grid-template-columns: 1fr 320px !important;  
            gap: 30px;                         
            width: 100% !important;            
        }
        
        /* ========== 响应式设计 ========== */
        @media (max-width: 1200px) {            
            .main-content { 
                grid-template-columns: 1fr !important;    
            }
            
            .repo-container {
                width: 85% !important;
                padding: 20px;                 
            }
            
            .file-header {
                flex-direction: column;        
                align-items: stretch;
            }
        }
        
        @media (max-width: 768px) {            
            .repo-container {
                width: 95% !important;
                padding: 15px;                 
            }
        }
        
        /* ========== 按钮样式 ========== */
        .btn { 
            display: inline-flex;
            align-items: center;
            gap: 8px;                 
            padding: 10px 20px;       
            border: 1px solid #d1d5da; 
            border-radius: 6px; 
            background: #f6f8fa; 
            color: #24292e; 
            text-decoration: none; 
            font-size: 16px;          
            font-weight: 500;
            cursor: pointer;
            transition: all 0.2s ease;
        }
        
        .btn:hover { 
            background: #f1f3f4; 
            border-color: #c6cbd1;
            text-decoration: none;
            transform: translateY(-1px);  
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }
        
        .btn-primary { 
            background: #2ea043; 
            color: white; 
            border-color: #2ea043; 
        }
        
        .btn-primary:hover { 
            background: #2c974b; 
            border-color: #2c974b;
        }
        
        .btn-upload {
            background: #0969da;
            color: white;
            border-color: #0969da;
        }
        
        .btn-upload:hover {
            background: #0860ca;
            border-color: #0860ca;
        }
        
        .btn-secondary {
            background: #6c757d;
            color: white;
            border-color: #6c757d;
        }
        
        .btn-secondary:hover {
            background: #5a6268;
            border-color: #5a6268;
        }
        
        /* ========== 语言统计条 ========== */
        .language-bar { 
            height: 10px;             
            background: #e1e4e8; 
            border-radius: 5px; 
            overflow: hidden; 
            margin-bottom: 10px; 
        }
        
        .language-segment { 
            height: 100%; 
            float: left; 
        }
        
        /* ========== 提交记录区域 ========== */
        .commits-section {
            background: white;
            border: 1px solid #d1d5da;
            border-radius: 6px;
            padding: 20px;            
            margin-top: 20px;         
        }
        
        .commit-item {
            padding: 15px 0;          
            border-bottom: 1px solid #eaecef;
        }
        
        .commit-item:last-child {
            border-bottom: none;
        }
        
        .commit-message {
            font-weight: 600;
            margin-bottom: 6px;       
            color: #24292e;
            font-size: 16px;          
        }
        
        .commit-meta {
            font-size: 14px;          
            color: #586069;
        }
        
        /* ========== 空状态样式 ========== */
        .empty-state {
            padding: 40px 30px;       
            text-align: center; 
            color: #586069;
        }
        
        .empty-state-icon {
            font-size: 50px;          
            margin-bottom: 15px;
            opacity: 0.7;
        }
        
        .empty-state h3 {
            margin: 0 0 10px 0; 
            color: #24292e;
            font-size: 18px;          
        }
        
        .empty-state p {
            margin: 0;
            font-size: 16px;          
        }
        
        /* ========== 消息提示样式 ========== */
        .message-container {
            position: fixed;
            top: 20px;
            right: 20px;
            z-index: 10000;
        }
        
        .message {
            padding: 12px 16px;
            border-radius: 6px;
            margin-bottom: 10px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.1);
            animation: slideIn 0.3s ease;
        }
        
        .message.success {
            background: #d4edda;
            color: #155724;
            border: 1px solid #c3e6cb;
        }
        
        .message.danger {
            background: #f8d7da;
            color: #721c24;
            border: 1px solid #f5c6cb;
        }
        
        .message.warning {
            background: #fff3cd;
            color: #856404;
            border: 1px solid #ffeaa7;
        }
        
        @keyframes slideIn {
            from { transform: translateX(100%); opacity: 0; }
            to { transform: translateX(0); opacity: 1; }
        }
        /* Add these styles to your existing CSS */
        
        /* Delete repository button styling */
        .btn-danger { 
            background: #dc3545; 
            color: white; 
            border-color: #dc3545; 
        }
        
        .btn-danger:hover { 
            background: #c82333; 
            border-color: #bd2130;
            transform: translateY(-1px);  
            box-shadow: 0 2px 4px rgba(220, 53, 69, 0.3);
        }
        
        /* Confirmation dialog styling */
        .delete-confirmation-dialog {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.5);
            display: none;
            justify-content: center;
            align-items: center;
            z-index: 10001;
        }
        
        .delete-confirmation-content {
            background: white;
            padding: 30px;
            border-radius: 8px;
            max-width: 500px;
            width: 90%;
            box-shadow: 0 4px 20px rgba(0, 0, 0, 0.3);
        }
        
        .delete-confirmation-title {
            color: #dc3545;
            font-size: 20px;
            font-weight: 600;
            margin-bottom: 15px;
            display: flex;
            align-items: center;
            gap: 10px;
        }
        
        .delete-confirmation-text {
            margin-bottom: 20px;
            line-height: 1.5;
            color: #24292e;
        }
        
        .delete-confirmation-input {
            width: 100%;
            padding: 10px;
            border: 2px solid #d1d5da;
            border-radius: 6px;
            margin-bottom: 20px;
            font-size: 14px;
        }
        
        .delete-confirmation-input:focus {
            outline: none;
            border-color: #dc3545;
        }
        
        .delete-confirmation-buttons {
            display: flex;
            gap: 10px;
            justify-content: flex-end;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AppHeaderStartAdd" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AppHeaderEndAdd" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="mainContainer" runat="server">
    <div class="repo-container">
        <!-- 仓库头部 -->
        <div class="repo-header">
            <h1 style="margin: 0; font-size: 24px;">
                <asp:Literal ID="litOwnerName" runat="server" Text="CandyLedge" />
                <span style="color: #586069;"> / </span>
                <asp:Literal ID="litRepoName" runat="server" Text="CodingRep" />
                <asp:Label ID="lblRepoStatus" runat="server" CssClass="repo-status public" Text="Public" />
            </h1>
            
            <p class="repo-meta">
                <asp:Literal ID="litRepoDescription" runat="server" Text="A coding repository for various projects and experiments" />
            </p>
            
            <div class="repo-stats">
                <span>📅 Created <asp:Literal ID="litCreateDate" runat="server" Text="2024-01-15" /></span>
                <span>📝 <asp:Literal ID="litCommitCount" runat="server" Text="5" /> commits</span>
                <span style="color: #f1e05a;">● <asp:Literal ID="litMainLanguage" runat="server" Text="JavaScript" /></span>
            </div>
        </div>

        <!-- 导航栏 -->
        <div class="repo-nav">
            <div class="nav-tabs">
                <a href="#" class="nav-tab active">📄 Code</a>
            </div>
        </div>

        <!-- 文件上传区域 -->
        <div id="uploadSection" class="upload-section">
            <div class="upload-tabs">
                <div class="upload-tab active" onclick="switchUploadTab('single')">📄 单文件上传</div>
                <div class="upload-tab" onclick="switchUploadTab('multiple')">📁 多文件上传</div>
            </div>
            
            <!-- 单文件上传 -->
            <div id="singleUpload" class="upload-content active">
                <div class="file-upload-area">
                    <div class="upload-icon">📤</div>
                    <div class="upload-text">选择文件或拖拽到此处</div>
                    <asp:FileUpload ID="fileUploadSingle" runat="server" CssClass="file-input" />
                </div>
                <div class="upload-buttons">
                    <asp:Button ID="btnUploadSingle" runat="server" Text="📤 上传文件" CssClass="btn btn-upload" OnClick="btnUploadSingle_Click" />
                    <input type="button" class="btn btn-secondary" value="取消" onclick="hideUploadSection()" />
                </div>
            </div>
            
            <!-- 多文件上传 -->
            <div id="multipleUpload" class="upload-content">
                <div class="file-upload-area">
                    <div class="upload-icon">📁</div>
                    <div class="upload-text">选择多个文件或拖拽文件夹到此处</div>
                    <input type="file" id="fileUploadMultiple" multiple="multiple" class="file-input" />
                </div>
                <div class="upload-buttons">
                    <asp:Button ID="btnUploadMultiple" runat="server" Text="📁 批量上传" CssClass="btn btn-upload" OnClick="btnUploadMultiple_Click" />
                    <input type="button" class="btn btn-secondary" value="取消" onclick="hideUploadSection()" />
                </div>
            </div>
        </div>

        <!-- 主要内容区域 -->
        <div class="main-content">
            <!-- 左侧主内容 -->
            <div>
                <!-- 文件浏览器 -->
                <div class="file-browser">
                    <div class="file-header">
                        <div class="file-header-left">
                            <span style="font-size: 16px;">🌿</span>
                            
                            <div class="branch-selector">
                                <asp:DropDownList ID="ddlBranches" runat="server" CssClass="branch-dropdown" AutoPostBack="true" OnSelectedIndexChanged="ddlBranches_SelectedIndexChanged">
                                </asp:DropDownList>
                            </div>
                            
                            <span class="commit-info">• <asp:Literal ID="litBranchCommits" runat="server" Text="5" /> commits</span>
                        </div>
                        <div class="file-header-right">
                            <input type="button" class="btn btn-upload" value="📤 Upload" onclick="showUploadSection()" />
                            <asp:Button ID="btnDownload" runat="server" Text="📥 Download" CssClass="btn btn-primary" OnClick="btnDownload_Click" />
                        </div>
                    </div>
                    
                    <!-- 文件列表 -->
                    <div class="file-list">
                        <asp:Repeater ID="rptFiles" runat="server">
                            <ItemTemplate>
                                <div class="file-item">
                                    <div style="display: flex; align-items: center; gap: 10px; flex: 1;">
                                        <span style="font-size: 16px;"><%# Eval("FileIcon") %></span>
                                        <asp:LinkButton ID="lnkFileName" runat="server" CssClass="file-name" 
                                            Text='<%# Eval("FileName") %>' CommandArgument='<%# Eval("FileId") %>' OnClick="lnkFileName_Click" />
                                    </div>
                                    <div style="display: flex; align-items: center; gap: 15px;">
                                        <div class="file-meta">
                                            <span><%# Eval("LastCommitMessage") %></span> • 
                                            <span><%# Eval("LastCommitAuthor") %></span> • 
                                            <span><%# Eval("LastCommitTime", "{0:yyyy-MM-dd HH:mm}") %></span>
                                        </div>
                                        <div class="file-actions">
                                            <asp:Button ID="btnDeleteFile" runat="server" Text="🗑️" CssClass="file-action-btn delete" 
                                                CommandArgument='<%# Eval("FilePath") %>' OnClick="btnDeleteFile_Click" 
                                                OnClientClick="return confirm('确定要删除这个文件吗？');" />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                        <asp:Panel ID="pnlNoFiles" runat="server" Visible="false" CssClass="empty-state">
                            <div class="empty-state-icon">📁</div>
                            <h3>This repository is empty</h3>
                            <p>Upload your first files to get started!</p>
                        </asp:Panel>
                    </div>
                </div>

                <!-- 最近提交记录 -->
                <div class="commits-section">
                    <h3 style="margin: 0 0 15px 0; font-size: 18px; font-weight: 600; display: flex; align-items: center; gap: 10px;">
                        <span>📝</span>
                        Recent Commits
                    </h3>
                    
                    <asp:Repeater ID="rptCommits" runat="server">
                        <ItemTemplate>
                            <div class="commit-item">
                                <div class="commit-message">
                                    <%# Eval("CommitMessage") %>
                                </div>
                                <div class="commit-meta">
                                    <span>👤 <%# Eval("CommitAuthor") %></span> • 
                                    <span>🕒 <%# Eval("CommitTime", "{0:yyyy-MM-dd HH:mm}") %></span>
                                </div>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                    
                    <asp:Panel ID="pnlNoCommits" runat="server" Visible="false" CssClass="empty-state">
                        <div class="empty-state-icon">📝</div>
                        <h3>No commits yet</h3>
                        <p>Start by uploading your first files!</p>
                    </asp:Panel>
                </div>
            </div>

            <!-- 右侧侧边栏 -->
            <div>
                <div class="sidebar">
                    <div class="sidebar-section">
                        <div class="sidebar-title">About</div>
                        <p style="font-size: 14px; color: #586069; margin-bottom: 12px; line-height: 1.4;">
                            <asp:Literal ID="litSidebarDescription" runat="server" Text="A coding repository for various projects and experiments" />
                        </p>
                        <div style="font-size: 13px; color: #586069;">
                            <div style="margin-bottom: 6px;">📅 Created <asp:Literal ID="litSidebarCreateDate" runat="server" Text="Jan 15, 2024" /></div>
                            <div>🔓 <asp:Literal ID="litSidebarStatus" runat="server" Text="Public" /> repository</div>
                        </div>
                    </div>

                    <div class="sidebar-section">
                        <div class="sidebar-title">🌿 Branches</div>
                        <p style="font-size: 13px; color: #586069; margin-bottom: 10px;">
                            <asp:Literal ID="litBranchCount" runat="server" Text="4" /> branches
                        </p>
                    </div>

                    <div class="sidebar-section">
                        <div class="sidebar-title">📝 Activity</div>
                        <p style="font-size: 13px; color: #586069; margin-bottom: 6px;">
                            <asp:Literal ID="litTotalCommits" runat="server" Text="5" /> commits total
                        </p>
                        <p style="font-size: 13px; color: #586069;">
                            Last updated <asp:Literal ID="litLastUpdate" runat="server" Text="2 hours ago" />
                        </p>
                    </div>

                    <div class="sidebar-section">
                        <div class="sidebar-title">👤 Owner</div>
                        <div style="display: flex; align-items: center; gap: 10px;">
                            <div style="width: 36px; height: 36px; background: #6366f1; border-radius: 50%; display: flex; align-items: center; justify-content: center; color: white; font-weight: bold; font-size: 16px;">
                                <asp:Literal ID="litOwnerInitial" runat="server" Text="C" />
                            </div>
                            <span style="font-size: 15px; font-weight: 600;">
                                <asp:Literal ID="litSidebarOwner" runat="server" Text="CandyLedge" />
                            </span>
                        </div>
                    </div>

                    <div class="sidebar-section">
                        <div class="sidebar-title">💻 Languages</div>
                        <div class="language-bar"></div>
                        
                        <asp:Repeater ID="rptLanguages" runat="server">
                            <ItemTemplate>
                                <div style="display: flex; justify-content: space-between; margin-bottom: 4px;">
                                    <span><span style="color: <%# Eval("LanguageColor") %>;">●</span> <%# Eval("LanguageName") %></span>
                                    <span><%# Eval("LanguagePercentage", "{0:F1}") %>%</span>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- 消息提示容器 -->
    <div id="messageContainer" class="message-container"></div>

    <script type="text/javascript">
        // 显示上传区域
        function showUploadSection() {
            document.getElementById('uploadSection').classList.add('show');
        }

        // 隐藏上传区域
        function hideUploadSection() {
            document.getElementById('uploadSection').classList.remove('show');
        }

        // 切换上传标签
        function switchUploadTab(type) {
            // 移除所有活动状态
            var tabs = document.querySelectorAll('.upload-tab');
            var contents = document.querySelectorAll('.upload-content');
            
            for (var i = 0; i < tabs.length; i++) {
                tabs[i].classList.remove('active');
            }
            for (var i = 0; i < contents.length; i++) {
                contents[i].classList.remove('active');
            }
            
            // 添加活动状态
            event.target.classList.add('active');
            document.getElementById(type + 'Upload').classList.add('active');
        }

        // 消息提示函数
        function showMessage(message, type) {
            var container = document.getElementById('messageContainer');
            var messageDiv = document.createElement('div');
            messageDiv.className = 'message ' + type;
            messageDiv.innerHTML = message;
            
            container.appendChild(messageDiv);
            
            // 3秒后自动移除
            setTimeout(function() {
                if (messageDiv.parentNode) {
                    messageDiv.parentNode.removeChild(messageDiv);
                }
            }, 3000);
        }

        // 拖拽上传功能
        window.onload = function() {
            var uploadAreas = document.querySelectorAll('.file-upload-area');
            
            for (var i = 0; i < uploadAreas.length; i++) {
                var area = uploadAreas[i];
                
                area.addEventListener('dragover', function(e) {
                    e.preventDefault();
                    this.classList.add('dragover');
                });
                
                area.addEventListener('dragleave', function(e) {
                    e.preventDefault();
                    this.classList.remove('dragover');
                });
                
                area.addEventListener('drop', function(e) {
                    e.preventDefault();
                    this.classList.remove('dragover');
                    
                    var files = e.dataTransfer.files;
                    if (files.length > 0) {
                        showMessage('检测到 ' + files.length + ' 个文件，请点击上传按钮完成上传', 'success');
                    }
                });
            }
        };
    </script>
    
</asp:Content>
