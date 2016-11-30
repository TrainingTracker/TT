using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    /// <summary>
    /// Interface for Session Dal
    /// </summary>
   public interface ISessionDal
    {
        /// <summary>
        /// Inteface method to fetch sessions by User id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List Of session</returns>
        List<Session> GetSessionsByUserId(int userId);

        /// <summary>
        /// Inteface method to fetch sessions by User id
        /// </summary>
        /// <param name="objSession">User Id</param>
        /// <returns>List Of session</returns>
       int AddEditSessions( Session objSession );

       /// <summary>
       /// Interface method to fetch Sessions on filters
       /// </summary>
       /// <param name="pageSize">record count to be returned</param>
       /// <param name="sessionType">session type</param>
       /// <param name="searchKeyword">search keyword</param>
       /// <param name="teamId">teamId</param>
       /// <returns>List of session </returns>
       List<Session> GetSessionOnFilter(int pageSize ,int sessionType ,string searchKeyword,int teamId); 
    }
}
