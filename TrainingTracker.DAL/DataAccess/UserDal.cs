﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TrainingTracker.Common.Utility;
using TrainingTracker.Common.ViewModel;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using TrainingTracker.Common.Entity;
using CodeReviewPoint = TrainingTracker.Common.Entity.CodeReviewPoint;
using Feedback = TrainingTracker.Common.Entity.Feedback;
using FeedbackType = TrainingTracker.Common.Entity.FeedbackType;
using Notification = TrainingTracker.Common.Entity.Notification;
using NotificationType = TrainingTracker.Common.Enumeration.NotificationType;
using Skill = TrainingTracker.Common.Entity.Skill;
using User = TrainingTracker.Common.Entity.User;

namespace TrainingTracker.DAL.DataAccess
{
    /// <summary>
    /// Class for User Dal Implementation
    /// </summary>
    public class UserDal : IUserDal
    {
        /// <summary>
        /// Calls stored procedure which validates user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <returns>True if valid customer.</returns>
        public bool ValidateUser(User userData)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Users.Any(x => x.UserName == userData.UserName && x.Password == userData.Password && (x.IsAdministrator == true || x.TeamId.HasValue));
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        /// <summary>
        /// Calls stored procedure which adds user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <param name="userId">Out parameter created UserId.</param>
        /// <returns>True if added.</returns>
        public bool AddUser(User userData, out int userId)
        {
            userId = 0;
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    EntityFramework.User objUser = new EntityFramework.User
                                                   {
                                                       FirstName = userData.FirstName,
                                                       LastName = userData.LastName,
                                                       UserName = userData.UserName,
                                                       Password = userData.Password,
                                                       Email = userData.Email,
                                                       Designation = userData.Designation,
                                                       ProfilePictureName = userData.ProfilePictureName,
                                                       IsFemale = userData.IsFemale,
                                                       IsAdministrator = userData.IsAdministrator,
                                                       IsTrainer = userData.IsTrainer,
                                                       IsTrainee = userData.IsTrainee,
                                                       IsManager = userData.IsManager,
                                                       DateAddedToSystem = DateTime.Now,
                                                       IsActive = userData.IsActive,
                                                       TeamId = userData.TeamId,
                                                       EmployeeId = userData.EmployeeId
                                                   };

                    context.Users.Add(objUser);
                    context.SaveChanges();
                    userId = objUser.UserId;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        /// <summary>
        /// Calls stored procedure which updates user.
        /// </summary>
        /// <param name="userData">User data object.</param>
        /// <returns>True if updated.</returns>
        public bool UpdateUser(User userData)
        {
            if (userData.UserId <= 0) return false;

            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    var userContext = context.Users.FirstOrDefault(x => x.UserId == userData.UserId);

                    if (userContext == null) return false;

                    userContext.FirstName = userData.FirstName;
                    userContext.LastName = userData.LastName;
                    userContext.UserName = userData.UserName;
                    userContext.Email = userData.Email;
                    userContext.Designation = userData.Designation;
                    userContext.ProfilePictureName = userData.ProfilePictureName;
                    userContext.IsFemale = userData.IsFemale;
                    userContext.IsAdministrator = userData.IsAdministrator;
                    userContext.IsTrainer = userData.IsTrainer;
                    userContext.IsTrainee = userData.IsTrainee;
                    userContext.IsManager = userData.IsManager;
                    userContext.IsActive = userData.IsActive;
                    userContext.EmployeeId = userData.EmployeeId;
                    if (!string.IsNullOrEmpty(userData.Password))
                    {
                        userContext.Password = userData.Password;
                    }
                    userContext.TeamId = userData.TeamId;

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        /// <summary>
        /// Gets all Users.
        /// </summary>
        /// <returns>List of all users.</returns>
        public List<User> GetAllUsers()
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Users.Select(x => new User
                                                     {
                                                         UserId = x.UserId,
                                                         FirstName = x.FirstName,
                                                         LastName = x.LastName,
                                                         FullName = x.FirstName + " " + x.LastName,
                                                         UserName = x.UserName,
                                                         Email = x.Email,
                                                         Designation = x.Designation,
                                                         ProfilePictureName = x.ProfilePictureName,
                                                         IsFemale = x.IsFemale ?? false,
                                                         IsAdministrator = x.IsAdministrator ?? false,
                                                         IsTrainer = x.IsTrainer ?? false,
                                                         IsTrainee = x.IsTrainee ?? false,
                                                         IsManager = x.IsManager ?? false,
                                                         IsActive = x.IsActive ?? false,
                                                         DateAddedToSystem = x.DateAddedToSystem,
                                                         TeamId = x.TeamId,
                                                         EmployeeId = x.EmployeeId
                                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// Fetch user by User Name
        /// </summary>
        /// <param name="userName">string of username to be searched</param>
        /// <returns>Instance of User</returns>
        public User GetUserByUserName(string userName)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    //No user can have same UserName, so used First in IQuerable clause intented to throw exception.
                    return context.Users.Where(x => x.UserName == userName)
                                  .Select(x => new User
                                               {
                                                   UserId = x.UserId,
                                                   FirstName = x.FirstName,
                                                   LastName = x.LastName,
                                                   UserName = x.UserName,
                                                   Email = x.Email,
                                                   Designation = x.Designation,
                                                   ProfilePictureName = x.ProfilePictureName,
                                                   IsFemale = x.IsFemale ?? false,
                                                   IsAdministrator = x.IsAdministrator ?? false,
                                                   IsTrainer = x.IsTrainer ?? false,
                                                   IsTrainee = x.IsTrainee ?? false,
                                                   IsManager = x.IsManager ?? false,
                                                   IsActive = x.IsActive ?? false,
                                                   DateAddedToSystem = x.DateAddedToSystem,
                                                   UserRating = 0,
                                                   TeamId = x.TeamId,
                                                   EmployeeId = x.EmployeeId
                                               }).First();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return new User();
            }
        }

        /// <summary>
        /// Fetch User by UserId
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>Instance of user</returns>
        public User GetUserById(int userId)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    //No user can have same user id it's an Primary key, so used First in IQuerable clause intented to throw exception.
                    return context.Users.Where(x => x.UserId == userId)
                                  .Select(x => new User
                                               {
                                                   UserId = x.UserId,
                                                   FirstName = x.FirstName,
                                                   LastName = x.LastName,
                                                   UserName = x.UserName,
                                                   Email = x.Email,
                                                   FullName = x.FirstName + " " + x.LastName,
                                                   Designation = x.Designation,
                                                   ProfilePictureName = x.ProfilePictureName,
                                                   IsFemale = x.IsFemale ?? false,
                                                   IsAdministrator = x.IsAdministrator ?? false,
                                                   IsTrainer = x.IsTrainer ?? false,
                                                   IsTrainee = x.IsTrainee ?? false,
                                                   IsManager = x.IsManager ?? false,
                                                   IsActive = x.IsActive ?? false,
                                                   DateAddedToSystem = x.DateAddedToSystem,
                                                   UserRating = 0,
                                                   TeamId = x.TeamId,
                                                   EmployeeId = x.EmployeeId
                                               }).First();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }


        /// <summary>
        /// Public method GetUserId returns list of user id as per the condition,
        /// So specific notifications are added to specific user profile.
        /// </summary>
        /// <param name="notification">Contain notificationType as parameter.</param>
        /// <param name="addedFor">Contain notificationType as parameter.</param>
        /// <returns>Returns user Ids as a list.</returns>
        public List<int> GetUserId(Notification notification, int addedFor)
        {
            try
            {
                User currentUser = GetUserById(addedFor);

                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    switch (notification.TypeOfNotification)
                    {
                        case NotificationType.CodeReviewFeedbackNotification:
                        case NotificationType.AssignmentFeedbackNotification:
                        case NotificationType.SkillFeedbackNotification:
                        case NotificationType.CommentFeedbackNotification:
                        case NotificationType.WeeklyFeedbackNotification:
                        case NotificationType.NewNoteToFeedback:
                        case NotificationType.CourseFeedbackNotification:
                        case NotificationType.NewDiscussionPostNotification:
                        case NotificationType.NewDiscussionThreadNotification:
                        case NotificationType.RandomReviewFeedbackNotification:
                            return context.Users
                                          .Where(x => (x.UserId == addedFor || x.IsTrainer == true || x.IsManager == true)
                                                      && (x.IsActive == true && x.UserId != notification.AddedBy)
                                                      && (currentUser.TeamId.HasValue && x.TeamId == currentUser.TeamId))
                                          .Select(x => x.UserId)
                                          .ToList();

                        case NotificationType.NewReleaseNotification:
                        case NotificationType.NewFeatureRequestNotification:
                        case NotificationType.FeatureModifiedNotification:
                            return context.Users.Where(x => x.UserId != notification.AddedBy && x.IsActive == true)
                                          .Select(x => x.UserId)
                                          .ToList();

                        case NotificationType.NewUserNotification:
                        case NotificationType.UserActivatedNotification:
                        case NotificationType.NewSessionNotification:
                        case NotificationType.SessionUpdatedNotification:
                            return context.Users.Where(x => (x.IsManager == true || x.IsTrainer == true)
                                                            && (x.IsActive == true && x.UserId != notification.AddedBy)
                                                            && (currentUser.TeamId.HasValue && x.TeamId == currentUser.TeamId))
                                          .Select(x => x.UserId)
                                          .ToList();
                    }
                    return null;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// Public method GetActiveUsers returns list of user which IsActive field is true.
        /// </summary>
        /// <returns></returns>
        public List<User> GetActiveUsers()
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Users.Where(x => x.IsActive == true)
                                  .OrderByDescending(x => x.IsAdministrator == true)
                                  .ThenByDescending(x => x.IsManager == true)
                                  .ThenByDescending(x => x.IsTrainer == true)
                                  .ThenByDescending(x => x.IsTrainee == true)
                                  .ThenBy(x => x.FirstName)
                                  .Select(x => new User
                                               {
                                                   UserId = x.UserId,
                                                   FirstName = x.FirstName,
                                                   LastName = x.LastName,
                                                   FullName = x.FirstName + " " + x.LastName,
                                                   UserName = x.UserName,
                                                   Email = x.Email,
                                                   Designation = x.Designation,
                                                   ProfilePictureName = x.ProfilePictureName,
                                                   IsFemale = x.IsFemale ?? false,
                                                   IsAdministrator = x.IsAdministrator ?? false,
                                                   IsTrainer = x.IsTrainer ?? false,
                                                   IsTrainee = x.IsTrainee ?? false,
                                                   IsManager = x.IsManager ?? false,
                                                   IsActive = x.IsActive ?? false,
                                                   DateAddedToSystem = x.DateAddedToSystem,
                                                   TeamId = x.TeamId,
                                                   EmployeeId = x.EmployeeId
                                               }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// Public method GetActiveUsers by team 
        /// </summary>
        /// <returns>List of User</returns>
        public List<User> GetActiveUsersByTeam(int teamId)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Users.Where(x => x.IsActive == true && x.TeamId == teamId)
                                  .OrderByDescending(x => x.IsAdministrator == true)
                                  .ThenByDescending(x => x.IsManager == true)
                                  .ThenByDescending(x => x.IsTrainer == true)
                                  .ThenByDescending(x => x.IsTrainee == true)
                                  .ThenBy(x => x.FirstName)
                                  .Select(x => new User
                                               {
                                                   UserId = x.UserId,
                                                   FirstName = x.FirstName,
                                                   LastName = x.LastName,
                                                   FullName = x.FirstName + " " + x.LastName,
                                                   UserName = x.UserName,
                                                   Email = x.Email,
                                                   Designation = x.Designation,
                                                   ProfilePictureName = x.ProfilePictureName,
                                                   IsFemale = x.IsFemale ?? false,
                                                   IsAdministrator = x.IsAdministrator ?? false,
                                                   IsTrainer = x.IsTrainer ?? false,
                                                   IsTrainee = x.IsTrainee ?? false,
                                                   IsManager = x.IsManager ?? false,
                                                   IsActive = x.IsActive ?? false,
                                                   DateAddedToSystem = x.DateAddedToSystem,
                                                   TeamId = x.TeamId,
                                                   EmployeeId = x.EmployeeId != null ? x.EmployeeId : ""
                                               }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// Get All User for the team
        /// </summary>
        /// <param name="teamId">team id</param>
        /// <returns>List of User</returns>
        public List<User> GetAllUsersForTeam(int teamId)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Users.Where(x => x.TeamId == teamId).Select(x => new User
                                                                                    {
                                                                                        UserId = x.UserId,
                                                                                        FirstName = x.FirstName,
                                                                                        LastName = x.LastName,
                                                                                        FullName = x.FirstName + " " + x.LastName,
                                                                                        UserName = x.UserName,
                                                                                        Email = x.Email,
                                                                                        Designation = x.Designation,
                                                                                        ProfilePictureName = x.ProfilePictureName,
                                                                                        IsFemale = x.IsFemale ?? false,
                                                                                        IsAdministrator = x.IsAdministrator ?? false,
                                                                                        IsTrainer = x.IsTrainer ?? false,
                                                                                        IsTrainee = x.IsTrainee ?? false,
                                                                                        IsManager = x.IsManager ?? false,
                                                                                        IsActive = x.IsActive ?? false,
                                                                                        DateAddedToSystem = x.DateAddedToSystem,
                                                                                        TeamId = teamId,
                                                                                        EmployeeId = x.EmployeeId
                                                                                    }).OrderBy(x => x.UserName).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }


        /// <summary>
        /// Get all the Trainees List belong to a specified team.
        /// </summary>
        /// <param name="teamId">teamId which is used to fetch the Trainees data of that team only.</param>
        /// <returns>List of User object if successfully fetches some data and returns null if no data is present or some exception occurs. </returns>
        public List<User> GetAllTrainees(int teamId)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    return context.Users.Where(l => l.IsActive == true && l.TeamId == teamId && l.IsTrainee == true)
                                  .Select(x => new User
                                               {
                                                   UserId = x.UserId,
                                                   FirstName = x.FirstName,
                                                   LastName = x.LastName,
                                                   FullName = x.FirstName + " " + x.LastName,
                                                   UserName = x.UserName,
                                                   Email = x.Email,
                                                   Designation = x.Designation,
                                                   ProfilePictureName = x.ProfilePictureName,
                                                   IsFemale = x.IsFemale ?? false,
                                                   IsAdministrator = x.IsAdministrator ?? false,
                                                   IsTrainer = x.IsTrainer ?? false,
                                                   IsTrainee = x.IsTrainee ?? false,
                                                   IsManager = x.IsManager ?? false,
                                                   IsActive = x.IsActive ?? false,
                                                   DateAddedToSystem = x.DateAddedToSystem,
                                                   TeamId = x.TeamId
                                               })
                                  .ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// Gets all Designation.
        /// </summary>
        /// <returns>List of all designation.</returns>
        public List<Designation> GetAllDesignation()
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Users.Select(x => new Designation
                                                     {
                                                         DesignationName = x.Designation
                                                     })
                                  .Where(x => x.DesignationName != null)
                                  .Distinct()
                                  .OrderBy(x => x.DesignationName)
                                  .ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }
    }
}