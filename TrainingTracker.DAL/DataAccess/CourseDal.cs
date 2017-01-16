using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.DataAccess
{
    public class CourseDal
    {
        public PagedResult<Course> GetCourses(int pageNumber, int pageSize)
        {
            using (var context = new TrainingTrackerEntities())
            {
                return context.Courses.Page(pageNumber, pageSize);
            }
        }
    }
}
