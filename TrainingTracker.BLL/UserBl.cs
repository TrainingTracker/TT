using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.ViewModel;

namespace TrainingTracker.BLL
{
    /// <summary>
    /// Bussiness class for user
    /// </summary>
    public class UserBl : BussinessBase
    {

        /// <summary>
        /// Calls stored procedure which adds user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <param name="userId">Out parameter created UserId.</param>
        /// <returns>True if added.</returns>
        public bool AddUser( User userData , out int userId )
        {
            userData.Password = Common.Encryption.Cryptography.Encrypt(userData.Password);
            return UserDataAccesor.AddUser(userData , out userId);
        }

        /// <summary>
        /// Calls stored procedure which updates user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <returns>True if updated.</returns>
        public bool UpdateUser( User userData )
        {
            userData.Password = Common.Encryption.Cryptography.Encrypt(userData.Password);
            return UserDataAccesor.UpdateUser(userData);
        }

        /// <summary>
        /// GEt all user
        /// </summary>
        /// <returns>List of User</returns>
        public List<User> GetAllUsers()
        {
            return UserDataAccesor.GetAllUsers();
        }

        /// <summary>
        /// GEt all user
        /// </summary>
        /// <returns>List of User</returns>
        public List<User> GetAllUsersByTeam( User currentUser )
        {
            if (currentUser.IsAdministrator && !currentUser.TeamId.HasValue) return UserDataAccesor.GetAllUsers();

            return currentUser.TeamId.HasValue
                              ? UserDataAccesor.GetAllUsersForTeam(currentUser.TeamId.Value)
                              : new List<User>();          
        }

        /// <summary>
        /// Get User By name
        /// </summary>
        /// <param name="userName">string of User Name</param>
        /// <returns>instance of User object</returns>
        public User GetUserByUserName( string userName )
        {
            return (string.IsNullOrEmpty(userName)) ? new User() : UserDataAccesor.GetUserByUserName(userName);
        }

        /// <summary>
        /// Get User By user id
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>instance of User object</returns>
        public User GetUserByUserId( int userId )
        {
            return UserDataAccesor.GetUserById(userId);
        }

        /// <summary>
        /// Get view model for user profile page
        /// </summary>
        /// <param name="userId">logged in user id</param>
        /// <returns>instance of User vm</returns>
        public UserProfileVm GetUserProfileVm( int userId )
        {
            User currentUser = UserDataAccesor.GetUserById(userId);

            return new UserProfileVm
            {
                User = currentUser ,
                Skills = currentUser.IsTrainee ? SkillDataAccesor.GetSkillsByUserId(userId) : new List<Skill>() ,
                AllSkills = currentUser.IsTrainee ? SkillDataAccesor.GetAllSkillsForApp() : new List<Skill>(),
                Sessions = currentUser.IsTrainee ? SessionDataAccesor.GetSessionsByUserId(userId) : new List<Session>() ,
                Projects =  new List<Project>(),
                Feedbacks = currentUser.IsTrainee ? FeedbackDataAccesor.GetUserFeedback(userId , 5) : FeedbackDataAccesor.GetFeedbackAddedByUser(userId),
                RecentCrFeedback = currentUser.IsTrainee ? FeedbackDataAccesor.GetUserFeedback(userId , 1000 , 4) : new List<Feedback>() ,
                RecentWeeklyFeedback = currentUser.IsTrainee ? FeedbackDataAccesor.GetUserFeedback(userId , 1000 , 5) : new List<Feedback>() ,
                AllTrainer = currentUser.IsTrainee ? GetAllUsersByTeam(currentUser).Where(x => x.IsTrainer || x.IsManager).ToList() : new List<User>() ,
                FeedbackTypes = Common.Utility.UtilityFunctions.GetSystemFeedbackTypes(),
                TrainorSynopsis = currentUser.IsTrainer || currentUser.IsManager ? FeedbackDataAccesor.GetTrainorFeedbackSynopsis(currentUser.UserId) :new TrainerFeedbackSynopsis()
            };            
        }

        /// <summary>
        /// Function for getting list of active user.
        /// </summary>
        /// <returns>Returns list of active user.</returns>
        public List<User> GetActiveUsers( User currentUser )
        {
            if (currentUser.IsAdministrator && !currentUser.TeamId.HasValue) return UserDataAccesor.GetActiveUsers();

            return currentUser.TeamId.HasValue
                              ? UserDataAccesor.GetActiveUsersByTeam(currentUser.TeamId.Value).Where(x => !currentUser.IsTrainee || !x.IsTrainee || x.UserId==currentUser.UserId ).ToList()
                              : new List<User>();          
        }

