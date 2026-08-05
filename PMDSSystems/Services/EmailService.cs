
using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace PMDSSystems.Services
{
    public class EmailSettings
    {
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
    }

    public interface IEmailService
    {
        Task SendEmailAsync(
            string recipientEmail,
            string subject,
            string body);
    }

    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(
            IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }

        public async Task SendEmailAsync(
            string recipientEmail,
            string subject,
            string body)
        {
            using var message = new MailMessage();

            message.From = new MailAddress(
                _settings.SenderEmail,
                _settings.SenderName);

            message.To.Add(recipientEmail);

            message.Subject = subject;

            message.Body = body;

            message.IsBodyHtml = true;

            using var smtp = new SmtpClient(
                _settings.SmtpServer,
                _settings.Port);

            smtp.Credentials = new NetworkCredential(
                _settings.Username,
                _settings.Password);

            smtp.EnableSsl = _settings.EnableSsl;

            await smtp.SendMailAsync(message);
        }
    }
}

