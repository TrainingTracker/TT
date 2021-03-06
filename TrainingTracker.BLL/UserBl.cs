﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Encryption;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;
using TrainingTracker.Common.ViewModel;
using CodeReviewTag = TrainingTracker.Common.Entity.CodeReviewTag;
using EmailAlertSubscription = TrainingTracker.DAL.EntityFramework.EmailAlertSubscription;
using TrainingTracker.DAL.DataAccess;
using Feedback = TrainingTracker.Common.Entity.Feedback;
using Skill = TrainingTracker.Common.Entity.Skill;
using User = TrainingTracker.Common.Entity.User;

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
        /// <param name="managerId">Id of manager adding.</param>
        /// <param name="userId">Out parameter created UserId.</param>
        /// <returns>True if added.</returns>
        public bool AddUser(User userData, int managerId, out int userId)
        {
            userData.Password = Cryptography.Encrypt(userData.Password);
            var isUserAdded = UserDataAccesor.AddUser(userData, out userId);

            var dbUser = GetUserByUserId(userId);

            var teamManagers = UnitOfWork.UserRepository
                                         .Find(u => u.TeamId == dbUser.TeamId && u.IsManager == true)
                                         .Select(lead => lead.UserId)
                                         .ToList();

            if (isUserAdded)
            {
                teamManagers.ForEach(manager => UnitOfWork.EmailAlertSubscriptionRepository
                                                          .AddOrUpdate(new EmailAlertSubscription
                                                                       {
                                                                           SubscribedByUserId = manager,
                                                                           SubscribedForUserId = dbUser.UserId
                                                                       }));
                UnitOfWork.Commit();
        }

            if (isUserAdded && dbUser.IsTrainee)
            {
                new NotificationBl().UserNotification(dbUser, managerId);
            }

            return isUserAdded;
        }

        /// <summary>
        /// Calls stored procedure which updates user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <param name="addedById">manager id</param>
        /// <returns>True if updated.</returns>
        public bool UpdateUser(User userData, int addedById)
        {
            userData.Password = Cryptography.Encrypt(userData.Password);
            var isUserUpdated = UserDataAccesor.UpdateUser(userData);
            var dbUser = GetUserByUserId(userData.UserId);
            var teamManagers = UnitOfWork.UserRepository
                                         .Find(u => u.TeamId == dbUser.TeamId && u.IsManager == true)
                                         .Select(lead => lead.UserId)
                                         .ToList();

            try
            {
                if (isUserUpdated && dbUser.IsTrainee)
                {
                    UnitOfWork.EmailAlertSubscriptionRepository
                              .GetAllSubscribedMentors(userData.UserId, includeDeleted: true)
                              .ForEach(s =>
                                       {
                                           s.IsDeleted = !(userData.IsActive && teamManagers.Contains(s.SubscribedByUserId));
                                           teamManagers.Remove(s.SubscribedByUserId);//Remove from managers if record exists. Also avoids duplicate notifications.
                                           UnitOfWork.EmailAlertSubscriptionRepository.AddOrUpdate(s);
                                       });

                    teamManagers.ForEach(manager => UnitOfWork.EmailAlertSubscriptionRepository
                                                              .AddOrUpdate(new EmailAlertSubscription
                                                                           {
                                                                               SubscribedByUserId = manager,
                                                                               SubscribedForUserId = dbUser.UserId
                                                                           }));

                    UnitOfWork.Commit();

                    if (dbUser.IsActive)
                    {
                        new NotificationBl().UserNotification(dbUser, addedById, isNewUser: false);
                    }
                }
            }
            catch (Exception ex)
        {
                LogUtility.ErrorRoutine(ex);
                isUserUpdated = false;
            }

            return isUserUpdated;
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
            return (string.IsNullOrEmpty(userName)) ? new User() : UserDataAccesor.GetUserByUserName(userName);
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
        /// <param name="userId">user id</param>
        /// <param name="loggedInUser"></param>
        /// <returns>instance of User vm</returns>
        public UserProfileVm GetUserProfileVm(int userId, User loggedInUser)
        {
            User requestedUser = userId == loggedInUser.UserId ? loggedInUser : UserDataAccesor.GetUserById(userId);

            CodeReview codeReview = loggedInUser.IsTrainer || loggedInUser.IsManager 
                                        ? CodeReviewConverter.ConvertFromCore(UnitOfWork.CodeReviewRepository.GetSavedCodeReviewForTrainee(userId, loggedInUser.UserId)) 
                                        : null;
            var commonTags = UnitOfWork.CodeReviewRepository
                .GetCommonlyUsedTags(userId, 5)
                                       .Select(skill => new CodeReviewTag
                        {
                            CodeReviewTagId = 0,
                            Skill = new Skill
                                    {
                                                                        Name = skill.Name,
                                      SkillId = skill.SkillId
                                    }
                        }).ToList();

            if (codeReview != null)
            {
                codeReview.CodeReviewPreviewHtml = UtilityFunctions.GenerateCodeReviewPreview(codeReview, true);
                codeReview.SystemRating = new FeedbackBl().CalculateCodeReviewRating(codeReview);
            }
           
            return new UserProfileVm
            {
                User = userId == loggedInUser.UserId ? null : requestedUser,
                Skills = requestedUser.IsTrainee ? SkillDataAccesor.GetSkillsByUserId(userId) : null,
                TraineeSynopsis = requestedUser.IsTrainee ? FeedbackDataAccesor.GetTraineeFeedbackSynopsis(requestedUser.UserId) : null,
                Sessions = requestedUser.IsTrainee ? SessionConverter.ConvertListFromCore(UnitOfWork.SessionRepository.GetAllSessionForAttendee(userId)) : null,
                Projects = null,
                Feedbacks = requestedUser.IsTrainee ? FeedbackDataAccesor.GetUserFeedback(userId, 5) : FeedbackDataAccesor.GetFeedbackAddedByUser(userId),
                TrainorSynopsis = requestedUser.IsTrainer || requestedUser.IsManager ? FeedbackDataAccesor.GetTrainorFeedbackSynopsis(requestedUser.UserId) : null,
                AllAssignedCourses = requestedUser.IsTrainee ? LearningPathDataAccessor.GetAllCoursesForTrainee(requestedUser.UserId).OrderByDescending(x => x.PercentageCompleted).ToList() : new List<CourseTrackerDetails>(),
                       SavedCodeReview = codeReview,
                CommonTags = commonTags
              //  SavedCodeReviewData = logedInUser.IsTrainer && (codeReview != null && codeReview.Id > 0) ? UtilityFunctions.GenerateCodeReviewPreview(codeReview, true) : string.Empty
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
                    if (ttMember.UserName == gpsMember.UserName)
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

        public List<Skill> AddSkill(Skill category)
        {
            if (SkillDataAccesor.AddSkill(category))
          {
              return SkillDataAccesor.GetAllSkillsForApp();
          }
          return null;
        }

        public List<Team> GetAllTeams()
        {
            return  TeamDataAccesor.GetAllTeam();
        }

    }
}                                                                                               