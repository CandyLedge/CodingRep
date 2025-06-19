    <%@ Page Title="" Language="C#" MasterPageFile="~/src/motherboard/AppHeaderGeneralBar.master" AutoEventWireup="true" CodeBehind="Repo.aspx.cs" Inherits="CodingRep.src.views.Repo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        /* ========== 调整整体容器大小 - 更居中 ========== */
        .repo-container { 
            max-width: none !important;   
            width: 78% !important;        /* 🔧 从85%缩小到78% - 更居中 */
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
        
        /* 🔧 美化的下拉箭头 */
        .branch-selector::after {
            content: '';
            position: absolute;
            right: 12px;
            top: 50%;
            transform: translateY(-50%);
            width: 0;
            height: 0;
            border-left: 6px solid transparent;
            border-right: 6px solid transparent;
            border-top: 7px solid #586069;
            pointer-events: none;
            transition: transform 0.2s ease;
        }
        
        .branch-dropdown:focus + .branch-selector::after,
        .branch-selector:hover::after {
            border-top-color: #0366d6;
        }
        
        /* 🔧 数据绑定的下拉选项样式 */
        .branch-dropdown option {
            padding: 10px 15px;
            background: white;
            color: #24292e;
            font-size: 16px;
        }
        
        /* 🔧 针对不同浏览器的下拉框优化 */
        @-moz-document url-prefix() {
            .branch-dropdown {
                padding-right: 30px;
            }
        }
        
        /* Webkit浏览器滚动条优化 */
        .branch-dropdown::-webkit-scrollbar {
            width: 10px;
        }
        
        .branch-dropdown::-webkit-scrollbar-track {
            background: #f1f1f1;
            border-radius: 5px;
        }
        
        .branch-dropdown::-webkit-scrollbar-thumb {
            background: #c1c1c1;
            border-radius: 5px;
        }
        
        .branch-dropdown::-webkit-scrollbar-thumb:hover {
            background: #a8a8a8;
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
        
        /* 🔧 确保主内容区域使用所有可用空间 */
        .main-content > div:first-child {
            min-width: 0;                      
            width: 100%;                       
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
        
        /* ========== 放大的按钮样式 ========== */
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
        
        /* 🔧 按钮图标大小 */
        .btn span {
            font-size: 18px;          
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
        
        /* ========== 🚧 未来功能标记 ========== */
        .future-feature {
            position: relative;
            opacity: 0.6;
            cursor: not-allowed;
        }
        
        .future-feature::after {
            content: '🚧 Coming Soon';
            position: absolute;
            top: -30px;
            left: 50%;
            transform: translateX(-50%);
            background: #fff3cd;
            color: #856404;
            padding: 4px 8px;         
            border-radius: 4px;
            font-size: 12px;          
            white-space: nowrap;
            opacity: 0;
            transition: opacity 0.2s;
        }
        
        .future-feature:hover::after {
            opacity: 1;
        }
        
        /* ========== 🔄 预留功能区域 ========== */
        .reserved-section {
            border: 2px dashed #d1d5da;
            border-radius: 6px;
            padding: 25px;            
            margin: 20px 0;
            text-align: center;
            color: #586069;
            background: #f8f9fa;
        }
        
        .reserved-section h4 {
            margin: 0 0 10px 0;
            color: #24292e;
            font-size: 16px;          
        }
        
        .reserved-section p {
            margin: 0;
            font-size: 14px;          
        }
        
        /* ========== 🔧 强制覆盖可能的父级限制 ========== */
        body .repo-container {
            max-width: none !important;
            width: 78% !important;
        }
        
        body .main-content {
            width: 100% !important;
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AppHeaderStartAdd" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AppHeaderEndAdd" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="mainContainer" runat="server">
    <div class="repo-container">
        <!-- 🔧 数据绑定的仓库头部 -->
        <div class="repo-header">
            <h1 style="margin: 0; font-size: 24px;">
                <!-- 🔧 数据绑定：仓库所有者和名称 -->
                <asp:Literal ID="litOwnerName" runat="server" Text="CandyLedge" />
                <span style="color: #586069;"> / </span>
                <asp:Literal ID="litRepoName" runat="server" Text="CodingRep" />
                
                <!-- 🔧 数据绑定：动态Public/Private状态 -->
                <asp:Label ID="lblRepoStatus" runat="server" CssClass="repo-status public" Text="Public" />
            </h1>
            
            <!-- 🔧 数据绑定：仓库描述 -->
            <p class="repo-meta">
                <asp:Literal ID="litRepoDescription" runat="server" Text="A coding repository for various projects and experiments" />
            </p>
            
            <div class="repo-stats">
                <!-- 🔧 数据绑定：创建日期、提交数、主要语言 -->
                <span>📅 Created <asp:Literal ID="litCreateDate" runat="server" Text="2024-01-15" /></span>
                <span>📝 <asp:Literal ID="litCommitCount" runat="server" Text="5" /> commits</span>
                <span style="color: #f1e05a;">● <asp:Literal ID="litMainLanguage" runat="server" Text="JavaScript" /></span>
            </div>
        </div>

        <!-- 🔧 导航栏 -->
        <div class="repo-nav">
            <div class="nav-tabs">
                <a href="#" class="nav-tab active">📄 Code</a>
                <a href="#" class="nav-tab">📝 Commits</a>
                <a href="#" class="nav-tab">🌿 Branches</a>
                <!-- 🚧 未来功能标记 -->
                <a href="#" class="nav-tab future-feature">⚡ Actions</a>
                <a href="#" class="nav-tab future-feature">🛡 Security</a>
            </div>
        </div>

        <!-- 🔧 居中的主要内容区域 -->
        <div class="main-content">
            <!-- 🔧 左侧主内容 -->
            <div>
                <!-- 🔧 文件浏览器 -->
                <div class="file-browser">
                    <div class="file-header">
                        <div class="file-header-left">
                            <span style="font-size: 16px;">🌿</span>
                            
                            <!-- 🔧 TODO: 在Page_Load中绑定分支数据，添加OnSelectedIndexChanged事件 -->
                            <div class="branch-selector">
                                <asp:DropDownList ID="ddlBranches" runat="server" CssClass="branch-dropdown" AutoPostBack="true">
                                    <asp:ListItem Text=" main" Value="main" Selected="True" />
                                    <asp:ListItem Text=" develop" Value="develop" />
                                    <asp:ListItem Text=" feature/new-ui" Value="feature" />
                                    <asp:ListItem Text=" hotfix/bug-fix" Value="hotfix" />
                                </asp:DropDownList>
                            </div>
                            
                            <!-- 🔧 数据绑定：提交数量 -->
                            <span class="commit-info">• <asp:Literal ID="litBranchCommits" runat="server" Text="5" /> commits</span>
                        </div>
                        <div class="file-header-right">
                            <!-- 🔧 TODO: 添加OnClick事件 -->
                            <asp:Button ID="btnUpload" runat="server" Text="📤 Upload" CssClass="btn btn-upload" />
                            <asp:Button ID="btnDownload" runat="server" Text="📥 Download" CssClass="btn btn-primary" />
                        </div>
                    </div>
                    
                    <!-- 🔧 数据绑定的文件列表 -->
                    <div class="file-list">
                        <!-- 🔧 示例静态数据，用于预览效果 -->
                        <div class="file-item">
                            <div style="display: flex; align-items: center; gap: 10px;">
                                <span style="font-size: 16px;">📁</span>
                                <a href="#" class="file-name">src</a>
                            </div>
                            <div class="file-meta">
                                <span>Initial commit</span> • 
                                <span>CandyLedge</span> • 
                                <span>2024-01-15 10:30</span>
                            </div>
                        </div>
                        <div class="file-item">
                            <div style="display: flex; align-items: center; gap: 10px;">
                                <span style="font-size: 16px;">📄</span>
                                <a href="#" class="file-name">README.md</a>
                            </div>
                            <div class="file-meta">
                                <span>Update README</span> • 
                                <span>CandyLedge</span> • 
                                <span>2024-01-16 14:20</span>
                            </div>
                        </div>
                        <div class="file-item">
                            <div style="display: flex; align-items: center; gap: 10px;">
                                <span style="font-size: 16px;">⚙️</span>
                                <a href="#" class="file-name">package.json</a>
                            </div>
                            <div class="file-meta">
                                <span>Add dependencies</span> • 
                                <span>CandyLedge</span> • 
                                <span>2024-01-17 09:15</span>
                            </div>
                        </div>
                        <div class="file-item">
                            <div style="display: flex; align-items: center; gap: 10px;">
                                <span style="font-size: 16px;">🎨</span>
                                <a href="#" class="file-name">styles.css</a>
                            </div>
                            <div class="file-meta">
                                <span>Update styling</span> • 
                                <span>CandyLedge</span> • 
                                <span>2024-01-18 16:45</span>
                            </div>
                        </div>
                        
                        <!-- 🔧 实际的Repeater控件（数据绑定时使用） -->
                        <asp:Repeater ID="rptFiles" runat="server" Visible="false">
                            <ItemTemplate>
                                <div class="file-item">
                                    <div style="display: flex; align-items: center; gap: 10px;">
                                        <span style="font-size: 16px;"><%# Eval("FileIcon") %></span>
                                        <asp:LinkButton ID="lnkFileName" runat="server" CssClass="file-name" 
                                            Text='<%# Eval("FileName") %>' CommandArgument='<%# Eval("FileId") %>' />
                                    </div>
                                    <div class="file-meta">
                                        <span><%# Eval("LastCommitMessage") %></span> • 
                                        <span><%# Eval("LastCommitAuthor") %></span> • 
                                        <span><%# Eval("LastCommitTime", "{0:yyyy-MM-dd HH:mm}") %></span>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                        
                        <!-- 🔧 空状态面板 -->
                        <asp:Panel ID="pnlNoFiles" runat="server" Visible="false" CssClass="empty-state">
                            <div class="empty-state-icon">📁</div>
                            <h3>This repository is empty</h3>
                            <p>Upload your first files to get started!</p>
                        </asp:Panel>
                    </div>
                </div>

                <!-- 🔧 🔄 预留的代码编辑器区域 -->
                <div class="reserved-section">
                    <h4>🔄 Code Editor (Reserved)</h4>
                    <p>Future: Inline code editing and preview functionality</p>
                </div>

                <!-- 🔧 数据绑定的最近提交记录 -->
                <div class="commits-section">
                    <h3 style="margin: 0 0 15px 0; font-size: 18px; font-weight: 600; display: flex; align-items: center; gap: 10px;">
                        <span>📝</span>
                        Recent Commits
                    </h3>
                    
                    <!-- 🔧 示例静态数据，用于预览效果 -->
                    <div class="commit-item">
                        <div class="commit-message">
                            Add new feature for file upload
                        </div>
                        <div class="commit-meta">
                            <span>👤 CandyLedge</span> • 
                            <span>🕒 2024-01-18 16:45</span>
                        </div>
                    </div>
                    <div class="commit-item">
                        <div class="commit-message">
                            Fix bug in branch switching
                        </div>
                        <div class="commit-meta">
                            <span>👤 CandyLedge</span> • 
                            <span>🕒 2024-01-17 11:30</span>
                        </div>
                    </div>
                    <div class="commit-item">
                        <div class="commit-message">
                            Update documentation and improve UI
                        </div>
                        <div class="commit-meta">
                            <span>👤 CandyLedge</span> • 
                            <span>🕒 2024-01-16 14:20</span>
                        </div>
                    </div>
                    
                    <!-- 🔧 实际的Repeater控件（数据绑定时使用） -->
                    <asp:Repeater ID="rptCommits" runat="server" Visible="false">
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
                    
                    <!-- 🔧 无提交记录面板 -->
                    <asp:Panel ID="pnlNoCommits" runat="server" Visible="false" CssClass="empty-state">
                        <div class="empty-state-icon">📝</div>
                        <h3>No commits yet</h3>
                        <p>Start by uploading your first files!</p>
                    </asp:Panel>
                </div>
            </div>

            <!-- 🔧 右侧侧边栏 - 数据绑定 -->
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

                    <!-- 🔧 数据绑定的分支信息 -->
                    <div class="sidebar-section">
                        <div class="sidebar-title">🌿 Branches</div>
                        <p style="font-size: 13px; color: #586069; margin-bottom: 10px;">
                            <asp:Literal ID="litBranchCount" runat="server" Text="4" /> branches
                        </p>
                        <!-- 🔧 示例静态数据 -->
                        <div style="font-size: 13px; line-height: 1.5;">
                            <div style="margin-bottom: 3px;">🌟 main (default)</div>
                            <div style="margin-bottom: 3px;">🚀 develop</div>
                            <div style="margin-bottom: 3px;">✨ feature/new-ui</div>
                            <div>🔧 hotfix/bug-fix</div>
                        </div>
                        
                        <!-- 🔧 实际的Repeater控件（数据绑定时使用） -->
                        <asp:Repeater ID="rptBranches" runat="server" Visible="false">
                            <ItemTemplate>
                                <div style="margin-bottom: 3px;">
                                    <%# Eval("BranchIcon") %> <%# Eval("BranchDisplayName") %>
                                    <%# Eval("IsDefault").ToString() == "True" ? " (default)" : "" %>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                    </div>

                    <!-- 🔧 数据绑定的活动信息 -->
                    <div class="sidebar-section">
                        <div class="sidebar-title">📝 Activity</div>
                        <p style="font-size: 13px; color: #586069; margin-bottom: 6px;">
                            <asp:Literal ID="litTotalCommits" runat="server" Text="5" /> commits total
                        </p>
                        <p style="font-size: 13px; color: #586069;">
                            Last updated <asp:Literal ID="litLastUpdate" runat="server" Text="2 hours ago" />
                        </p>
                    </div>

                    <!-- 🔧 数据绑定的所有者信息 -->
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

                    <!-- 🚧 未来功能区域 -->
                    <div class="sidebar-section future-feature">
                        <div class="sidebar-title">🚧 Releases</div>
                        <p style="font-size: 13px; color: #586069;">Coming soon...</p>
                    </div>

                    <!-- 🔧 数据绑定的语言统计 -->
                    <div class="sidebar-section">
                        <div class="sidebar-title">💻 Languages</div>
                        <div class="language-bar">
                            <!-- 🔧 示例静态语言条 -->
                            <div class="language-segment" style="width: 85.2%; background: #f1e05a;"></div>
                            <div class="language-segment" style="width: 10.1%; background: #e34c26;"></div>
                            <div class="language-segment" style="width: 4.7%; background: #563d7c;"></div>
                        </div>
                        
                        <!-- 🔧 示例静态语言列表 -->
                        <div style="font-size: 13px; line-height: 1.5;">
                            <div style="display: flex; justify-content: space-between; margin-bottom: 4px;">
                                <span><span style="color: #f1e05a;">●</span> JavaScript</span>
                                <span>85.2%</span>
                            </div>
                            <div style="display: flex; justify-content: space-between; margin-bottom: 4px;">
                                <span><span style="color: #e34c26;">●</span> HTML</span>
                                <span>10.1%</span>
                            </div>
                            <div style="display: flex; justify-content: space-between;">
                                <span><span style="color: #563d7c;">●</span> CSS</span>
                                <span>4.7%</span>
                            </div>
                        </div>
                        
                        <!-- 🔧 实际的Repeater控件（数据绑定时使用） -->
                        <asp:Repeater ID="rptLanguages" runat="server" Visible="false">
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
</asp:Content>