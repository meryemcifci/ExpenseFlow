using ExpenseFlow.Business.Abstract;
using ExpenseFlow.Core.Hubs;
using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace ExpenseFlow.Business.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationDal _notificationDal;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            INotificationDal notificationDal,
            UserManager<AppUser> userManager,
            IHubContext<NotificationHub> hubContext)
        {
            _notificationDal = notificationDal;
            _userManager = userManager;
            _hubContext = hubContext;
        }
        // Çalışanın departman yöneticisine bildirim oluşturur
        public async Task CreateNotificationForDepartmentManagerAsync(int employeeUserId, string message, string redirectUrl)
        {
            var employee = await _userManager.FindByIdAsync(employeeUserId.ToString());
            if (employee == null || employee.DepartmentId == null)
                return;

            var departmentId = employee.DepartmentId;

            var managers = await _userManager.GetUsersInRoleAsync("Manager");

            var departmentManager = managers
                .FirstOrDefault(x => x.DepartmentId == departmentId);

            if (departmentManager == null)
                return;

            var notification = new Notification
            {
                UserId = departmentManager.Id,
                Message = message,
                IsRead = false,
                CreatedAt = DateTime.Now,
                RedirectUrl = redirectUrl,
            };

            _notificationDal.Add(notification);

            await _hubContext.Clients
                .User(departmentManager.Id.ToString())
                .SendAsync("ReceiveNotification", new
                {
                    id = notification.Id,
                    message = notification.Message,
                    redirectUrl = notification.RedirectUrl
                });
        }

        // Belirli bir role sahip tüm kullanıcılara bildirim oluşturur
        public async Task CreateNotificationForRoleAsync(string role, string message, string redirectUrl)
        {
            var users = await _userManager.GetUsersInRoleAsync(role);


            foreach (var user in users)   
            {
                var notification = new Notification
                {
                    UserId = user.Id,
                    Message = message,
                    IsRead = false,
                    CreatedAt = DateTime.Now,
                    RedirectUrl = redirectUrl,
                };

                _notificationDal.Add(notification);

                await _hubContext.Clients
                    .User(user.Id.ToString())
                    .SendAsync("ReceiveNotification", new
                    {
                        id = notification.Id,
                        message = notification.Message,
                        redirectUrl = notification.RedirectUrl
                    });
            }

        }
        // Belirli bir kullanıcı için bildirim oluşturur
        public async Task CreateNotificationForUserAsync(int userId, string message,string redirectUrl)
        {
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                RedirectUrl = redirectUrl,
                IsRead = false,
                CreatedAt = DateTime.Now
            };

            _notificationDal.Add(notification);

            await _hubContext.Clients
                .User(userId.ToString())
                .SendAsync("ReceiveNotification", new
                {
                    id = notification.Id,
                    message = notification.Message,
                    redirectUrl = notification.RedirectUrl
                });
        }

        // Kullanıcıya ait tüm bildirimleri getirir
        public List<Notification> GetByUserId(int userId)
        {
            return _notificationDal.GetByUserId(userId);
        }

        // Kullanıcıya ait okunmamış bildirimleri getirir
        public List<Notification> GetUnreadByUserId(int userId)
        {
            return _notificationDal.GetUnreadByUserId(userId);
        }
        // Kullanıcıya ait okunmamış bildirim sayısını getirir
        public int GetUnreadCount(int userId)
        {
            return _notificationDal.GetUnreadCount(userId);
        }
        // Bildirimi okunmuş olarak işaretler
        public void MarkAsRead(int notificationId)
        {
            var notification = _notificationDal.GetById(notificationId);

            if (notification == null || notification.IsRead)
                return;

            notification.IsRead = true;
            _notificationDal.Update(notification);
        }


    }


}
