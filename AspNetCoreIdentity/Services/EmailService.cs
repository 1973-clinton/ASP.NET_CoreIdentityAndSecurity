using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using AspNetCoreIdentity.Settings;
using Microsoft.Extensions.Options;

namespace AspNetCoreIdentity.Services
{
    public class EmailService : IEmailService
    {
        private readonly IOptions<SMTPSettings> _smtpSettings;

        public EmailService(IOptions<SMTPSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings;
        }

        public async Task Send(string from, string to, string subject, string body)
        {
            var message = new MailMessage(from, to, subject, body);

            using var emailClient = new SmtpClient(_smtpSettings.Value.Host, _smtpSettings.Value.Port)
            {
                Credentials = new NetworkCredential(_smtpSettings.Value.User, _smtpSettings.Value.Password)
            };
            await emailClient.SendMailAsync(message);
        }
    }
}
