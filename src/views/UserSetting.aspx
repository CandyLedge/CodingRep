<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSetting.aspx.cs" Inherits="CodingRep.src.views.UserSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link rel="stylesheet" href="../assets/style/UserSetting-iframe-Style.css"/>
</head>
<body>
    <form id="form1" runat="server">
        <div class="user-setting-avatar">
            <asp:TextBox ID="avatarUrlInput" runat="server" placeholder="请输入头像URL" CssClass="user-setting-avatar__input"></asp:TextBox>
            <asp:Button ID="previewBtn" runat="server" Text="预览头像" OnClick="previewBtn_Click" CssClass="user-setting-avatar__preview-btn" />
            <div class="user-setting-avatar__preview-container">
                <p>预览头像：</p>
                <img id="previewAvatar" runat="server" alt="预览头像" class="user-setting-avatar__preview-image" style="max-width: 150px; max-height: 150px;" />
            </div>
            <div class="user-setting-avatar__preview-container">
                <asp:Button ID="uploadBtn" runat="server" Text="保存头像" OnClick="uploadBtn_Click" CssClass="user-setting-avatar__upload-btn" />
                <p>当前头像：</p>
                <img id="currentAvatar" runat="server" alt="当前头像" class="user-setting-avatar__current-image" />
            </div>
        </div>
    </form>
</body>
</html>
