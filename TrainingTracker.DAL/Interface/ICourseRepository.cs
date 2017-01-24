using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Repositories;

namespace TrainingTracker.DAL.Interface
{
    public interface ICourseRepository : IRepository<Course>
    {
        PagedResult<Course> GetCourses(int pageNumber, int pageSize);
        public bool Update(Course updatedData);
    }
}