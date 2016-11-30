/**************************************************************************************************
*   Created By : Satyabrata                                                                                                                                                           
*   Created On : 12 Sept 2016
*   Modified By :
*   Modified Date:
*   Description: Data Access class for Notification.
**************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
namespace TrainingTracker.DAL.DataAccess
{
    /// <summary>
    /// Class for Notification Dal Implementation
    /// </summary>
    public class NotificationDal : INotificationDal
    {

        /// <summary>
        /// Function AddNotification which add notification for a list of user.
        /// </summary>
        /// <param name="notification">Notification object</param>
        /// <param name="userIds">List of userid</param>
        /// <returns>Return true if a notification is added successfully else false.</returns>
        public bool AddNotification(Common.Entity.Notification notification, List<int> userIds)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    var entityFrameworkNotification = new EntityFramework.Notification
                                                          {
                                                              Description = notification.Description,
                                                              Link = notification.Link,
                                                              NotificationType = (int)notification.TypeOfNotification,
                                                              AddedOn = notification.AddedOn,
                                                              NotificationTitle = notification.Title,
                                                              AddedBy = notification.AddedBy
                                                          };

                    foreach (var user in userIds)
                    {
                        entityFrameworkNotification.UserNotificationMappings.Add(new UserNotificationMapping
                                                                                     {
                                                                                         UserId = user,
                                                                                         Seen = false
                                                                                     });
                    }

                    context.Notifications.Add(entityFrameworkNotification);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                LogUtility.ErrorRoutine(e);
                return false;
            }
        }

        /// <summary>
        /// Function GetNotification Return list of notification.
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <returns>Return list of notification for a user</returns>
        public List<Common.Entity.Notification> GetNotification(int userId)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.UserNotificationMappings
                        .Where(x => x.UserId == userId && !x.Seen)
                            .Select(x => new Common.Entity.Notification
                            {
                                NotificationId = x.NotificationId,
                                Title = x.Notification.NotificationTitle,
                                Description = x.Notification.Description,
                                Link = x.Notification.Link,
                                TypeOfNotification = (Common.Enumeration.NotificationType)x.Notification.NotificationType,
                                AddedBy = x.Notification.AddedBy,
                                AddedOn = x.Notification.AddedOn,
                                UserDetails = new Common.Entity.User
                                {
                                    FirstName = x.Notification.User.FirstName ,
                                    LastName = x.Notification.User.LastName ,
                                    ProfilePictureName = x.Notification.User.ProfilePictureName
                                }
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
        /// Update notification as make seen status true. 
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="notification">Notification object</param>
        /// <returns>Returns boolean value according to Notification is added successfully or not</returns>
        public bool UpdateNotification(int userId, Common.Entity.Notification notification)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    context.UserNotificationMappings
                            .Where(s => s.UserId == userId && !s.Seen && s.NotificationId == notification.NotificationId)
                            .ToList()
                            .ForEach(s => { s.Seen = true; });

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
        /// Clear All the pending Notification  for user
        /// </summary>
        /// <param name="userId">passed userid</param>
        /// <returns>success flag </returns>
        public bool MarkAllNotificationAsRead(int userId)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                   context.UserNotificationMappings.Where(u=>u.UserId==userId && !u.Seen).ToList().ForEach(x => x.Seen = true);
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
    }
}
