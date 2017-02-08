using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class EmailRepository : Repository<EmailContent>, IEmailRepository
    {
        private readonly TrainingTrackerEntities _context;

        public EmailRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }
    }
}
