using System.Collections.Generic;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.RepoInterface
{
    public interface IEmailAlertSubscriptionRepository : IRepository<EmailAlertSubscription>
    {
        void AddOrUpdate(EmailAlertSubscription subcription);

        List<EmailAlertSubscription> GetAllSubscribedMentors(int traineeId);

    }
}