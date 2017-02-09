using EmailAlertSubscription = TrainingTracker.Common.Entity.EmailAlertSubscription;

namespace TrainingTracker.DAL.ModelMapper
{
    public class EmailAlertSubscriptionConverter : EntityConverter<EntityFramework.EmailAlertSubscription, EmailAlertSubscription>
    {
        
        public override EmailAlertSubscription ConvertFromCore(EntityFramework.EmailAlertSubscription sourceEmailSubscription)
        {
            return new EmailAlertSubscription
            {
                Id = sourceEmailSubscription.Id,
                SubscribedByUserId = sourceEmailSubscription.SubscribedByUserId,
                SubscribedForUserId = sourceEmailSubscription.SubscribedForUserId,
                IsSubscribedForComment = sourceEmailSubscription.IsSubscribedForComment,
                IsSubscribedForWeeklyFeedback = sourceEmailSubscription.IsSubscribedForWeeklyFeedback,
                IsSubscribedForCodeReview = sourceEmailSubscription.IsSubscribedForCodeReview,
                IsSubscibedForAssignment = sourceEmailSubscription.IsSubscibedForAssignment,
                IsSubscibedForSkill = sourceEmailSubscription.IsSubscibedForSkill,
                IsDeleted = sourceEmailSubscription.IsDeleted
            };
        }

        public override EntityFramework.EmailAlertSubscription ConvertToCore(EmailAlertSubscription sourceEmailSubscription)
        {
            return new EntityFramework.EmailAlertSubscription
            {
                Id = sourceEmailSubscription.Id,
                SubscribedByUserId = sourceEmailSubscription.SubscribedByUserId,
                SubscribedForUserId = sourceEmailSubscription.SubscribedForUserId,
                IsSubscribedForComment = sourceEmailSubscription.IsSubscribedForComment,
                IsSubscribedForWeeklyFeedback = sourceEmailSubscription.IsSubscribedForWeeklyFeedback,
                IsSubscribedForCodeReview = sourceEmailSubscription.IsSubscribedForCodeReview,
                IsSubscibedForAssignment = sourceEmailSubscription.IsSubscibedForAssignment,
                IsSubscibedForSkill = sourceEmailSubscription.IsSubscibedForSkill,
                IsDeleted = sourceEmailSubscription.IsDeleted
            };
        }
    }
}