using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;
using Course = TrainingTracker.DAL.EntityFramework.Course;

namespace TrainingTracker.DAL.DataAccess
{
    public class CourseDal
    {
        public PagedResult<Course> GetCourses(int pageNumber, int pageSize)
        {
            using (var context = new TrainingTrackerEntities())
            {
                return context.Courses.OrderBy(x => x.Name).Page(pageNumber, pageSize);
            }
        }
    }
}
