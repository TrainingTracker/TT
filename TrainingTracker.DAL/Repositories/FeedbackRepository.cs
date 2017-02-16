using System.Linq;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class FeedbackRepository : Repository<Feedback>, IFeedbackRepository
    {
        private readonly TrainingTrackerEntities _context;

        public FeedbackRepository(TrainingTrackerEntities context) : base(context)
        {
            _context = context;
        }

        public Feedback LoadFeedbackAndThreads(int feedbackId)
        {
            return SingleOrDefault(x=>x.FeedbackId == feedbackId);
        }
    }
}
