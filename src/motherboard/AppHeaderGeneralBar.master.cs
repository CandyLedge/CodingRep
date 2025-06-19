using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodingRep.App_Code;

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
            if (Session["userId"] != null)
            {
                int userId = (int)Session["userId"];
                using (ModelDb db = new ModelDb())
                {
                    users user = db.users.Find(userId);
                    if (user != null && !string.IsNullOrEmpty(user.avatarUrl))
                    {
                        userAvatar.Src = user.avatarUrl; 
                    }
                }
            }
        }

        protected void btnPersonalRepo_Click(object sender, EventArgs e)
        {
            Response.Redirect("../views/DashBoard.aspx");
        }

        protected void btnSettings_Click(object sender, EventArgs e)
        {
            string iframeUrl = "../views/UserSetting.aspx"; // 设置要显示的iframe页面地址
            string script = $@"
                var mask = document.createElement('div');
                mask.style.position = 'fixed';
                mask.style.top = '0';
                mask.style.left = '0';
                mask.style.width = '100%';
                mask.style.height = '100%';
                mask.style.backgroundColor = 'rgba(0, 0, 0, 0.5)';
                mask.style.zIndex = '9999';
                mask.onclick = function() {{ 
                    if (mask.parentNode) {{
                        mask.parentNode.removeChild(mask);
                    }}
                }};

                var iframe = document.createElement('iframe');
                iframe.src = '{iframeUrl}';
                iframe.style.position = 'fixed';
                iframe.style.top = '20%';
                iframe.style.left = '50%';
                iframe.style.transform = 'translateX(-50%)';
                iframe.style.width = '45%';
                iframe.style.height = '60%';
                iframe.style.zIndex = '10000';
                iframe.style.border = 'none';

                mask.appendChild(iframe);
                document.body.appendChild(mask);
            ";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowIframe", script, true);
        }
        
        

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("../views/Index.aspx");
        }
    }
}