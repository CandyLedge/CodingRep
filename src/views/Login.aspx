<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CodingRep.src.views.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="../assets/style/Login-Style.css" rel="stylesheet"/>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="login-container">
                <div class="display-area">
                    <img src="display_image_url_here" alt="Display Image" style="max-width: 100%;"/> <!-- 替换为实际展示图片URL -->
                </div>
                <div class="input-area">
                    <h2>CodingRep 登录</h2>
                    <label>账户</label>
                    <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                    <label>密码</label>
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                    <div style="text-align: center;">
                        <asp:Button ID="btnLogin" runat="server" Text="登录" CssClass="login-btn" OnClick="loginBtn_Click" />
                        <asp:HyperLink ID="hlRegister" runat="server" NavigateUrl="Register.aspx" CssClass="hlRegister">前往注册</asp:HyperLink>
                    </div>
                    <div style="text-align: center; margin-top: 10px;">
                        <asp:HyperLink ID="hlForgotLogin" runat="server" NavigateUrl="#" CssClass="forgot-link">忘记登录信息</asp:HyperLink>
                    </div>
                </div>
            </div>
        </div>
    </form>
</body>
</html>
