using System;
using System.Linq;
using CodingRep.service.tool;
using CodingRep.service.tool.impl;

namespace CodingRep.utils
{
    public static class EmailVerificationService
    {   
        private static readonly IEmailSender toolEmailSender = new EmailSender();

        // 生成验证码 lujun
        private static string generateVerificationCode(int length = 6)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        // 发送纯文本验证码邮件
        public static string sendVerificationCode(string to)
        {
            string code = generateVerificationCode();
            string body = $"您的验证码是：{code}";
            toolEmailSender.sendEmail(to, "验证码邮件", body);
            return code;
        }

        // 发送HTML格式验证码邮件
        public static string sendHtmlVerificationCode(string to)
        {
            string code = generateVerificationCode();
            string htmlBody = $"<p>您好！</p><p>您的验证码是：<strong>{code}</strong></p>";
            toolEmailSender.sendHtmlEmail(to, "验证码邮件", htmlBody);
            return code;
        }

        // 示例：预留密码重置邮件功能
        public static void sendPasswordResetEmail(string to, string resetLink)
        {
            string subject = "密码重置请求";
            string htmlBody = $"<p>请点击以下链接重置密码：</p><a href=\"{resetLink}\">{resetLink}</a>";
            toolEmailSender.sendHtmlEmail(to, subject, htmlBody);
        }

        // 示例：预留通用通知邮件
        public static void sendNotificationEmail(string to, string subject, string message)
        {
            toolEmailSender.sendEmail(to, subject, message);
        }
    }
}
