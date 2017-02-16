using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.RepoInterface;
using Course = TrainingTracker.DAL.EntityFramework.Course;

namespace TrainingTracker.DAL.Interface
{
    public interface ICourseRepository : IRepository<Course>
    {
        PagedResult<Course> GetCourses(int pageNumber, int pageSize);
        bool Update(Course updatedData);
    }
}