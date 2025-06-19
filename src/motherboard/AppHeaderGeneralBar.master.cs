using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodingRep.src.motherboard
{
    public partial class AppHeaderGeneralBar : System.Web.UI.MasterPage
    {
        protected void Page_Init(object sender, EventArgs e)
        {
            if (Session["userId"] == null)
            {
                Response.Redirect("../views/Index.aspx");
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
    }
}