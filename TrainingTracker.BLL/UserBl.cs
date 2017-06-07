using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.ViewModel;
using TrainingTracker.DAL.EntityFramework;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using System.Dynamic;
using Newtonsoft.Json;
using Feedback = TrainingTracker.Common.Entity.Feedback;
using Project = TrainingTracker.Common.Entity.Project;
using Session = TrainingTracker.Common.Entity.Session;
using Skill = TrainingTracker.Common.Entity.Skill;
using User = TrainingTracker.Common.Entity.User;
using TrainingTracker.Common.Utility;
using TrainingTracker.Common.Constants;

namespace TrainingTracker.BLL
{
    /// <summary>
    /// Bussiness class for user
    /// </summary>
    public class UserBl : BusinessBase
    {

        /// <summary>
        /// Calls stored procedure which adds user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <param name="userId">Out parameter created UserId.</param>
        /// <returns>True if added.</returns>
        public bool AddUser(User userData, out int userId)
        {
            userData.Password = Common.Encryption.Cryptography.Encrypt(userData.Password);
            return UserDataAccesor.AddUser(userData, out userId);
        }

        /// <summary>
        /// Calls stored procedure which updates user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <returns>True if updated.</returns>
        public bool UpdateUser(User userData)
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
        public List<User> GetAllUsersByTeam(User currentUser)
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
        public User GetUserByUserName(string userName)
        {
            return (string.IsNullOrEmpty(userName)) ? new User() :  UserDataAccesor.GetUserByUserName(userName);
        }

        /// <summary>
        /// Get User By user id
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>instance of User object</returns>
        public User GetUserByUserId(int userId)
        {
            return UserDataAccesor.GetUserById(userId);
        }

        /// <summary>
        /// Get view model for user profile page
        /// </summary>
        /// <param name="userId">logged in user id</param>
        /// <returns>instance of User vm</returns>
        public UserProfileVm GetUserProfileVm(int userId, User logedInUser)
        {
            User currentUser = userId == logedInUser.UserId ? logedInUser : UserDataAccesor.GetUserById(userId);

            CodeReview codeReview = logedInUser.IsTrainer 
                                        ? CodeReviewConverter.ConvertFromCore(UnitOfWork.CodeReviewRepository.GetSavedCodeReviewForTrainee(userId, logedInUser.UserId)) 
                                        :null;

            return new UserProfileVm
            {

                User = userId == logedInUser.UserId ? null : currentUser,
                Skills = currentUser.IsTrainee ? SkillDataAccesor.GetSkillsByUserId(userId) : null,
                TraineeSynopsis = currentUser.IsTrainee ? FeedbackDataAccesor.GetTraineeFeedbackSynopsis(currentUser.UserId) : null,
                Sessions = currentUser.IsTrainee ? SessionConverter.ConvertListFromCore(UnitOfWork.SessionRepository.GetAllSessionForAttendee(userId)) : null,
                Projects = null,
                Feedbacks = currentUser.IsTrainee ? FeedbackDataAccesor.GetUserFeedback(userId, 5) : FeedbackDataAccesor.GetFeedbackAddedByUser(userId),
                TrainorSynopsis = currentUser.IsTrainer || currentUser.IsManager ? FeedbackDataAccesor.GetTrainorFeedbackSynopsis(currentUser.UserId) : null,
                AllAssignedCourses = currentUser.IsTrainee ? LearningPathDataAccessor.GetAllCoursesForTrainee(currentUser.UserId).OrderByDescending(x => x.PercentageCompleted).ToList() : new List<CourseTrackerDetails>(),
                SavedCodeReview = codeReview ,
                SavedCodeReviewData = logedInUser.IsTrainer && (codeReview != null && codeReview.Id > 0) ? UtilityFunctions.GenerateCodeReviewPreview(codeReview, false) : string.Empty
            };
        }

