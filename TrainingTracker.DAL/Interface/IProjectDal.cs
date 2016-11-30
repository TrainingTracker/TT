using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    /// <summary>
    /// Interface for Project Dal
    /// </summary>
    public interface IProjectDal
    {
        /// <summary>
        /// Inteface method to fetch projects by user
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>instance of list of project</returns>
        List<Project> GetProjectsByUserId(int userId);
    }
}
