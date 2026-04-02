using Microsoft.Extensions.Logging;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;

namespace Notifications.Infrastructure.Senders;

public sealed class SmsSender(ILogger<SmsSender> logger) : INotificationSender
{
    public async Task<bool> SendAsync(NotificationRequest request, CancellationToken ct = default)
    {
        // TODO: Aqui chamaria o SDK da Twilio
        logger.LogInformation("SMS notification sent successfully to {Recipient}", request.Recipient);

        await Task.Delay(50, ct);
        return true;
    }

    public bool SupportsChannel(NotificationChannel channel) =>
        channel == NotificationChannel.Sms;
}