        /// <summary>
        /// Function for getting list of active user.
        /// </summary>
        /// <returns>Returns list of active user.</returns>
        public List<User> GetActiveUsers(User currentUser)
        {
            if (currentUser.IsAdministrator && !currentUser.TeamId.HasValue) return UserDataAccesor.GetActiveUsers();

            return currentUser.TeamId.HasValue
                              ? UserDataAccesor.GetActiveUsersByTeam(currentUser.TeamId.Value).Where(x => !currentUser.IsTrainee || !x.IsTrainee || x.UserId == currentUser.UserId).ToList()
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
        public List<Feedback> GetUserFeedbackOnFilter(int userId, int pageSize, int feedbackId, DateTime? startAddedOn = null, DateTime? endAddedOn = null)
        {
            // truncate any time part from the filter, if only if the variables have any value
            if (startAddedOn.HasValue) startAddedOn = startAddedOn.Value.Date;
            if (endAddedOn.HasValue) endAddedOn = endAddedOn.Value.Date;

            return FeedbackDataAccesor.GetUserFeedback(userId, pageSize, feedbackId, startAddedOn, endAddedOn);
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
        public FeedbackPlot GetUserFeedbackOnFilterForPlot(int traineeId, DateTime? startDate, DateTime? endDate,
                                                                          string arrayFeedbackType, int trainerId)
        {

            FeedbackPlot objfeedbackPlot = new FeedbackPlot
            {
                AssignmentFeedbacks = new List<Feedback>(),
                CodeReviewFeedbacks = new List<Feedback>(),
                WeeklyFeedbacks = new List<Feedback>()
            };

            if (string.IsNullOrEmpty(arrayFeedbackType)) return objfeedbackPlot;

            int[] feedbackTypes = Array.ConvertAll(arrayFeedbackType.Split(','), int.Parse);

            foreach (var type in feedbackTypes)
            {
                switch (type)
                {
                    case 3:
                        objfeedbackPlot.AssignmentFeedbacks = FeedbackDataAccesor.GetUserFeedback(traineeId, 1000, type)
                                                                                 .Where(x =>
                                                                                     (trainerId == 0 || x.AddedBy.UserId == trainerId) &&
                                                                                             (!startDate.HasValue || x.AddedOn > startDate.Value.AddDays(-1)) &&
                                                                                             (!endDate.HasValue || x.AddedOn < endDate.Value.AddDays(1))
                                                                                     )
                                                                                 .ToList();
                        break;

                    case 4:
                        //  var testtt = FeedbackDataAccesor.GetUserFeedback(traineeId, 1000, type);


                        objfeedbackPlot.CodeReviewFeedbacks = FeedbackDataAccesor.GetUserFeedback(traineeId, 1000, type)
                                                                                 .Where(x =>
                                                                                     (trainerId == 0 || x.AddedBy.UserId == trainerId) &&
                                                                                             (!startDate.HasValue || x.AddedOn > startDate.Value.AddDays(-1)) &&
                                                                                             (!endDate.HasValue || x.AddedOn < endDate.Value.AddDays(1))
                                                                                     )
                                                                                 .ToList();
                        break;

                    case 5:
                        objfeedbackPlot.WeeklyFeedbacks = FeedbackDataAccesor.GetUserFeedback(traineeId, 1000, type)
                                                                              .Where(x =>
                                                                                     (trainerId == 0 || x.AddedBy.UserId == trainerId) &&
                                                                                             ((!startDate.HasValue || x.StartDate > startDate.Value.AddDays(-1)) &&
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
        public ManageProfileVm GetManageProfileVm(User currentUser)
        {
            return new ManageProfileVm
            {
                AllTeams = TeamDataAccesor.GetAllTeam(),
                AllUser = GetAllUsersByTeam(currentUser)
            };
        }


        /// <summary>
        ///  Get List of all trainees which belongs to given teamId
        /// </summary>
        /// <param name="teamId">teamId </param>
        /// <returns>List of trainees if exists otherwise null</returns>
        public List<User> GetAllTrainees(int teamId)
        {
            return (UserDataAccesor.GetAllTrainees(teamId));
        }


        /// <summary>
        ///  Get List of all skills 
        /// </summary>
        /// <returns>List of Skills if exists otherwise null</returns>
        public List<Skill> GetAllSkills()
        {
            return SkillDataAccesor.GetAllSkillsForApp();
        }

        public async Task<List<User>> SyncGPSUsers(User currentUser)
        {
            List<User> gpsMembersUnderLead = await GetMembersUnderLead(currentUser.EmployeeId);
            List<User> ttMembersUnderLead = GetManageProfileVm(currentUser).AllUser;
            List<User> unsyncedMembers = new List<User>();
            foreach (var gpsMember in gpsMembersUnderLead)
            {
                foreach (var ttMember in ttMembersUnderLead)
                {
                    if(ttMember.UserName == gpsMember.UserName)
                    {
                        ttMember.EmployeeId = gpsMember.EmployeeId;
                        if (!UserDataAccesor.UpdateUser(ttMember))
                        {
                            unsyncedMembers.Add(ttMember);
                        }
                    }
                    else
                    {
                        unsyncedMembers.Add(ttMember);
                    }
                }
            }
            return unsyncedMembers;
        }

        public async Task<List<User>> GetMembersUnderLead(string userId)
        {
            var responseBody = await GPSService.GPSService.SendRequest(new Uri(Constants.GpsWebApiUrl) + "Users/" + userId + "/TeamMembers", new Uri(Constants.GpsWebApiUrl), Constants.ApiKey, Constants.AppId);
            var data = JsonConvert.DeserializeObject<List<User>>(responseBody);
            return (data != null ? data : null);
        }

        /// <summary>
        /// Get all designation
        /// </summary>
        /// <returns>List of Designation</returns>
        public List<Designation> GetAllDesignation()
        {
            return UserDataAccesor.GetAllDesignation();
        }
    }
}                                                                                               