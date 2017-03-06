using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class CourseSubtopicRepository : Repository<CourseSubtopic>, ICourseSubtopicRepository
    {
        private readonly TrainingTrackerEntities _context;

        public CourseSubtopicRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }
    }
}