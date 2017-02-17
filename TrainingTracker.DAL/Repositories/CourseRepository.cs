using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;
using Course = TrainingTracker.DAL.EntityFramework.Course;

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


        //public void AddOrUpdate(Course course)
        //{
        //    if (course.Id == 0)
        //    {
        //        Add(course);
        //    }
        //    else
        //    {
        //        _context.Courses.Attach(course);
        //        _context.Entry(subcription).State = EntityState.Modified;
        //        _context.Entry(subcription).Property(x => x.SubscribedByUserId).IsModified = false;
        //        _context.Entry(subcription).Property(x => x.SubscribedForUserId).IsModified = false;
        //    }
        //}
    }
}