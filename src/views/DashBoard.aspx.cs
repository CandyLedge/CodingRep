using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CodingRep.App_Code;
using CodingRep.service.query;
using CodingRep.utils;

namespace CodingRep.src.views
{
    public partial class DashBoard : System.Web.UI.Page
    {
        private readonly UserRepositoryService urs = UserRepositoryService.Create();
        private int                   uid;
        private List<repositories>    allRepos;
        protected void Page_Load(object sender, EventArgs e)
        {
            uid = Convert.ToInt16(Session["userId"].ToString());
            allRepos = urs.GetAllReposByUID(uid);
            foreach (var repo in allRepos)
            {
                var listItem = new HtmlGenericControl("li");
                var hyperLink = new HyperLink
                {
                    NavigateUrl = $"~/src/views/Repo.aspx?id={repo.id}",
                    Text = repo.name
                };
                listItem.Controls.Add(hyperLink);
                listItem.Attributes.Add("class", "repo-list-item");
                repoList.Controls.Add(listItem);
            }
            
            var commits = urs.GetAllCommitsByUserId(uid);
            foreach (var commit in commits)
            {
                var commitDiv = new HtmlGenericControl("div");
                commitDiv.Attributes.Add("class", "commit-record");

                var recordItem = new HtmlGenericControl("div");
                recordItem.Attributes.Add("class", "commit-record-item");

                var repoName = new Literal { Text = $"<span class=\"commit-repo\">仓库:</span> {commit.RepositoryName}<br/>" };
                var branchName = new Literal { Text = $"<span class=\"commit-branch\">分支:</span> {commit.BranchName}<br/>" };
                var timestamp = new Literal { Text = $"<span class=\"commit-time\">时间:</span> {commit.Commit.timestamp}<br/>" };
                var message = new Literal { Text = $"<span class=\"commit-msg\">信息:</span> {commit.Commit.message}<br/>" };

                recordItem.Controls.Add(repoName);
                recordItem.Controls.Add(branchName);
                recordItem.Controls.Add(timestamp);
                recordItem.Controls.Add(message);

                commitDiv.Controls.Add(recordItem);

                commitRecords.Controls.Add(commitDiv);
            }
            
        }

        protected void btnNewRepo_Click(object sender, EventArgs e)
        {
            Response.Redirect("../views/CreateRepo.aspx");
        }
    }
}