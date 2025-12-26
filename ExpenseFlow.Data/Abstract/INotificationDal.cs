using ExpenseFlow.Entity;

namespace ExpenseFlow.Data.Abstract
{
    public interface INotificationDal
    {
        void Add(Notification notification);
        List<Notification> GetByUserId(int userId);
        int GetUnreadCount(int userId);

        Notification GetById(int id);
        void Update(Notification notification);

        List<Notification> GetUnreadByUserId(int userId);



    }


}
