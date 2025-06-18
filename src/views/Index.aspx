<%@ Page Title="" Language="C#" MasterPageFile="~/src/motherboard/AppHeaderLoginBar.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CodingRep.src.views.Index" %>


<asp:Content runat="server" ContentPlaceHolderID="head">
    <link rel="stylesheet" href="../assets/style/Index-Style.css"/>
</asp:Content>


<asp:Content ID="Content1" ContentPlaceHolderID="mainContainer" runat="server">
    
    <div class="index-container">
 
        <div class="index-container-box">
            
            <div class="index-row">
                <div class="index-col-md-6">
                    <div class="index-jumbotron">
                        <h1>CodingRep</h1>
                        <p>CodingRep是一个的代码托管平台</p>
                        <p>
                            <a class="index-btn index-btn-primary index-btn-lg" href="https://github.com/CandyLedge/CodingRep/" role="button">GitHub链接</a>
                        </p>
                    </div>
                </div>
                <div class="index-col-md-6">
                    <div class="index-panel index-panel-default">
                        <div class="index-panel-body">
                            <h3>免费加入CodingRep0v0</h3>
                            <div class="index-input-group index-input-group-lg">
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="index-form-control" placeholder="请输入邮箱地址" required="true" />
                                <span class="index-input-group-btn">
                                    <asp:Button ID="btnSignUp" runat="server" Text="在CodringRep上注册" CssClass="index-btn index-btn-primary" />
                                </span>
                            </div>
                            <p class="index-help-block">已经有账号了? <a href="#">登录</a></p>
                        </div>
                    </div>
                </div>
            </div>
            
        </div>
        
    </div>
</asp:Content>
