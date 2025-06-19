<%@ Page Title="" Language="C#" MasterPageFile="~/src/motherboard/AppHeaderGeneralBar.master" AutoEventWireup="true" CodeBehind="CreateRepo.aspx.cs" Inherits="CodingRep.src.views.CreateRepo" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .repo-create-container {
            max-width: 1200px;
            margin: 0 auto;
            padding: 32px 24px;
        }

        .page-header {
            margin-bottom: 32px;
        }

        .page-title {
            font-size: 32px;
            font-weight: 600;
            color: #24292f;
            margin: 0 0 8px 0;
        }

        .page-subtitle {
            color: #656d76;
            font-size: 16px;
            margin: 0;
        }

        .form-group {
            margin-bottom: 24px;
        }

        .form-group:last-child {
            margin-bottom: 0;
        }

        .form-label {
            display: block;
            font-weight: 600;
            font-size: 16px;
            color: #24292f;
            margin-bottom: 8px;
        }

        .required::after {
            content: " *";
            color: #cf222e;
        }

        .form-input {
            width: 100%;
            padding: 10px 14px;
            font-size: 16px;
            line-height: 24px;
            color: #24292f;
            background-color: #ffffff;
            border: 1px solid #d0d7de;
            border-radius: 6px;
            box-sizing: border-box;
        }

        .form-input:focus {
            border-color: #0969da;
            outline: none;
            box-shadow: inset 0 1px 0 rgba(208, 215, 222, 0.2), 0 0 0 2px rgba(9, 105, 218, 0.3);
        }

        .form-textarea {
            width: 100%;
            padding: 10px 14px;
            font-size: 16px;
            line-height: 24px;
            color: #24292f;
            background-color: #ffffff;
            border: 1px solid #d0d7de;
            border-radius: 6px;
            resize: vertical;
            min-height: 120px;
            font-family: -apple-system,BlinkMacSystemFont,"Segoe UI",Helvetica,Arial,sans-serif;
            box-sizing: border-box;
        }

        .form-textarea:focus {
            border-color: #0969da;
            outline: none;
            box-shadow: inset 0 1px 0 rgba(208, 215, 222, 0.2), 0 0 0 2px rgba(9, 105, 218, 0.3);
        }

        .owner-select {
            display: flex;
            align-items: center;
            padding: 14px 18px;
            background-color: #f6f8fa;
            border: 1px solid #d0d7de;
            border-radius: 6px;
        }

        .owner-avatar {
            width: 28px;
            height: 28px;
            border-radius: 50%;
            background-color: #0969da;
            display: flex;
            align-items: center;
            justify-content: center;
            color: white;
            font-size: 14px;
            font-weight: 600;
            margin-right: 14px;
        }

        .owner-name {
            font-weight: 600;
            font-size: 16px;
            color: #24292f;
        }

        .visibility-section {
            border: 1px solid #d0d7de;
            border-radius: 6px;
            overflow: hidden;
        }

        .visibility-option {
            display: flex;
            align-items: flex-start;
            padding: 22px;
            border-bottom: 1px solid #d0d7de;
            cursor: pointer;
            transition: background-color 0.2s;
        }

        .visibility-option:last-child {
            border-bottom: none;
        }

        .visibility-option:hover {
            background-color: #f6f8fa;
        }

        .visibility-option.selected {
            background-color: #dbeafe;
            border-color: #0969da;
        }

        .visibility-radio {
            margin-right: 18px;
            margin-top: 2px;
        }

        .visibility-content {
            flex: 1;
        }

        .visibility-title {
            display: flex;
            align-items: center;
            font-weight: 600;
            font-size: 16px;
            color: #24292f;
            margin-bottom: 8px;
        }

        .visibility-icon {
            margin-right: 10px;
        }

        .visibility-description {
            font-size: 14px;
            color: #656d76;
            line-height: 1.5;
        }

        .form-actions {
            margin-top: 40px;
            padding-top: 24px;
            border-top: 1px solid #d0d7de;
            display: flex;
            justify-content: flex-end;
        }

        .btn {
            display: inline-block;
            padding: 10px 24px;
            font-size: 16px;
            font-weight: 500;
            line-height: 24px;
            white-space: nowrap;
            vertical-align: middle;
            cursor: pointer;
            border: 1px solid;
            border-radius: 6px;
            text-decoration: none;
        }

        .btn-primary {
            color: #ffffff;
            background-color: #1f883d;
            border-color: rgba(31, 136, 61, 0.4);
        }

        .btn-primary:hover {
            background-color: #1a7f37;
            border-color: rgba(31, 136, 61, 0.4);
        }

        .btn-primary:disabled {
            color: rgba(255, 255, 255, 0.8);
            background-color: #94d3a2;
            border-color: rgba(31, 136, 61, 0.4);
            cursor: not-allowed;
        }

        .help-text {
            font-size: 14px;
            color: #656d76;
            margin-top: 8px;
            line-height: 1.4;
        }

        @media (max-width: 768px) {
            .repo-create-container {
                padding: 24px 16px;
                max-width: 100%;
            }
            
            .page-title {
                font-size: 28px;
            }
            
            .visibility-option {
                padding: 16px;
            }
            
            .form-actions {
                justify-content: center;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="AppHeaderStartAdd" runat="server">
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="AppHeaderEndAdd" runat="server">
</asp:Content>

<asp:Content ID="Content4" ContentPlaceHolderID="mainContainer" runat="server">
    <div class="repo-create-container">
        <div class="page-header">
            <h1 class="page-title">创建新仓库</h1>
            <p class="page-subtitle">仓库包含项目的所有文件，包括修订历史记录。</p>
        </div>
        
        <!-- 所有者 -->
        <div class="form-group">
            <label class="form-label">所有者</label>
            <div class="owner-select">
                <div class="owner-avatar">
                    <asp:Literal ID="litOwnerInitial" runat="server" />
                </div>
                <span class="owner-name">
                    <asp:Literal ID="litOwnerName" runat="server" />
                </span>
            </div>
        </div>
        
        <!-- 仓库名称 -->
        <div class="form-group">
            <label class="form-label required">仓库名称</label>
            <asp:TextBox ID="txtRepoName" runat="server" CssClass="form-input" 
                placeholder="请输入仓库名称" MaxLength="100" />
            <div class="help-text">优秀的仓库名称简洁而富有意义。</div>
        </div>
        
        <!-- 描述 -->
        <div class="form-group">
            <label class="form-label">描述 <span style="color: #656d76; font-weight: normal;">(可选)</span></label>
            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" 
                CssClass="form-textarea" placeholder="简短描述您的项目..." MaxLength="500" />
        </div>
        
        <!-- 可见性 -->
        <div class="form-group">
            <label class="form-label">可见性</label>
            <div class="visibility-section">
                <div class="visibility-option selected" onclick="selectVisibility('public', this)">
                    <asp:RadioButton ID="rbPublic" runat="server" GroupName="visibility" 
                        CssClass="visibility-radio" Checked="true" />
                    <div class="visibility-content">
                        <div class="visibility-title">
                            <span class="visibility-icon">🌐</span>
                            公开
                        </div>
                        <div class="visibility-description">
                            互联网上的任何人都可以查看此仓库。您可以选择谁可以提交代码。
                        </div>
                    </div>
                </div>
                
                <div class="visibility-option" onclick="selectVisibility('private', this)">
                    <asp:RadioButton ID="rbPrivate" runat="server" GroupName="visibility" 
                        CssClass="visibility-radio" />
                    <div class="visibility-content">
                        <div class="visibility-title">
                            <span class="visibility-icon">🔒</span>
                            私有
                        </div>
                        <div class="visibility-description">
                            您可以选择谁可以查看和提交到此仓库。
                        </div>
                    </div>
                </div>
            </div>
        </div>
        
        <!-- 操作按钮 -->
        <div class="form-actions">
            <asp:Button ID="btnCreate" runat="server" Text="创建仓库" 
                CssClass="btn btn-primary" OnClick="btnCreate_Click" />
        </div>
    </div>
    
    <script type="text/javascript">
        function selectVisibility(type, element) {
            // 移除所有选中状态
            var options = document.querySelectorAll('.visibility-option');
            options.forEach(function (option) {
                option.classList.remove('selected');
            });

            // 添加选中状态
            element.classList.add('selected');

            // 设置对应的单选按钮
            if (type === 'public') {
                document.getElementById('<%= rbPublic.ClientID %>').checked = true;
            } else {
                document.getElementById('<%= rbPrivate.ClientID %>').checked = true;
            }
        }

        // 添加仓库名称实时验证
        document.addEventListener('DOMContentLoaded', function() {
            var repoNameInput = document.getElementById('<%= txtRepoName.ClientID %>');
            var createButton = document.getElementById('<%= btnCreate.ClientID %>');
            
            if (repoNameInput) {
                // 创建错误提示元素
                var errorDiv = document.createElement('div');
                errorDiv.className = 'error-message';
                errorDiv.style.color = '#cf222e';
                errorDiv.style.fontSize = '14px';
                errorDiv.style.marginTop = '8px';
                errorDiv.style.display = 'none';
                repoNameInput.parentNode.insertBefore(errorDiv, repoNameInput.nextSibling.nextSibling);
                
                repoNameInput.addEventListener('input', function() {
                    var value = this.value.trim();
                    var isValid = true;
                    var errorMessage = '';
                    
                    if (value.length === 0) {
                        isValid = false;
                        errorMessage = '请输入仓库名称';
                    } else if (value.length > 100) {
                        isValid = false;
                        errorMessage = '仓库名称过长（最多100个字符）';
                    } else if (!/^[a-zA-Z0-9._-]+$/.test(value)) {
                        isValid = false;
                        errorMessage = '仓库名称只能包含字母、数字、点号、连字符和下划线';
                    }
                    
                    if (isValid) {
                        this.style.borderColor = '#d0d7de';
                        errorDiv.style.display = 'none';
                        createButton.disabled = false;
                    } else {
                        this.style.borderColor = '#cf222e';
                        errorDiv.textContent = errorMessage;
                        errorDiv.style.display = 'block';
                        createButton.disabled = true;
                    }
                });
            }
        });
    </script>
</asp:Content>
