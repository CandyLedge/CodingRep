using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CodingRep.App_Code;

namespace CodingRep.src.views
{
    public partial class UserSetting : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["UserId"] == null)
                {
                    // 如果用户未登录（没有UserId），跳转到登录页面
                    Response.Redirect("Index.aspx"); // 假设登录页面路径为Login.aspx
                    return; // 停止后续代码执行
                }
                // 获取当前登录用户ID（假设已通过Session存储）
                int userId = (int)Session["UserId"];

                using (var db = new ModelDb())
                {
                    var user = db.users.Find(userId);
                    if (user != null && !string.IsNullOrEmpty(user.avatarUrl))
                    {
                        // 设置当前头像显示
                        currentAvatar.Src = user.avatarUrl;
                        // 同步设置预览框的初始值
                        avatarUrlInput.Text = user.avatarUrl;
                    }
                }
            }
            
        }

        protected void uploadBtn_Click(object sender, EventArgs e)
        {
            
        string avatarUrl = avatarUrlInput.Text.Trim();
            if (!string.IsNullOrEmpty(avatarUrl))
            {
                // 获取当前登录用户ID（假设已通过Session存储）
                int userId = (int)Session["UserId"];

                using (var db = new ModelDb())
                {
                    var user = db.users.Find(userId);
                    if (user != null)
                    {
                        // 更新头像URL
                        user.avatarUrl = avatarUrl;
                        db.SaveChanges();

                        // 更新页面显示的头像
                        currentAvatar.Src = avatarUrl;
                    }
                }
            }
        }

        protected void previewBtn_Click(object sender, EventArgs e)
        {

            string avatarUrl = avatarUrlInput.Text.Trim();
            if (!string.IsNullOrEmpty(avatarUrl))
            {
                // 设置预览头像的URL
                previewAvatar.Src = avatarUrl;
            }
        }
    }
}