using System;
using System.Net;
using System.Net.Mail;

namespace CodingRep.service.tool.impl
{
    public class EmailSender : IEmailSender
    { 
        // TODO 改成环境变量
        private readonly string emailAccount = "codingrep@yeah.net";
        private readonly string authCode = "ETVF9ataPkPhGewu";
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
