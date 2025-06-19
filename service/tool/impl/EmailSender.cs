using System;
using System.Net;
using System.Net.Mail;
using CodingRep.service.stores;

namespace CodingRep.service.tool.impl
{
    public class EmailSender : IEmailSender
    { 
        // TODO 改成环境变量
        private readonly string emailAccount;
        private readonly string authCode;
        private readonly SmtpClient sharedSmtpClient;

        public EmailSender()
        {
            var envStore =new EnvironmentStore();
            emailAccount = Environment.GetEnvironmentVariable("NETEASE_EMAIL_ACCOUNT");
            authCode = Environment.GetEnvironmentVariable("NETEASE_EMAIL_AUTH_CODE");
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
