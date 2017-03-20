using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.ModelMapper
{
    public class NotificationConverter : EntityConverter<EntityFramework.Notification, Common.Entity.Notification>
    {
        private UserConverter _userConverter;
        public UserConverter UserConverter
        {
            get { return _userConverter ?? (_userConverter = new UserConverter()); }
        }

        public override Notification ConvertFromCore(EntityFramework.Notification sourceEntity)
        {
           return  new Notification
           {
               AddedBy = sourceEntity.AddedBy,
               AddedOn = sourceEntity.AddedOn,
               Description = sourceEntity.Description,
               Title = sourceEntity.NotificationTitle,
               TypeOfNotification = (Common.Enumeration.NotificationType)sourceEntity.NotificationType,
               Link = sourceEntity.Link,
               NotificationId = sourceEntity.NotificationId,
               UserDetails = UserConverter.ConvertFromCore(sourceEntity.User)
           };
        }

        public override EntityFramework.Notification ConvertToCore(Notification targetEntity)
        {
            return new EntityFramework.Notification
            {
                AddedBy =  targetEntity.AddedBy,
                AddedOn = targetEntity.AddedOn,
                Description = targetEntity.Description,
                NotificationTitle = targetEntity.Title,
                NotificationType = (int) targetEntity.TypeOfNotification,
                Link = targetEntity.Link,
                NotificationId = targetEntity.NotificationId
            };
        }
    }
}
