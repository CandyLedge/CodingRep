using System;
using System.Linq;
using CodingRep.App_Code;

namespace CodingRep.service.query
{
    /// <summary>
    /// 用户服务类
    /// </summary>
    public class UserService : IDisposable
    {
        private ModelDb _context;

        public UserService()
        {
            _context = new ModelDb();
        }

        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        public users GetUserById(int userId)
        {
            try
            {
                return _context.users.Find(userId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("获取用户失败: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据用户名获取用户
        /// </summary>
        public users GetUserByUsername(string username)
        {
            try
            {
                return _context.users.FirstOrDefault(u => u.userName == username);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("获取用户失败: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 检查用户是否存在
        /// </summary>
        public bool UserExists(int userId)
        {
            try
            {
                return _context.users.Any(u => u.id == userId);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("检查用户存在失败: " + ex.Message);
                return false;
            }
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}