using System.Collections.Generic;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Interface
{
    public interface IEmailAlertSubscriptionRepository : IRepository<EmailAlertSubscription>
    {
        void AddOrUpdate(EmailAlertSubscription subcription);

        List<EmailAlertSubscription> GetAllSubscribedMentors(int traineeId);
        List<EmailAlertSubscription> GetAllSubscribedMentors(int traineeId, bool includeDeleted);

        int GetId(int subscribedByUserId, int subscribedForUserId);
    }
}