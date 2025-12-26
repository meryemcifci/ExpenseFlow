using Microsoft.AspNetCore.SignalR;
using ExpenseFlow.WebUI.Hubs;
using ExpenseFlow.Business.Abstract;

namespace ExpenseFlow.WebUI.Services
{
    public class SignalRNotificationPublisher : INotificationPublisher
    {
        private readonly IHubContext<NotificationHub> _hubContext;

        public SignalRNotificationPublisher(IHubContext<NotificationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task PublishToUserAsync(int userId, object payload)
        {
            await _hubContext.Clients
                .User(userId.ToString())
                .SendAsync("ReceiveNotification", payload);
        }

        public async Task PublishToRoleAsync(string role, object payload)
        {
            await _hubContext.Clients
                .Group(role + "s")
                .SendAsync("ReceiveNotification", payload);
        }
    }
}
