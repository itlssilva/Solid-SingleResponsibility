using Notifications.Domain.Models;

namespace Notifications.Application.Interface;

public interface INotificationOrchestrator
{
    Task<bool> NotifyAsync(NotificationRequest request, CancellationToken ct = default);
}
