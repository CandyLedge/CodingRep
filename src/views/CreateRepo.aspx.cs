using CodingRep.service.query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CodingRep.src.views
{
    public partial class CreateRepo : System.Web.UI.Page
    {
        private RepositoryService _repositoryService;
        private UserService _userService;

        protected void Page_Load(object sender, EventArgs e)
        {
            _repositoryService = new RepositoryService();
            _userService = new UserService();

            if (!IsPostBack)
            {
                LoadCurrentUser();
            }
        }

        /// <summary>
        /// 加载当前用户信息
        /// </summary>
        private void LoadCurrentUser()
        {
            try
            {
                int userId = GetCurrentUserId();
                var user = _userService.GetUserById(userId);

                if (user != null)
                {
                    litOwnerName.Text = user.userName;
                    litOwnerInitial.Text = user.userName.Substring(0, 1).ToUpper();
                }
                else
                {
                    litOwnerName.Text = "Unknown User";
                    litOwnerInitial.Text = "U";
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("加载用户信息失败: " + ex.Message);
                litOwnerName.Text = "Unknown User";
                litOwnerInitial.Text = "U";
            }
        }

        /// <summary>
        /// 创建仓库按钮点击事件
        /// </summary>
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                string repoName = txtRepoName.Text.Trim();
                string description = txtDescription.Text.Trim();
                bool isPrivate = rbPrivate.Checked;
                int userId = GetCurrentUserId();

                // 创建仓库
                var result = _repositoryService.CreateRepository(userId, repoName, description, isPrivate);

                if (result.Success)
                {
                    // 跳转到新创建的仓库页面
                    Response.Redirect($"~/src/views/Repo.aspx?id={result.RepositoryId}");
                }
                else
                {
                    ShowAlert(result.Message);
                }
            }
            catch (Exception ex)
            {
                ShowAlert("创建仓库时发生错误: " + ex.Message);
                System.Diagnostics.Debug.WriteLine("创建仓库错误: " + ex.Message);
            }
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        private int GetCurrentUserId()
        {
            // 从Session获取用户ID
            if (Session["UserId"] != null)
            {
                return Convert.ToInt32(Session["UserId"]);
            }

            // 从Cookie获取用户ID（如果有记住登录功能）
            if (Request.Cookies["UserId"] != null)
            {
                if (int.TryParse(Request.Cookies["UserId"].Value, out int cookieUserId))
                {
                    Session["UserId"] = cookieUserId;
                    return cookieUserId;
                }
            }

            // 开发环境默认用户ID
#if DEBUG
            Session["UserId"] = 1;
            return 1;
#else
                // 生产环境重定向到登录页
                Response.Redirect("~/Login.aspx");
                return 0;
#endif
        }

        /// <summary>
        /// 显示提示信息
        /// </summary>
        private void ShowAlert(string message)
        {
            string script = $"alert('{message.Replace("'", "\\'")}');";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", script, true);
        }

        protected override void OnUnload(EventArgs e)
        {
            _repositoryService?.Dispose();
            _userService?.Dispose();
            base.OnUnload(e);
        }
    }
}
