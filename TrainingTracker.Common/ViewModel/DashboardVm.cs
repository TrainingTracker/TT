using System;
using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Common.ViewModel
{
    public class DashboardVm
    {
        public List<UserData> Trainees { get; set; }
       // public List<Session> UpcomingSessions { get; set; }
    }

    public class UserData
    {
        public User User { get; set; }

        /// <summary>
        /// List of all Weekly feedbacks
        /// </summary>
        public List<Feedback> WeeklyFeedback { get; set; }

        /// <summary>
        /// List of All skills and other feedback
        /// </summary>
        public List<Feedback> SkillsFeedback { get; set; }

        /// <summary>
        /// List of All Remaining Feedbacks
        /// </summary>
        public List<Feedback> RemainingFeedbacks { get; set; }

        /// <summary>
        /// Lastweek feedback
        /// </summary>
        public bool LastWeekFeedbackAdded { get; set; }

        /// <summary>
        /// List of weeks where feedback is not added for user.
        /// </summary>
        public List<string> WeekForFeedbackNotPresent { get; set; }

         /// <summary>
        /// Gets and sets last Code Review Date
        /// </summary>
        public DateTime LastCodeReviewDate { get; set; }

        /// <summary>
        /// Gets and Sets code review set or not
        /// </summary>
        public bool IsCodeReviewAdded { get; set; }

    }
}