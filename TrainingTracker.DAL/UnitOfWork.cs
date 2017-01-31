using System.Runtime.Remoting.Contexts;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.RepoInterface;
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

        private ICourseRepository _courseRepository;
        public ICourseRepository CourseRepository
        {
            get { return _courseRepository ?? (_courseRepository = new CourseRepository(_context)); }
        }

        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get { return _userRepository ?? (_userRepository = new UserRepository(_context)); }
        }

        public int Commit()
        {
           return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}