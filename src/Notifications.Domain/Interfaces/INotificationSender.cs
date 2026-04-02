using Notifications.Domain.Models;

namespace Notifications.Domain.Interfaces;

public interface INotificationSender
{
    Task<bool> SendAsync(NotificationRequest request, CancellationToken ct = default);
    bool SupportsChannel(NotificationChannel channel);
}
