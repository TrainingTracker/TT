using System.Data.Entity;
using System.Linq;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Repositories
{
    public class CourseRepository : Repository<Course>, ICourseRepository
    {
        private readonly TrainingTrackerEntities _context;

        public CourseRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public PagedResult<Course> GetCourses(int pageNumber, int pageSize)
        {
            return _context.Courses.OrderBy(x => x.Name).Page(pageNumber, pageSize);
        }
    }
}