using Microsoft.Extensions.Logging;
using Notifications.Domain.Models;

namespace Notifications.Infrastructure.Logging;

public sealed class NotificationLogger(ILogger<NotificationLogger> logger)
{
    public void LogResult(NotificationRequest request, bool success)
    {
        if (success)
            logger.LogInformation(
                "Notificação enviada | Canal={Channel} | Destino={Recipient}",
                request.Channel, request.Recipient);
        else
            logger.LogWarning(
                "Falha ao enviar | Canal={Channel} | Destino={Recipient}",
                request.Channel, request.Recipient);
    }
}
