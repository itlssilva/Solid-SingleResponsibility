# AI Agent Guidelines for Notifications API (Single Responsibility Principle)

## Architecture Overview
This project implements a multi-channel notification API demonstrating the Single Responsibility Principle. Each notification channel (email, SMS, push, webhook) is handled by a dedicated service class. An orchestrator service decides which channel to use based on the notification type, but never implements the sending logic itself.

Key components:
- **Notification Services**: Separate classes for each channel (e.g., `EmailNotificationService`, `SmsNotificationService`)
- **Orchestrator**: `NotificationOrchestrator` that selects and delegates to the appropriate service
- **API Endpoints**: Minimal API endpoints in `Program.cs` for sending notifications

See `README.md` for detailed principle explanation.

## Project Structure
- `src/Notifications.Api/`: Main API project
- Solution file: `SingleResponsibility.slnx` (Rider format)
- Configuration: Standard ASP.NET Core appsettings with logging

## Development Workflows
- **Build**: `dotnet build` from project root or `src/Notifications.Api/`
- **Run**: `dotnet run --project src/Notifications.Api/Notifications.Api.csproj`
- **Test API**: Use `Notifications.Api.http` file in Rider/Visual Studio for testing endpoints
- **Debug**: Standard .NET debugging; launch settings in `Properties/launchSettings.json`

## Coding Conventions
- **Language**: C# with nullable reference types enabled (`<Nullable>enable</Nullable>`)
- **Framework**: ASP.NET Core Minimal APIs (net10.0)
- **Dependency Injection**: Register services in `Program.cs` builder.Services
- **Records**: Use C# records for immutable data models (example: `WeatherForecast` record in `Program.cs`)
- **SRP Implementation**: Each service class has exactly one responsibility - handling one notification channel

## Implementation Patterns
- **Service Registration**: Add notification services to DI container:
  ```csharp
  builder.Services.AddScoped<IEmailNotificationService, EmailNotificationService>();
  builder.Services.AddScoped<INotificationOrchestrator, NotificationOrchestrator>();
  ```
- **Endpoint Mapping**: Use minimal API mapping in `Program.cs`:
  ```csharp
  app.MapPost("/notifications/send", async (NotificationRequest request, INotificationOrchestrator orchestrator) => {
      await orchestrator.SendNotificationAsync(request);
      return Results.Ok();
  });
  ```
- **Channel-Specific Logic**: Keep sending implementation isolated per service (e.g., email service only handles SMTP, not channel selection)

## Key Files
- `README.md`: Architecture description and SRP explanation
- `src/Notifications.Api/Program.cs`: Entry point and API configuration
- `src/Notifications.Api/Notifications.Api.csproj`: Dependencies (currently minimal)
- `src/Notifications.Api/Notifications.Api.http`: API testing requests

## External Dependencies
- `Microsoft.AspNetCore.OpenApi`: For OpenAPI/Swagger documentation
- Future: Add email/SMS libraries as needed (e.g., MailKit for SMTP, Twilio SDK for SMS)

Focus on maintaining single responsibility: if a service needs to handle multiple channels, refactor into separate services.
