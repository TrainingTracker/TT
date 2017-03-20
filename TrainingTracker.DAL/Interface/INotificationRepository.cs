
namespace TrainingTracker.DAL.Interface
{
    public interface INotificationRepository : IRepository<EntityFramework.Notification>
    {
        void UpdateAllNotificationForUserAsRead(int userId);
        void UpdateRelatedNotificationForUserAsRead(int userId, int notificationId);
    }
}
