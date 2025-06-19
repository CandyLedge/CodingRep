using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodingRep.App_Code;
using CodingRep.service;
using CodingRep.utils;

namespace CodingRep.src.views
{
    public partial class Register : System.Web.UI.Page
    {
        private ModelDb db = new ModelDb();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnRegister_Click(object sender, EventArgs e)
        {
            var user = db.users.Where(u => u.userName == txtUsername.Text).ToList(); //查找是否重复账号
            if (user.Count()>0)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('用户已存在');", true);
            
            }
            else
            {
                var  passwordData = PasswordUtil.Hash(txtPassword.Text);
                var newUser = new users();
                newUser.userName     = txtUsername.Text;
                newUser.salt         = passwordData.Salt;
                newUser.passwordHash = passwordData.Hash;
                newUser.email        = txtEmail.Text;
                newUser.createdAt    = DateTime.Now;
                newUser.email        = txtEmail.Text;
                db.users.Add(newUser);
                db.SaveChanges();
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('注册成功');", true);
            }
        }
    }
}