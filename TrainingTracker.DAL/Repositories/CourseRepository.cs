using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Repositories
{
    public class CourseRepository : Repository<EntityFramework.Course>, ICourseRepository
    {
        private readonly TrainingTrackerEntities _context;

        public CourseRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }


        public PagedResult<EntityFramework.Course> GetCourses(int pageNumber, int pageSize)
        {
            return _context.Courses.OrderBy(x => x.Name).Page(pageNumber, pageSize);
        }
    }
}