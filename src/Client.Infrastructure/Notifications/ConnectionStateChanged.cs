using FSH.WebApi.Shared.Notifications;

namespace rmsweb.Client.Infrastructure.Notifications;
public record ConnectionStateChanged(ConnectionState State, string? Message) : INotificationMessage;