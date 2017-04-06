using System.Collections.Generic;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;
using Course = TrainingTracker.Common.Entity.Course;

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

        public IEnumerable<CourseTrackerDetails> GetAllAssignedCoursesForTrainee(int traineeId)
        {
            return _context.CourseUserMappings.Where(x => x.UserId == traineeId)
                                              .Select(x => new CourseTrackerDetails
                                                           {
                                                               Name = x.Course.Name,
                                                               CourseStarted = x.StartedOn,
                                                               CourseCompleted = x.CompletedOn
                                                           });
        }
    }
}