        /// <summary>
        /// Fetches user Feedback based on filter
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="pageSize">no of record count</param>
        /// <param name="feedbackId">feedback type id</param>
        /// <param name="startAddedOn">start date </param>
        /// <param name="endAddedOn">end date</param>
        /// <returns></returns>
        public List<Feedback> GetUserFeedbackOnFilter( int userId , int pageSize , int feedbackId , DateTime? startAddedOn = null , DateTime? endAddedOn = null )
        {
            // truncate any time part from the filter, if only if the variables have any value
            if (startAddedOn.HasValue) startAddedOn = startAddedOn.Value.Date;
            if (endAddedOn.HasValue) endAddedOn = endAddedOn.Value.Date;

            return FeedbackDataAccesor.GetUserFeedback(userId , pageSize , feedbackId , startAddedOn , endAddedOn);
        }

        /// <summary>
        /// fetches the user feedback based on filters
        /// </summary>
        /// <param name="traineeId">trainee's id</param>
        /// <param name="startDate">start date</param>
        /// <param name="endDate">end date</param>
        /// <param name="arrayFeedbackType">array of feedback type</param>
        /// <param name="trainerId">trainer id</param>
        /// <returns>returns instances of Feedback Plots</returns>
        public FeedbackPlot GetUserFeedbackOnFilterForPlot( int traineeId , DateTime? startDate , DateTime? endDate ,
                                                                          string arrayFeedbackType , int trainerId )
        {

            FeedbackPlot objfeedbackPlot = new FeedbackPlot
            {
                AssignmentFeedbacks = new List<Feedback>() ,
                CodeReviewFeedbacks = new List<Feedback>() ,
                WeeklyFeedbacks = new List<Feedback>()
            };

            if (string.IsNullOrEmpty(arrayFeedbackType)) return objfeedbackPlot;

            int[] feedbackTypes = Array.ConvertAll(arrayFeedbackType.Split(',') , int.Parse);

            foreach (var type in feedbackTypes)
            {
                switch (type)
                {
                    case 3:
                        objfeedbackPlot.AssignmentFeedbacks = FeedbackDataAccesor.GetUserFeedback(traineeId , 1000 , type)
                                                                                 .Where(x =>
                                                                                     (trainerId == 0 || x.AddedBy.UserId == trainerId) &&
                                                                                             (!startDate.HasValue || x.AddedOn > startDate.Value.AddDays(-1)) &&
                                                                                             (!endDate.HasValue || x.AddedOn < endDate.Value.AddDays(1))
                                                                                     )
                                                                                 .ToList();
                        break;

                    case 4:
                        //  var testtt = FeedbackDataAccesor.GetUserFeedback(traineeId, 1000, type);


                        objfeedbackPlot.CodeReviewFeedbacks = FeedbackDataAccesor.GetUserFeedback(traineeId , 1000 , type)
                                                                                 .Where(x =>
                                                                                     (trainerId == 0 || x.AddedBy.UserId == trainerId) &&
                                                                                             (!startDate.HasValue || x.AddedOn > startDate.Value.AddDays(-1)) &&
                                                                                             (!endDate.HasValue || x.AddedOn < endDate.Value.AddDays(1))
                                                                                     )
                                                                                 .ToList();
                        break;

                    case 5:
                        objfeedbackPlot.WeeklyFeedbacks = FeedbackDataAccesor.GetUserFeedback(traineeId , 1000 , type)
                                                                              .Where(x =>
                                                                                     (trainerId == 0 || x.AddedBy.UserId == trainerId) &&
                                                                                             ((!startDate.HasValue || x.StartDate > startDate.Value.AddDays(-1)) ||
                                                                                             (!endDate.HasValue || x.EndDate < endDate.Value.AddDays(1)))
                                                                                     )
                                                                                 .ToList();
                        break;
                }
            }
            return objfeedbackPlot;
        }

        /// <summary>
        /// Get manage Profile View model
        /// </summary>
        /// <param name="currentUser">current user</param>
        public ManageProfileVm GetManageProfileVm( User currentUser )
        {
            return new ManageProfileVm
            {
                AllTeams = TeamDataAccesor.GetAllTeam() ,
                AllUser = GetAllUsersByTeam(currentUser)
            };
        }
    }
}