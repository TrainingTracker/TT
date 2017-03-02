using TrainingTracker.Common.Entity;
using Course = TrainingTracker.DAL.EntityFramework.Course;

namespace TrainingTracker.DAL.Interface
{
    public interface ICourseRepository : IRepository<Course>
    {
        PagedResult<Course> GetCourses(int pageNumber, int pageSize);
    }
}