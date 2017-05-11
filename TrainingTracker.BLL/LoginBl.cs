using Newtonsoft.Json;
using System.Threading.Tasks;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.BLL
{
    public class LoginBl : BusinessBase
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

        /// <summary>
        /// Authenticates the user.
        /// </summary>
        /// <returns>User data with validation information.</returns>
        public async Task<User> GPSAuthentication(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName)) return null;

            var responseBody = await GPSService.GPSService.AuthenticateUser(Constants.GpsWebApiUrl, Constants.GpsWebApiUrl, Constants.ApiKey, Constants.AppId, userName, password);
            var userData = new User
            {
                UserName = userName,
                Password = password,
                IsValid = JsonConvert.DeserializeObject<bool>(responseBody)
            };

            if (!userData.IsValid) return userData;

            var userExists = UserDataAccesor.GetUserByUserName(userName);
            if (userExists == null && userExists.UserId == 0) return null;

            userExists.IsValid = userData.IsValid;
            return userExists;
        }

    }
}