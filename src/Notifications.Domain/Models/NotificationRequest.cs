namespace Notifications.Domain.Models;

public record NotificationRequest(
    string Recipient,   // e-mail, telefone ou device token
    string Subject,
    string Body,
    NotificationChannel Channel
);

public enum NotificationChannel { Email, Sms, Push }
