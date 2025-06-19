using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
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
        private readonly ConcurrentDictionary<string, (string Code, DateTime SendTime)> codeCache 
            = new ConcurrentDictionary<string, (string,DateTime)>();

        private readonly int expiryMinutes = 5;
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }  
        
        protected void btnRegister_Click(object sender, EventArgs e)
        {
            txtVerificationCode.Enabled = true;
            string email = txtEmail.Text.Trim();
            string inputCode = txtVerificationCode.Text.Trim();

            if (!codeCache.TryGetValue(email, out var cachedCode))
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('请先获取验证码');", true);
                return;
            }

            if ((DateTime.Now - cachedCode.SendTime).TotalMinutes > expiryMinutes)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('验证码已过期，请重新获取');", true);
                return;
            }

            if (inputCode != cachedCode.Code)
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('邮箱验证码错误');", true);
                return;
            }

            // 验证后注册
            var user = db.users.Where(u => u.userName == txtUsername.Text).ToList();
            if (user.Any())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('用户已存在');", true);
                return;
            }

            var passwordData = PasswordUtil.Hash(txtPassword.Text);
            var newUser = new users
            {
                userName = txtUsername.Text,
                salt = passwordData.Salt,
                passwordHash = passwordData.Hash,
                email = email,
                createdAt = DateTime.Now
            };

            db.users.Add(newUser);
            db.SaveChanges();
            codeCache.TryRemove(email, out _); // 注册成功后移除验证码

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('注册成功');", true);
            txtUsername.Text = txtEmail.Text = txtPassword.Text = txtVerificationCode.Text = "";
            lblVerificationCode.Text = "";
        }

        protected void btnGetVerificationCode_Click(object sender, EventArgs e)
        {
            string email = txtEmail.Text.Trim();

            // 查重
            var existingUsers = db.users.Where(u => u.email == email).ToList();
            if (existingUsers.Any())
            {
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('邮箱已注册');", true);
                return;
            }

            // 从 Session 读取缓存
            var cached = Session[email] as Tuple<string, DateTime>;

            if (cached != null && (DateTime.Now - cached.Item2).TotalMinutes < expiryMinutes)
            {
                lblVerificationCode.Text = "请勿频繁获取验证码";
                return;
            }

            string code = EmailVerificationService.sendVerificationCode(email);

            if (!string.IsNullOrEmpty(code))
            {
                Session[email] = new Tuple<string, DateTime>(code, DateTime.Now);
                lblVerificationCode.Text = "验证码已发送";
            }
            else
            {
                lblVerificationCode.Text = "验证码发送失败";
            }
        }

    }
}