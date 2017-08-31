using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class EmailAlertSubscriptionRepository : Repository<EmailAlertSubscription>, IEmailAlertSubscriptionRepository
    {
        private readonly TrainingTrackerEntities _context;

        public EmailAlertSubscriptionRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }


        public void AddOrUpdate(EmailAlertSubscription subcription)
        {
            if (subcription.Id == 0)
            {
                Add(subcription);
            }
            else
            {
                _context.EmailAlertSubscriptions.Attach(subcription);
                _context.Entry(subcription).State = EntityState.Modified;
                _context.Entry(subcription).Property(x => x.SubscribedByUserId).IsModified = false;
                _context.Entry(subcription).Property(x => x.SubscribedForUserId).IsModified = false;
            }
        }


        public List<EmailAlertSubscription> GetAllSubscribedMentors(int traineeId)
        {
            return GetAllSubscribedMentors(traineeId, false);
        }

        public List<EmailAlertSubscription> GetAllSubscribedMentors(int traineeId, bool includeDeleted)
        {
            return _context.EmailAlertSubscriptions
                           .Include(x => x.User)
                           .Where(x => x.SubscribedForUserId == traineeId
                                       && (includeDeleted || !x.IsDeleted))
                           .ToList();
        }


        public int GetId(int subscribedByUserId, int subscribedForUserId)
        {
            var entity = Find(x => x.SubscribedByUserId == subscribedByUserId
                                   && x.SubscribedForUserId == subscribedForUserId)
                .FirstOrDefault();

            if (entity == null) return 0;

            _context.Entry(entity).State = EntityState.Detached;
            return entity.Id;
        }
    }
}