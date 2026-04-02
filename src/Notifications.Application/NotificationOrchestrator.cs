using Notifications.Application.Interface;
using Notifications.Domain.Interfaces;
using Notifications.Domain.Models;
using Notifications.Infrastructure.Logging;

namespace Notifications.Application;

internal sealed class NotificationOrchestrator(IEnumerable<INotificationSender> senders,
    NotificationLogger notificationLogger) : INotificationOrchestrator
{
    public async Task<bool> NotifyAsync(NotificationRequest request, CancellationToken ct = default)
    {
        var sender = senders.FirstOrDefault(s => s.SupportsChannel(request.Channel))
            ?? throw new InvalidOperationException($"Canal {request.Channel} não suportado.");

        var success = await sender.SendAsync(request, ct);

        notificationLogger.LogResult(request, success);

        return success;
    }
}
