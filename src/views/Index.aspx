<%@ Page Title="" Language="C#" MasterPageFile="~/src/motherboard/AppHeaderLoginBar.master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="CodingRep.src.views.Index" %>


<asp:Content runat="server" ContentPlaceHolderID="head"></asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="mainContainer" runat="server">
    
    <div class="container">
        <div class="row">
            <div class="col-md-12">
                <div class="jumbotron">
                    <h1>CodingRep</h1>
                    <p>CodingRep是一个的代码托管平台</p>
                    <p><a class="btn btn-primary btn-lg" href="https://github.com/CandyLedge/CodingRep/" role="button">GitHub</a></p>
                </div>
            </div>
        </div>
        
    <div class="row">
        <div class="col-md-8 col-md-offset-2">
            <div class="panel panel-default">
                <div class="panel-body">
                    <h3>Join GitHub · It's free!</h3>
                    <form runat="server">
                        <div class="input-group input-group-lg">
                            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="请输入邮箱地址" required="true"/>
                            <span class="input-group-btn">
                                <asp:Button ID="btnSignUp" runat="server" Text="Sign up for CodingRep" CssClass="btn btn-primary" />
                            </span>
                        </div>
                        <p class="help-block">Already have an account? <a href="#">Sign in</a></p>
                    </form>
                </div>
            </div>
        </div>
    </div>
        
    </div>
</asp:Content>
