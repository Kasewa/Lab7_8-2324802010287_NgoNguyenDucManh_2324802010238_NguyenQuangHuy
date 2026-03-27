using ASC.Web.Configuration;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ASC.Web.Services
{
    public class AuthMessageSender : IEmailSender, ISmsSender
    {
        private readonly ApplicationSettings _settings;
        private readonly ILogger<AuthMessageSender> _logger;

        public AuthMessageSender(IOptions<ApplicationSettings> options, ILogger<AuthMessageSender> logger)
        {
            _settings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("ASC", _settings.UTAccount));
                emailMessage.To.Add(new MailboxAddress("", email));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = message };

                using var client = new SmtpClient();
                await client.ConnectAsync("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_settings.UTAccount, _settings.UTPercent);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);

                _logger.LogInformation("Email sent to {Email} with subject: {Subject}", email, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Email}. Subject: {Subject}", email, subject);
                throw;
            }
        }

        public Task SendSmsAsync(string number, string message)
        {
            return Task.CompletedTask;
        }
    }
}
