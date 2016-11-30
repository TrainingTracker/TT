
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    ///  Wrapper class for feedback Plot Data
    /// </summary>
   public class FeedbackPlot
    {
        /// <summary>
        /// Gets and sets List of Assigments feedback
        /// </summary>
        public List<Feedback> AssignmentFeedbacks { get; set; }

        /// <summary>
        /// Gets nad Sets List of Code Review Feedbacks
        /// </summary>
        public List<Feedback> CodeReviewFeedbacks { get; set; }

        /// <summary>
        /// Gets And sets List Of Weekly Feedbacks
        /// </summary>
        public List<Feedback> WeeklyFeedbacks { get; set; }
    }
}
