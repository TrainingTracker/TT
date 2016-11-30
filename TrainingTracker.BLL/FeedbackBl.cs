using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.BLL
{
    /// <summary>
    /// Business class for feedback, Implemets Business base
    /// </summary>
    public class FeedbackBl:BussinessBase
    {
        /// <summary>
        /// Add New feedback for user
        /// </summary>
        /// <param name="feedback">instance of feedback object</param>
        /// <returns>success flag wether feedback was added or not</returns>
        public bool AddFeedback(Feedback feedback)
        {
            feedback.Project = feedback.Project ?? new Project();
            feedback.Title = string.IsNullOrEmpty(feedback.Title) ? feedback.FeedbackType.Description : feedback.Title;
          
            if(feedback.FeedbackType.FeedbackTypeId == 2)
            {
                feedback.Title = feedback.Skill.Name;
                feedback.Skill = new Skill
                {
                    SkillId = feedback.Skill.SkillId
                };
            }
            else
            {
                feedback.Skill = new Skill();
            }

            // no way comment can have feedback rating
            if (feedback.FeedbackType.FeedbackTypeId == 1)
            {
                feedback.Rating = 0;
            }

            int feedbackId = FeedbackDataAccesor.AddFeedback(feedback);
           
            if (!(feedbackId>0)) return false;

            feedback.FeedbackId = feedbackId;

            return new NotificationBl().AddFeedbackNotification(feedback) ;
        }


        /// <summary>
        /// Fetches all feedback Threads
        /// </summary>
        /// <param name="feedbackId">feedbackId</param>
        /// <param name="currentUser">current user</param>
        public List<Threads> GetFeedbackThreads( int feedbackId , User currentUser)
        {
            int feedbackForUserId = FeedbackDataAccesor.GetTraineebyFeedbackId(feedbackId);

            if (!(currentUser.IsAdministrator || currentUser.IsManager || currentUser.IsTrainer || currentUser.UserId == feedbackForUserId)) return null;

            return FeedbackDataAccesor.GetFeedbackThreads(feedbackId);
        }

        /// <summary>
        /// Fetches all feedback Threads
        /// </summary>
        /// <param name="feedbackId">feedbackId</param>
        /// <param name="currentUser">current user </param>
        public Feedback GetFeedbackWithThreads( int feedbackId , User currentUser)
        {
            int feedbackForUserId = FeedbackDataAccesor.GetTraineebyFeedbackId(feedbackId);

            if (!(currentUser.IsAdministrator || currentUser.IsManager || currentUser.IsTrainer || currentUser.UserId==feedbackForUserId) ) return null;

            return FeedbackDataAccesor.GetFeedbackWithThreads(feedbackId);
        }

        /// <summary>
        /// Add New Thread to Feedback
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public bool AddNewThread(Threads thread)
        {
            return FeedbackDataAccesor.AddNewThread(thread) && new NotificationBl().AddNewThreadNotification(thread);
        }

        /// <summary>
        /// authorize Current user for team's Feedback
        /// </summary>
        /// <param name="feedbackId">feedback Id</param>
        /// <param name="currentUser">Current User</param>
        /// <returns>Success flag</returns>
        public bool AuthorizeCurrentUserForFeedback(int feedbackId, User currentUser)
        {
            return  (currentUser.IsAdministrator && !currentUser.TeamId.HasValue) || FeedbackDataAccesor.GetFeedbackWithThreads(feedbackId).AddedBy.TeamId == currentUser.TeamId ;
        }
    }
}