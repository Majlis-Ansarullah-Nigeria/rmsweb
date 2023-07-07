using FSH.WebApi.Shared.Notifications;

namespace rmsweb.Client.Infrastructure.Notifications;
public interface INotificationPublisher
{
    Task PublishAsync(INotificationMessage notification);
}