using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
   public class UserRepository : Repository<User>, IUserRepository
   {
       private readonly TrainingTrackerEntities _context;

       public UserRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }
   }
}
