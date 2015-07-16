using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace HomeWork3Common
{
    public class EmailModel
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
    public class EmailService : Services.IEmailService
    {
        System.Collections.Specialized.NameValueCollection Config;

        public EmailService(System.Collections.Specialized.NameValueCollection config)
        {
            Config = config;
        }
        public void SendEmail(EmailModel email)
        {
            if (Config == null)
                return;
            System.Net.Mail.MailMessage mail = new MailMessage();
            mail.To.Add(email.To);

            mail.From = new MailAddress( email.From);
            mail.Subject = email.Subject;
            mail.Body = email.Body;
            mail.IsBodyHtml = true;
            
            SmtpClient smtp = new SmtpClient
            {
                Host = Config.Get("Client"),
                Port = int.Parse(Config.Get("SMTPPort")),
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential
                (Config.Get("EmailName"), Config.Get("EmailPassword")),
                EnableSsl = bool.Parse(Config.Get("EnableSSL"))
            };

            smtp.Send(mail);
        }
    }
}
