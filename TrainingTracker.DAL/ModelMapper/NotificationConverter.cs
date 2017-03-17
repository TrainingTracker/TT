using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.ModelMapper
{
    public class NotificationConverter : EntityConverter<EntityFramework.Notification, Common.Entity.Notification>
    {
        public override Notification ConvertFromCore(EntityFramework.Notification sourceEntity)
        {
            throw new System.NotImplementedException();
        }

        public override EntityFramework.Notification ConvertToCore(Notification targetEntity)
        {
            throw new System.NotImplementedException();
        }
    }
}
