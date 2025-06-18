namespace CodingRep.service.tool
{
    public interface IEmailSender
    {
        /// <summary>
        /// 发送纯文本邮件（可用于简单通知）
        /// </summary>
        void sendEmail(string to, string subject, string body);

        /// <summary>
        /// 发送HTML格式邮件（可用于复杂排版，如验证码、找回密码页面链接等）
        /// </summary>
        void sendHtmlEmail(string to, string subject, string htmlBody);
    }
}
