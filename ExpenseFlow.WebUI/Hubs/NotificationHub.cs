using ExpenseFlow.WebUI.Controllers;
using Microsoft.AspNetCore.SignalR;

namespace ExpenseFlow.WebUI.Hubs
{
    public class NotificationHub : Hub
    {
        public async Task RegisterManager()
        {
            // Kullanıcı gerçekten Manager mı 
            if (Context.User.IsInRole("Manager"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Managers");
            }
            else
            {
                throw new HubException("YYöneticiler grubuna katılma yetkiniz yok.");
                
            }

        }

    }
}
