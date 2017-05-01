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
        public async Task<User> AuthenticateUser(string userName, string password)
        {
            var responseBody = await GPSService.GPSService.AuthenticateUser(Constants.GpsWebApiUrl, Constants.GpsWebApiUrl , Constants.ApiKey, Constants.AppId, userName, password);
            var userData = new User
            {
                UserName = userName,
                Password = password
            };
            userData.IsValid = JsonConvert.DeserializeObject<bool>(responseBody);
            if(userData.IsValid)
            {
                var userExists = (string.IsNullOrEmpty(userName)) ? null : UserDataAccesor.GetUserByUserName(userName);
                if(userExists != null)
                {
                    if (userExists.UserId > 0)
                    {
                        userExists.IsValid = userData.IsValid;
                        return userExists;
                    }                                         
                }
                return null;
            }
            return userData;          
            
        }

    }
}