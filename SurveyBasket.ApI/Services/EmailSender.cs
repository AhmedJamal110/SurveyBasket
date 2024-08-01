using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using SurveyBasket.ApI.Settings;

namespace SurveyBasket.ApI.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSetting;

        public EmailSender(IOptions<EmailSettings> emailSetting)
        {
            _emailSetting = emailSetting.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var message = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_emailSetting.Mail),
                Subject = subject
            };

            message.To.Add(MailboxAddress.Parse(email));

            var builder = new BodyBuilder
            {
                HtmlBody = htmlMessage
            };
            message.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient ();

            smtp.Connect(_emailSetting.Host, _emailSetting.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSetting.Mail, _emailSetting.Password);
            await smtp.SendAsync(message);
            smtp.Disconnect(true);

          
        }
    }
}
