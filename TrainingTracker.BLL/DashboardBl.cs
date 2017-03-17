using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.ViewModel;
using TrainingTracker.DAL;
using TrainingTracker.DAL.Repositories;

namespace TrainingTracker.BLL
{
    /// <summary>
    /// Bussiness Layer class for Dashboard, Inherits Bussiness Base Class
    /// </summary>
    public class DashboardBl:BusinessBase
    {
        /// <summary>
        /// Fetch Dashboard data by User
        /// </summary>
        /// <param name="user">Instance of logged in Team</param>
        /// <returns></returns>
       
        public DashboardVm GetDashboardData(User user)
        {
            return UnitOfWork.UserRepository.GetAllTraineeDataForDashbaord(user.TeamId ?? 0);
        }
    }
}