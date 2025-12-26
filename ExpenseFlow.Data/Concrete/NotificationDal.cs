using ExpenseFlow.Data.Abstract;
using ExpenseFlow.Data.Context;
using ExpenseFlow.Entity;

namespace ExpenseFlow.Data.Concrete
{
    public class NotificationDal : INotificationDal
    {
        private readonly ExpenseFlowContext _context;
        
        public NotificationDal(ExpenseFlowContext context)
        {
            _context = context;
        }
        //yeni bildirim ekliyor
        public void Add(Notification notification)
        {
            _context.Notifications.Add(notification);
            _context.SaveChanges();
        }
        //id ye göre bildirimi getiriyor
        public Notification GetById(int id)
        {
            return _context.Notifications.FirstOrDefault(x => x.Id == id);
        }

        //okunan ve okunmayan her bildirimi getiriyor
        public List<Notification> GetByUserId(int userId)
        {
            return _context.Notifications
                .Where(x => x.UserId == userId)                 
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }
        //sadece okunmayan bildirimleri getiriyor
        public List<Notification> GetUnreadByUserId(int userId)
        {
            return _context.Notifications
                .Where(x => x.UserId == userId && !x.IsRead)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();
        }
        //okunmamış bildirim sayısını getiriyor
        public int GetUnreadCount(int userId)
        {
            return _context.Notifications
                .Count(x => x.UserId == userId && !x.IsRead);   
        }
        //bildirimi güncelliyor
        public void Update(Notification notification)
        {
            _context.Notifications.Update(notification);
            _context.SaveChanges();
        }
    }

}
