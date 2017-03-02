using TrainingTracker.Common.Entity;
using CourseSubtopic = TrainingTracker.DAL.EntityFramework.CourseSubtopic;

namespace TrainingTracker.DAL.Interface
{
    public interface ICourseSubtopicRepository : IRepository<CourseSubtopic>
    {
    }
}