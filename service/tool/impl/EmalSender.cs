namespace CodingRep.service.tool.impl
{
    public class EmalSender : IEmailSender
    {
        public void sendEmail(string to, string subject, string body)
        {
            // TODO: 实现纯文本邮件发送逻辑
            // 可使用SmtpClient或第三方邮件服务实现
        }

        public void sendHtmlEmail(string to, string subject, string htmlBody)
        {
            // TODO: 实现HTML格式邮件发送逻辑
            // 需要设置邮件的IsBodyHtml属性为true
        }
    }
}
