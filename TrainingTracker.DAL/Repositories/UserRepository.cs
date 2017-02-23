using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.RepoInterface;
using User = TrainingTracker.Common.Entity.User;

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

        public List<EntityFramework.User> GetAllTrainees(int teamId, bool includeInActiveTrainee)
        {
            return
                _context.Users.Where(x => x.IsTrainee == true && 
                                          (teamId == 0 || x.TeamId == teamId) &&
                                          (includeInActiveTrainee || x.IsActive == true))
                              .ToList();
        }
    }
}
