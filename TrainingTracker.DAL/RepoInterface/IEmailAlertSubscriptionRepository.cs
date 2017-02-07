using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.RepoInterface
{
    public interface IEmailAlertSubscriptionRepository : IRepository<EmailAlertSubscription>
    {
        void AddOrUpdate(EmailAlertSubscription subcription);
    }
}