using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.RepoInterface;

namespace TrainingTracker.DAL.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly TrainingTrackerEntities _context;

        public UserRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public void SubscribeTraineeNotification(List<SubscribedTrainee> listToUpdate, SubscribedTrainee dataToAdd)
        {
            listToUpdate.Add(dataToAdd);
        }

        public void UnsubscribeTraineeNotification(List<SubscribedTrainee> listToUpdate, int traineeIdToUnsubscribe)
        {
            listToUpdate.Single(x => x.SubscribedForUserId == traineeIdToUnsubscribe).IsDeleted = true;
        }

        public void UpdateSubscribedTrainee(List<SubscribedTrainee> updatedList)
        {
            foreach (var item in updatedList)
            {
                if (item.Id == 0)
                {
                    _context.Entry(item).State = EntityState.Added;
                }
                else if(item.IsDeleted)
                {
                    _context.SubscribedTrainees.Attach(item);
                    _context.Entry(item).Property(x => x.IsDeleted).IsModified = true;
                }     
            }
        }

        public List<SubscribedTrainee> GetSubscribedTrainees(int userId)
        {
            return _context.SubscribedTrainees.Where(t => t.SubscribedByUserId == userId && !t.IsDeleted).ToList();
        }

    }
}