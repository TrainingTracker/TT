
using System.Collections.Generic;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Interface
{
    interface IUserRepository : IRepository<User>
    {
        List<EntityFramework.User> GetAllTrainees(int teamId, bool includeInActiveTrainee);
    }
}
