
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
    }
}
