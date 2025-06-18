using System;
using System.Net;
using System.Net.Mail;


namespace CodingRep.service.tool.impl
{
    public class EmailSender : IEmailSender
    {
        private readonly String emailAccount = Environment.GetEnvironmentVariable("NETEASE_EMAIL_ACCOUNT");
        private readonly String authCode     = Environment.GetEnvironmentVariable("NETEASE_EMAIL_AUTH_CODE");
        public void sendEmail(string to, string subject, string body)
        {
            var client = new SmtpClient("smtp.yeah.net", 25)
            {
                Credentials = new NetworkCredential(emailAccount, authCode),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(emailAccount, to, subject, body);
            client.Send(mailMessage);
        }

        public void sendHtmlEmail(string to, string subject, string htmlBody)
        {
            var client = new SmtpClient("smtp.yeah.net", 25)
            {
                Credentials = new NetworkCredential(emailAccount, authCode),
                EnableSsl = true
            };

            var mailMessage = new MailMessage(emailAccount, to, subject, htmlBody)
            {
                IsBodyHtml = true
            };

            client.Send(mailMessage);
        }
    }
}
