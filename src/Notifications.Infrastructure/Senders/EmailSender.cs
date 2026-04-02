using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;
using Notifications.Infrastructure.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Notifications.Infrastructure.Senders;

public sealed class EmailSender(
    IOptions<SendGridOptions> sendGridOptions,
    ILogger<EmailSender> logger) : INotificationSender
{
    private readonly SendGridOptions _options = sendGridOptions.Value;

    public async Task<bool> SendAsync(NotificationRequest request, CancellationToken ct = default)
    {
        var client = new SendGridClient(_options.ApiKey);
        var message = MailHelper.CreateSingleEmail(
            new EmailAddress(_options.FromEmail, _options.FromName),
            new EmailAddress(request.Recipient),
            request.Subject,
            request.Body,
            request.Body);

        try
        {
            var response = await client.SendEmailAsync(message, ct);

            if (response.IsSuccessStatusCode)
            {
                logger.LogInformation(
                    "Email notification sent successfully to {Recipient} via SendGrid",
                    request.Recipient);
                return true;
            }

            logger.LogWarning(
                "SendGrid rejected email to {Recipient} with status code {StatusCode}",
                request.Recipient,
                (int)response.StatusCode);

            return false;
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unexpected error while sending email to {Recipient} via SendGrid",
                request.Recipient);

            return false;
        }
    }

    public bool SupportsChannel(NotificationChannel channel) =>
        channel == NotificationChannel.Email;
}
