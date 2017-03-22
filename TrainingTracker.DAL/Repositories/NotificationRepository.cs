
using System.Linq;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class NotificationRepository : Repository<EntityFramework.Notification>, INotificationRepository
    {
        private readonly TrainingTrackerEntities _context;

        public NotificationRepository(TrainingTrackerEntities context) : base(context)
        {
            _context = context;
        }

        public void UpdateAllNotificationForUserAsRead(int userId)
        {
            _context.UserNotificationMappings.Where(x=>!x.Seen && x.UserId==userId)
                                             .ToList()
                                             .ForEach(x=>x.Seen = true);
        }

        public void UpdateRelatedNotificationForUserAsRead(int userId, int notificationId)
        {
            var coreCurrentNotification = _context.Notifications.FirstOrDefault(x => x.NotificationId == notificationId);
            _context.UserNotificationMappings.Where(x=>!x.Seen && x.UserId == userId && x.Notification.Link == coreCurrentNotification.Link)
                                             .ToList()
                                              .ForEach(x=>x.Seen = true);
        }
    }
}
