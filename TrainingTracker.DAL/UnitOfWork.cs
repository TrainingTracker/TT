using System.Runtime.Remoting.Contexts;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.Repositories;

namespace TrainingTracker.DAL
{
    public class UnitOfWork
    {
        private readonly TrainingTrackerEntities _context;

        public UnitOfWork()
        {
            _context = new TrainingTrackerEntities();
        }

        private IRepository<Course> _courseRepositoryWithoutRepo;
        public IRepository<Course> CourseRepositoryWithoutRepo
        {
            get { return _courseRepositoryWithoutRepo ?? (_courseRepositoryWithoutRepo = new Repository<Course>(_context)); }
        }

        private ICourseRepository _courseRepository;
        public ICourseRepository CourseRepository
        {
            get { return _courseRepository ?? (_courseRepository = new CourseRepository(_context)); }
        }

    }
}