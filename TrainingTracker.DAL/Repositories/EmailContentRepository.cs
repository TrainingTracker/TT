using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class EmailContentRepository : Repository<EmailContent>, IEmailContentRepository
    {
        private readonly TrainingTrackerEntities _context;

        public EmailContentRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }
    }
}
