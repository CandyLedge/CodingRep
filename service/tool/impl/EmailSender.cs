using System;
using System.Net;
using System.Net.Mail;

namespace CodingRep.service.tool.impl
{
    public class EmailSender : IEmailSender
    {
        private readonly string emailAccount = Environment.GetEnvironmentVariable("NETEASE_EMAIL_ACCOUNT");
        private readonly string authCode = Environment.GetEnvironmentVariable("NETEASE_EMAIL_AUTH_CODE");
        private readonly SmtpClient sharedSmtpClient;

        public EmailSender()
        {
            sharedSmtpClient = new SmtpClient("smtp.yeah.net", 25)
            {
                Credentials = new NetworkCredential(emailAccount, authCode),
                EnableSsl = true
            };
        }

        public void sendEmail(string to, string subject, string body)
        {
            var mailMessage = new MailMessage(emailAccount, to, subject, body);
            sharedSmtpClient.Send(mailMessage);
        }

        public void sendHtmlEmail(string to, string subject, string htmlBody)
        {
            var mailMessage = new MailMessage(emailAccount, to, subject, htmlBody)
            {
                IsBodyHtml = true
            };

            sharedSmtpClient.Send(mailMessage);
        }
    }
}
