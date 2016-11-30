using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Enumeration;
using TrainingTracker.Common.ViewModel;

namespace TrainingTracker.DAL.Interface
{
    /// <summary>
    /// Interface for User Dal
    /// </summary>
    public interface IUserDal
    {
        /// <summary>
        /// Calls stored procedure which validates user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <returns>True if valid customer.</returns>
        bool ValidateUser(User userData);

        /// <summary>
        /// Calls stored procedure which adds user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <param name="userId">Out parameter created UserId.</param>
        /// <returns>True if added.</returns>
        bool AddUser(User userData, out int userId);

        /// <summary>
        /// Calls stored procedure which updates user.
        /// </summary>
        /// <param name="objUser">User data object.</param>
        /// <returns>True if updated.</returns>
        bool UpdateUser(User objUser);

        /// <summary>
        /// Gets all Users.
        /// </summary>
        /// <returns>List of all users.</returns>
        List<User> GetAllUsers();

        /// <summary>
        /// Interface method to Fetch user By its name
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        User GetUserByUserName(string userName);

        /// <summary>
        /// Fetch User by its Id
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        User GetUserById(int userId);

        /// <summary>
        /// Get All Dashboard Data
        /// </summary>
        /// <param name="teamId">Logged in user Team Id</param>
        /// <returns>instance Of List of UserData</returns>
        List<UserData> GetDashboardData(int teamId);

        /// <summary>
        /// Get list of integer value as userId.
        /// </summary>
        /// <param name="notification">Takes parametere as typeOfNotification and the user for which notification is added.</param>
        /// <returns> Returns list of userid.</returns>
        List<int> GetUserId(Notification notification, int addedFor);

        /// <summary>
        /// Gets list of active user.
        /// </summary>
        /// <returns>Returns list of active user.</returns>
        List<User> GetActiveUsers();

        /// <summary>
        /// Gets all Users by team.
        /// </summary>
        /// <param name="teamId">team Id</param>
        /// <returns>List of all users.</returns>
        List<User> GetAllUsersForTeam(int teamId);

        /// <summary>
        /// interface method GetActiveUsers by team 
        /// </summary>
        /// <returns>List of User</returns>
        List<User> GetActiveUsersByTeam(int teamId);
    }
}
