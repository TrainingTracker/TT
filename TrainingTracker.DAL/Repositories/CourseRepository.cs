using System.Data.Entity;
using System.Linq;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.RepoInterface;

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


        public bool Update(Course updatedData)
        {
            var courseEntity = _context.Courses.SingleOrDefault(c => c.Id == updatedData.Id);
            if (courseEntity == null)
            {
                return false;
            }
            courseEntity.Name = updatedData.Name;
            courseEntity.Description = updatedData.Description;
            courseEntity.Icon = updatedData.Icon;
            courseEntity.Duration = updatedData.Duration;

            return true;
        }



    }
}