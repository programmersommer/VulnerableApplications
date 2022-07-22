using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;


namespace TimingAttack.Services
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_configuration["EmailSettings:SenderName"], _configuration["EmailSettings:SenderEmail"]));
            mimeMessage.To.Add(MailboxAddress.Parse(email));

            mimeMessage.Subject = subject;

            var builder = new BodyBuilder
            {
                TextBody = message,

                HtmlBody = message
            };

            mimeMessage.Body = builder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_configuration["SmtpSettings:Server"],
                    Convert.ToInt16(_configuration["SmtpSettings:Port"]),
                    SecureSocketOptions.StartTls);

                client.Authenticate(_configuration["SmtpSettings:Sender"], _configuration["SmtpSettings:Password"]);

                client.Send(mimeMessage);
                client.Disconnect(true);
            }
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
