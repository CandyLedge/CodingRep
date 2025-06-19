using System;
using System.Collections.Generic;
using System.Linq;
using CodingRep.App_Code;

namespace CodingRep.service.query
{
    public class UserRepositoryService: IDisposable
    {
        private readonly ModelDb _context;

        private UserRepositoryService(ModelDb context)
        {
            _context = context;
        }

        // 工厂方法，调用时不用写 new ModelDb()
        public static UserRepositoryService Create()
        {
            return new UserRepositoryService(new ModelDb());
        }

        public KeyValuePair<int, int> GetRepCntByUID(int userID)
        {
            int repCnt = _context.repositories.Count(r => r.userId == userID);
            return new KeyValuePair<int, int>(userID, repCnt);
        }

        public List<repositories> GetAllReposByUID(int userID)
        {
            return _context.repositories.Where(r => r.userId == userID).ToList();
        }
        
        public List<CommitWithBranchInfo> GetAllCommitsByUserId(int userId)
        {
            try
            {
                var userRepos = _context.repositories
                    .Where(r => r.userId == userId)
                    .ToList();

                var allCommits = new List<CommitWithBranchInfo>();

                foreach (var repo in userRepos)
                {
                    var repoService = new RepositoryService(_context);
                    var commits = repoService.GetAllBranchesCommits(repo.id);
                    
                    foreach (var commitInfo in commits)
                    {
                        allCommits.Add(new CommitWithBranchInfo
                        {
                            Commit = commitInfo.Commit,
                            BranchName = commitInfo.BranchName,
                            RepositoryName = repo.name
                        });
                    }
                }

                // 按时间排序，最新的在前
                return allCommits.OrderByDescending(c => c.Commit.timestamp).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception("获取用户所有提交记录失败: " + ex.Message, ex);
            }
        }

        #region 辅助类
        /// <summary>
        /// 带分支信息和仓库名称的提交记录
        /// </summary>
        public class CommitWithBranchInfo : RepositoryService.CommitWithBranchInfo
        {
            public string RepositoryName { get; set; }
        }
        #endregion

        public bool HasPrivateRepo(int userID)
        {
            return _context.repositories.Any(r => r.userId == userID && r.isPrivate);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}