using System;
using System.Security.Cryptography;
using System.Text;

namespace CodingRep.utils
{
    public class PasswordUtil
    {
        // 生成随机 salt
        private static string GenerateSalt(int size = 16)
        {
            var rng = new RNGCryptoServiceProvider();
            byte[] saltBytes = new byte[size];
            rng.GetBytes(saltBytes);
            return BitConverter.ToString(saltBytes).Replace("-", "").ToLower();
        }

        // 使用 salt + password 做 SHA256 哈希
        private static string HashPassword(string password, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                string combined = salt + password;
                byte[] bytes = Encoding.UTF8.GetBytes(combined);
                byte[] hash = sha256.ComputeHash(bytes);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }

        // 验证密码（输入密码 + 用户存储的 salt 进行对比）
        public static bool VerifyPassword(string inputPassword, string salt, string storedHash)
        {
            var hashed = HashPassword(inputPassword, salt);
            return hashed == storedHash;
        }
        
        public static (string Salt, string Hash) Hash(string password)
        {
            string salt = GenerateSalt();
            string hash = HashPassword(password, salt);
            return (salt, hash);
        }

    }
}