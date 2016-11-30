using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.BLL
{
    public class LoginBl:BussinessBase
    {
        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <returns>User data with validation information.</returns>
        public User AuthenticateUser(string userName, string password)
        {
            var userData = new User
            {
                UserName = userName,
                Password = password
            };
            userData.IsValid = UserDataAccesor.ValidateUser(userData);
            return userData;
        }
    }
}