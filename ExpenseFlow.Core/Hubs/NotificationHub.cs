
using Microsoft.AspNetCore.SignalR;

namespace ExpenseFlow.Core.Hubs
{
    public class NotificationHub : Hub
    {

        public override async Task OnConnectedAsync()
        {

            var user = Context.User;

            if (user.IsInRole("Manager"))
                await Groups.AddToGroupAsync(Context.ConnectionId, "Managers");

            if (user.IsInRole("Accountant"))
                await Groups.AddToGroupAsync(Context.ConnectionId, "Accountants");

            if (user.IsInRole("Admin"))
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admins");

            if (user.IsInRole("Employee"))
                await Groups.AddToGroupAsync(Context.ConnectionId, "Employees");

            await base.OnConnectedAsync();


            var userId = Context.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            }

            await base.OnConnectedAsync();


        }
    }

}
