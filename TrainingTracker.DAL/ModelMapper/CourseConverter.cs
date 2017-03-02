
using CourseModel = TrainingTracker.Common.Entity.Course;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class CourseConverter : EntityConverter<Course, CourseModel>
    {

        public override CourseModel ConvertFromCore(Course sourceCourse)
        {
            return new CourseModel
            {
                Id = sourceCourse.Id,
                Name = sourceCourse.Name,
                Description = sourceCourse.Description,
                Icon = sourceCourse.Icon,
                AddedBy = sourceCourse.AddedBy,
                IsActive = sourceCourse.IsActive,
                IsPublished = sourceCourse.IsPublished,
                Duration = sourceCourse.Duration,
                CreatedOn = sourceCourse.CreatedOn,
            };
        }

        public override Course ConvertToCore(CourseModel sourceCourse)
        {
            return new Course
            {
                Id = sourceCourse.Id,
                Name = sourceCourse.Name,
                Description = sourceCourse.Description,
                Icon = sourceCourse.Icon,
                AddedBy = sourceCourse.AddedBy,
                IsActive = sourceCourse.IsActive,
                IsPublished = sourceCourse.IsPublished,
                Duration = sourceCourse.Duration,
                CreatedOn = sourceCourse.CreatedOn,
            };
        }
    }
}
