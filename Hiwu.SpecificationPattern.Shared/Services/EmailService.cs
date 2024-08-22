using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MailKit.Net.Smtp;
using Hiwu.SpecificationPattern.Application.Interfaces.Services;
using Hiwu.SpecificationPattern.Domain.Settings;
using Hiwu.SpecificationPattern.Application.DataTransferObjects.Email;
using Hiwu.SpecificationPattern.Application.Exceptions;

namespace Hiwu.SpecificationPattern.Shared.Services
{
    /// <summary>
    /// Service for sending emails using SMTP protocol.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<MailSettings> mailSettings, ILogger<EmailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sends a single email based on the provided email request.
        /// </summary>
        /// <param name="request">EmailRequest object containing email details.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task SendAsync(EmailRequest request)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(request.From ?? _mailSettings.EmailFrom);
                email.To.Add(MailboxAddress.Parse(request.To));
                email.Subject = request.Subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = request.Body
                };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApiException(ex.Message);
            }
        }

        /// <summary>
        /// Sends an email to multiple recipients based on the provided email request.
        /// </summary>
        /// <param name="request">EmailRequest object containing email details.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task SendMultipleAsync(EmailRequest request)
        {
            try
            {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(request.From ?? _mailSettings.EmailFrom);

                // Split the To field into multiple recipients if it's a comma-separated string
                var recipients = request.To.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var recipient in recipients)
                {
                    email.To.Add(MailboxAddress.Parse(recipient.Trim()));
                }

                email.Subject = request.Subject;
                var builder = new BodyBuilder
                {
                    HtmlBody = request.Body
                };
                email.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.SmtpHost, _mailSettings.SmtpPort, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SmtpUser, _mailSettings.SmtpPass);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ApiException(ex.Message);
            }
        }
    }

}
