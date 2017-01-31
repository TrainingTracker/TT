using System.Collections.Generic;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.RepoInterface
{
    public interface IUserRepository : IRepository<User>
    {
        void SubscribeTraineeNotification(List<SubscribedTrainee> listToUpdate, SubscribedTrainee dataToAdd);
        void UnsubscribeTraineeNotification(List<SubscribedTrainee> listToUpdate, int traineeIdToUnsubscribe);
        void UpdateSubscribedTrainee(List<SubscribedTrainee> efUpdatedList);
        List<SubscribedTrainee> GetSubscribedTrainees(int userId);
    }
}