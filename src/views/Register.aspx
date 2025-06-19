<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="CodingRep.src.views.Register" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" lang="zh">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <link rel="stylesheet" href="../assets/style/Register-Style.css"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <div class="container">
            <!-- 左侧装饰区域 -->
            <div class="left-panel">
                <h1>免费创建账户</h1>
                <p>
                    免费创建一个CodingRep账户即可立即使用CodingRep的全部功能
                </p>
            </div>

            <!-- 右侧注册表单区域 -->
            <div class="right-panel">
                <table class="register-table">
                    <tr>
                        <td colspan="2">用户注册</td>
                    </tr>
                    <tr>
                        <td class="label">用户名：</td>
                        <td>
                            <asp:TextBox ID="txtUsername" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername"
                                ErrorMessage="请输入用户名" CssClass="validator-message" Text="请输入用户名" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revUsername" runat="server" ControlToValidate="txtUsername"
                                ErrorMessage="用户名必须为英文、数字或下划线，且不能以下划线开头" CssClass="validator-message" ValidationExpression="^[a-zA-Z][a-zA-Z0-9]*(_[a-zA-Z0-9]+)*$"
                                Text="用户名必须为英文、数字或下划线，且不能以下划线开头" Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">密码：</td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="请输入密码" CssClass="validator-message" Text="请输入密码" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">确认密码：</td>
                        <td>
                            <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword"
                                ErrorMessage="请确认密码" CssClass="validator-message" Text="请确认密码" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvPassword" runat="server" ControlToCompare="txtPassword"
                                ControlToValidate="txtConfirmPassword" ErrorMessage="两次输入的密码不一致" CssClass="validator-message"
                                Operator="Equal" Text="两次输入的密码不一致" Display="Dynamic"></asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">邮箱：</td>
                        <td>
                            <asp:TextBox ID="txtEmail" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="请输入邮箱地址" CssClass="validator-message" Text="请输入邮箱地址" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="revEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="邮箱格式不正确" CssClass="validator-message" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                Text="邮箱格式不正确" Display="Dynamic"></asp:RegularExpressionValidator>
                        </td>
                    </tr>
                    <tr>
                        <td class="label">验证码：</td>
                        <td> 
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <div class="verification-code-box">
                                        <asp:TextBox ID="txtVerificationCode" runat="server" CssClass="verification-code-input"></asp:TextBox>
                                        <asp:Button ID="btnGetVerificationCode" runat="server" Text="获取验证码" CssClass="verification-code-button" CausesValidation="False" OnClick="btnGetVerificationCode_Click" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <%-- TODO 邮件已发送 --%>
                            <asp:Label runat="server" ID="lblVerificationCode"></asp:Label>

                            <asp:RequiredFieldValidator ID="rfvVerificationCode" runat="server" ControlToValidate="txtVerificationCode"
                                ErrorMessage="请输入验证码" CssClass="validator-message" Text="请输入验证码" Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Button ID="btnRegister" runat="server" Text="注册" CssClass="button" OnClick="btnRegister_Click"/>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblMessage" runat="server" Text="" CssClass="label-error"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        
    </form>
</body>
</html>
