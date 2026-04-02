using Microsoft.Extensions.DependencyInjection;
using Notifications.Application.Interface;

namespace Notifications.Application;

public static class Extensions
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<INotificationOrchestrator, NotificationOrchestrator>();
    }
}
