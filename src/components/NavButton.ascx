<%@ Control Language="C#" CodeBehind="NavButton.ascx.cs" Inherits="CodingRep.components.NavButton" %>

<asp:HyperLink runat="server" ID="hyLink" CssClass="nav-button">
    <img ID="imgNavIcon" runat="server" alt="Home" class="nav-icon" />
</asp:HyperLink>


<asp:Literal runat="server" ID="dynamicCss"></asp:Literal>

<style>
    .nav-button {
        display: inline-block;
        width: 30px; /* 方形的宽度 */
        height: 30px; /* 方形的高度 */
        border: 1px solid #ccc; /* 外边框 */
        text-align: center;
        position: relative;
        border-radius: 8px; /* 设置圆角 */
    }
    
        .nav-button:hover::after {

        }
    
    .nav-icon {
        width: 20px; /* 图标的宽度 */
        height: 20px; /* 图标的高度 */
        margin-top: 10px; /* 调整图标在方块中的位置 */
    }
</style>

