<%@ Control Language="C#" CodeBehind="AppHeader.ascx.cs" Inherits="CodingRep.components.AppHeader" %>

<header>
    <div id="AppHeader-start">
        <div id="AppHeader-logo">
            <asp:HyperLink runat="server"  NavigateUrl="~/">
                <img src="~/src/assets/" alt="CodingRep" />
            </asp:HyperLink>
        </div>
    </div>
    <div id="AppHeader-end"></div>
</header>