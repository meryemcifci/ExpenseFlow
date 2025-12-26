public interface INotificationPublisher
{
    Task PublishToUserAsync(int userId, object payload);
    Task PublishToRoleAsync(string role, object payload);
}
