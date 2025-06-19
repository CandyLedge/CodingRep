using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodingRep.App_Code;
using CodingRep.service;
using CodingRep.service.tool.impl;
using CodingRep.utils;

namespace CodingRep.src.views
{
    public partial class Register : System.Web.UI.Page
    {
        private ModelDb db = new ModelDb();
        private string  verificationCode;
        private DateTime verificationCodeCollingTime;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }  
        
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            verificationCode = ViewState["verificationCode"].ToString();
            var user = db.users.Where(u => u.userName == txtUsername.Text).ToList(); //查找是否重复账号
            if (user.Any())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('用户已存在');", true);
            
            }
            else
            {
                if (verificationCode == null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请获取验证码');", true);
                }
                else if (txtVerificationCode.Text == null)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请输入邮箱验证码');", true);
                }
                else if (verificationCode != txtVerificationCode.Text)
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('邮箱验证码错误');", true);
                }
                else
                {
                    var passwordData = PasswordUtil.Hash(txtPassword.Text);
                    var newUser      = new users();
                    newUser.userName     = txtUsername.Text;
                    newUser.salt         = passwordData.Salt;
                    newUser.passwordHash = passwordData.Hash;
                    newUser.email        = txtEmail.Text;
                    newUser.createdAt    = DateTime.Now;
                    newUser.email        = txtEmail.Text;
                    db.users.Add(newUser);
                    db.SaveChanges();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('注册成功');", true);
                    txtUsername.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtPassword.Text = string.Empty;
                    txtVerificationCode.Text = string.Empty;
                    lblVerificationCode.Text = string.Empty;
                }
            }
        }

        protected void btnGetVerificationCode_Click(object sender, EventArgs e)
        {
            ViewState["SavePassword"] = txtPassword.Text;
            var user = db.users.Where(u => u.email == txtEmail.Text).ToList(); //查找是否重复邮箱
            if (user.Any())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('邮箱已注册');", true);
            }
            else
            {
                if (string.IsNullOrEmpty(txtEmail.Text))
                {
                    lblVerificationCode.Text = "请输入邮箱";
                }
                else if (verificationCodeCollingTime == default || (DateTime.Now - verificationCodeCollingTime).TotalMinutes >= 5)
                {
                    verificationCodeCollingTime   = DateTime.Now;
                    verificationCode              = EmailVerificationService.sendVerificationCode(txtEmail.Text);
                    ViewState["verificationCode"] = verificationCode;
                    lblVerificationCode.Text      = "邮件已发送";
                }
                else
                {
                    lblVerificationCode.Text = "请勿频繁获取验证码";
                }
            }
        }
    }
}