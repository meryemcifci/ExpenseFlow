using ExpenseFlow.Entity;

namespace ExpenseFlow.Business.Abstract
{
    public interface INotificationService
    {
        Task CreateNotificationForRoleAsync(string role, string message, string redirectUrl);
        int GetUnreadCount(int userId);

        List<Notification> GetByUserId(int userId);


        Task CreateNotificationForUserAsync(int userId, string message,string redirectUrl);

        Task CreateNotificationForDepartmentManagerAsync(
            int employeeUserId,
            string message,
            string redirectUrl

        );


        void MarkAsRead(int notificationId);
        List<Notification> GetUnreadByUserId(int userId);



    }


}
