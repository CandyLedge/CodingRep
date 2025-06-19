<%@ Page Title="" Language="C#" MasterPageFile="~/src/motherboard/AppHeaderGeneralBar.master" AutoEventWireup="true" CodeBehind="DashBoard.aspx.cs" Inherits="CodingRep.src.views.DashBoard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../assets/style/DashBoard-Style.css"/>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="AppHeaderStartAdd" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="AppHeaderEndAdd" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="mainContainer" runat="server">
    
<div class="dashboard-container">
    <div class="dashboard-sidebar sidebar-left">
        <!-- 仓库列表 -->
        <h2>仓库列表</h2>
        <ul id="repoList" runat="server">
            <!-- 动态内容占位符 -->
        </ul>
    </div>
    <div class="dashboard-content">
        <!-- 所有提交记录 -->
        <h2>提交记录</h2>
        <div id="commitRecords" runat="server">
            <!-- 动态内容占位符 -->
        </div>
    </div>
    
    <div class="dashboard-sidebar sidebar-right">
            <!-- 仓库列表 -->
            <h2>没想好放啥</h2>
        </div>
</div>
    
    
</asp:Content>
