using System.Linq;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Repositories
{
    public class SubtopicContentRepository : Repository<SubtopicContent>, ISubtopicContentRepository
    {
        private readonly TrainingTrackerEntities _context;

        public SubtopicContentRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

    }
}