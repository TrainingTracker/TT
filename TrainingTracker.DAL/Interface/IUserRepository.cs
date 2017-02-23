using System.Collections.Generic;

namespace TrainingTracker.DAL.Interface
{
    public interface IUserRepository
    {
        List<EntityFramework.User> GetAllTrainees(int teamId, bool includeInActiveTrainee);
    }
}
