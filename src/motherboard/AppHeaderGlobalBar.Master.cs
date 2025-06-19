using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodingRep.src.motherboard
{
    public partial class AppHeaderGlobalBar : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["userId"] == null) hlIcon.NavigateUrl = "../views/Index.aspx";
            else hlIcon.NavigateUrl = "../views/DashBoard.aspx";
        }
    }
}