using System.Linq;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    public interface ICourseRepository : IRepository<EntityFramework.Course>
    {
        PagedResult<EntityFramework.Course> GetCourses(int pageNumber, int pageSize);
    }
}