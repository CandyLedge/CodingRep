using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodingRep.App_Code;
using CodingRep.utils;

namespace CodingRep.src.views
{
    public partial class Login : System.Web.UI.Page
    {
        private ModelDb db = new ModelDb();
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void loginBtn_Click(object sender, EventArgs e)
        {
            var result = db.users.Where(u => u.userName == txtUsername.Text);
            if (result.Any())
            {
                var user = (result.First().salt, result.First().passwordHash);
                if (PasswordUtil.VerifyPassword(txtPassword.Text, user.Item1, user.Item2))
                {
                    Session["userId"] = result.First().id;
                    Response.Redirect("~/src/views/Repo.aspx");
                }
                else
                {
                    badUser();
                }
            }
            else
            {
                badUser();
            }
        }

        private void badUser()
        {
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('账号或密码错误');", true);
        }
    }
}