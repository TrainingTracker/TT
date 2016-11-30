/**************************************************************************************************
*   Created By : Satyabrata                                                                                                                                                           
*   Created On : 11 Sept 2016
*   Modified By :
*   Modified Date:
*   Description: Business class for Notification.
**************************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Enumeration;
using FeedbackType = TrainingTracker.Common.Enumeration.FeedbackType;

namespace TrainingTracker.BLL
{
    public class NotificationBl : BussinessBase
    {
        private const string ReleaseLink = "/Release?releaseId={0}";
        private const string FeedbackLink = "/Profile/UserProfile?userId={0}&feedbackId={1}";
        private const string SessionLink = "/Session?sessionId={0}";
        private const string ReleaseDescription = "New release, Version:";
        private const string feedbackDescription = "New comment on";

       

        /// <summary>
        /// Add notification for a list of users. 
        /// </summary>
        /// <param name="notification">Notification class onject</param>
        /// <param name="userIds">List of userId</param>
        /// <returns>Returns true if Notification is added successfully else false.</returns>
        internal bool AddNotification(Notification notification, List<int> userIds)
        {
            return NotificationDataAccesor.AddNotification(notification, userIds);
        }

        /// <summary>
        /// Update notification which make notification seen status true.
        /// </summary>
        /// <param name="userId">UserId</param>
        /// <param name="notification">Notification object</param>
        /// <returns>Returns true if Notification is updated successfully else false.</returns>
        public List<Notification> UpdateNotification(int userId, Notification notification)
        {
            return NotificationDataAccesor.UpdateNotification(userId, notification) ? GetNotification(userId) : null;
        }


        /// <summary>
        /// Function which takes version generates notification message, list of userId, link
        /// Calls AddNotification() to save in the database.
        /// </summary>
        /// <param name="release">Release object</param>
        /// <param name="userId">UseId</param>
        /// <returns>Returns true if Notification is added successfully else false.</returns>
        internal bool AddReleaseNotification(Release release, int userId)
        {
            NotificationType notificationType;
            string featureText;

            if ( !release.IsPublished)
            {
                if (release.IsNew)
                {
                    notificationType = NotificationType.NewFeatureRequestNotification;
                    featureText = "New Feature/Bug Request";
                }
                else
                {
                    notificationType = NotificationType.FeatureModifiedNotification;
                    featureText = "Feature Details Updated";
                }
               
            }
            else 
            {
                notificationType = NotificationType.NewReleaseNotification;
                featureText = "New Release";
            }
          
            var notification = new Notification
            {
                Description = ReleaseDescription + release.Major + "." + release.Minor + "." + release.Patch,
                Link = string.Format(ReleaseLink,release.ReleaseId),
                TypeOfNotification = notificationType,
                AddedBy = userId,
                Title = featureText ,
                AddedOn = release.ReleaseDate ?? DateTime.Now,
            };
            return AddNotification(notification, UserDataAccesor.GetUserId(notification, userId));
        }

        /// <summary>
        /// Get list of notification.
        /// </summary>
        /// <param name="userId">UseId</param>
        /// <returns>Returns list of notification.</returns>
        public List<Notification> GetNotification(int userId)
        {
            return NotificationDataAccesor.GetNotification(userId);
        }

        /// <summary>
        /// Function which takes feedback data and user list to whom notification is to be added,
        /// Calls AddNotification() to save in the database.
        /// </summary>
        /// <param name="feedback">Contain Feedback object as parameter.</param>
        /// <returns>Returns a boolean value as feedback notification is added successfully or not.</returns>
        internal bool AddFeedbackNotification(Feedback feedback)
        {
            NotificationType notificationType;
            string notificationText = string.Empty;

            switch ((FeedbackType)feedback.FeedbackType.FeedbackTypeId)
            {
                case FeedbackType.Weekly:
                {
                    notificationType = NotificationType.WeeklyFeedbackNotification;
                    notificationText = "New Weekly Feedback";
                    break;
                }
                case FeedbackType.Comment:
                {
                    notificationType = NotificationType.CommentFeedbackNotification;
                    notificationText = "New Comment";
                    break;
                }
                case FeedbackType.Skill:
                {
                    notificationType = NotificationType.SkillFeedbackNotification;
                    notificationText = "New Skill";
                    break;
                }
                case FeedbackType.Assignment:
                {
                    notificationType = NotificationType.AssignmentFeedbackNotification;
                    notificationText = "New Assignment Feedback";
                    break;
                }
                case FeedbackType.CodeReview:
                {
                    notificationType = NotificationType.CodeReviewFeedbackNotification;
                    notificationText = "New CR Feedback";
                    break;
                }
                default:
                {
                    return false;
                }
            }

            var user = UserDataAccesor.GetUserById(feedback.AddedFor.UserId);

            var notification = new Notification
            {
                Description = user.FirstName  + " " + user.LastName    ,
                Link = string.Format(FeedbackLink, feedback.AddedFor.UserId,feedback.FeedbackId),
                TypeOfNotification = notificationType ,
                AddedBy = feedback.AddedBy.UserId,
                Title = notificationText ,
                AddedOn = DateTime.Now,
            };
            return AddNotification(notification, UserDataAccesor.GetUserId(notification, feedback.AddedFor.UserId));
        }

        /// <summary>
        ///  Add notification for user on Session
        /// </summary>
        /// <param name="session">Contain Session object as parameter.</param>
        /// <returns>Returns a boolean value as add session notification is added successfully or not.</returns>
        internal bool AddSessionNotification(Session session)
        {

            var notification = new Notification
            {
                Description = "New Session Added",
                Link = string.Format(SessionLink, session.Id),
                TypeOfNotification = session.IsNeW ? NotificationType.NewSessionNotification : NotificationType.SessionUpdatedNotification,
                AddedBy = session.Presenter,
                Title = session.IsNeW ? "New Session Added" : "Session Details Updated",
                AddedOn = DateTime.Now,
                AddedTo = session.Attendee
            };

            List<int> userIdList = UserDataAccesor.GetUserId(notification, session.Presenter);
            userIdList.AddRange(session.Attendee.Select(int.Parse).ToList());
            return AddNotification(notification , userIdList);
        }

        /// <summary>
        /// Mark All notification as read for given UserId
        /// </summary>
        /// <param name="userId">user Id</param>
        /// <returns>flag for success Event</returns>
        public bool MarkAllNotificationAsRead(int userId)
        {
            return NotificationDataAccesor.MarkAllNotificationAsRead(userId);
        }

        /// <summary>
        /// Add Notification for Thread
        /// </summary>
        /// <param name="thread"></param>
        internal bool AddNewThreadNotification(Threads thread)
        {

            int userId = FeedbackDataAccesor.GetTraineebyFeedbackId(thread.FeedbackId);

            if (userId == 0) return false;

            var notification = new Notification
            {
                Description = "New Note Added To Feedback" ,
                Link = string.Format(FeedbackLink , userId , thread.FeedbackId) ,
                TypeOfNotification = NotificationType.NewNoteToFeedback ,
                AddedBy = thread.AddedBy.UserId ,
                Title = "New Note Added to Feedback" ,
                AddedOn = DateTime.Now ,
            };

            return AddNotification(notification , UserDataAccesor.GetUserId(notification , userId));
        }
    }
}
