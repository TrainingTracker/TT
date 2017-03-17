using System.Collections.Generic;
using System.Linq;
using TrainingTracker.Common.ViewModel;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Interface
{
    public interface IUserRepository : IRepository<User>
    {
        List<EntityFramework.User> GetAllTrainees(int teamId, bool includeInActiveTrainee);
        DashboardVm GetAllTraineeDataForDashbaord(int teamId);
    }
}
