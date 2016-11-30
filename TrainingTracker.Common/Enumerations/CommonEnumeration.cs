namespace TrainingTracker.Common.Enumeration
{
    /// <summary>
    /// Enumeration type TypeOfNotification.
    /// </summary>
    public enum NotificationType
    {
        /// <summary>
        /// set value of ReleaseNotification to 1.
        /// </summary>
        NewReleaseNotification = 1,

        /// <summary>
        ///  set value of FeedbackNotification to 2.
        /// </summary>
        CommentFeedbackNotification = 2,

        /// <summary>
        /// set value of Feature Notification to 3.
        /// </summary>
        NewFeatureRequestNotification = 3 ,

        /// <summary>
        /// set value of ReleaseNotification to 4.
        /// </summary>
        FeatureModifiedNotification = 4,

        /// <summary>
        ///  set value of WeeklyNotification to 5.
        /// </summary>
        WeeklyFeedbackNotification = 5 ,

        /// <summary>
        ///  set value of FeedbackNotification to 6.
        /// </summary>
        AssignmentFeedbackNotification = 6 ,

        /// <summary>
        ///  set value of FeedbackNotification to 7.
        /// </summary>
        CodeReviewFeedbackNotification = 7 ,

        /// <summary>
        ///  set value of FeedbackNotification to 8.
        /// </summary>
        SkillFeedbackNotification = 8 ,

        /// <summary>
        /// Enum for New Session Notification
        /// </summary>
        NewSessionNotification = 9,

        /// <summary>
        /// Enum for Session Updated Notification
        /// </summary>
        SessionUpdatedNotification = 10,

        /// <summary>
        /// Enum For New note to feedback
        /// </summary>
        NewNoteToFeedback=11
    }


    /// <summary>
    /// enumeration for feedback type
    /// </summary>
    public enum FeedbackType
    {
        /// <summary>
        /// Comment Feedback
        /// </summary>
        Comment =1,

        /// <summary>
        /// Skill Feedback
        /// </summary>
        Skill=2,

        /// <summary>
        /// Assignment Feedback
        /// </summary>
        Assignment=3,

        /// <summary>
        /// Code Review Feedback
        /// </summary>
        CodeReview=4,

        /// <summary>
        /// Weekly Feedback
        /// </summary>
        Weekly=5
    }

    /// <summary>
    /// Enumeration type for survey's response type
    /// </summary>
    public enum ResponseType
    {
        /// <summary>
        /// Response Type text
        /// </summary>
        Text = 1,

        /// <summary>
        /// Response Type Dropdown
        /// </summary>
        Select = 2,

        /// <summary>
        /// Response Type Radio button
        /// </summary>
        Radio = 3,

        /// <summary>
        /// Response Type Checkbox
        /// </summary>
        Checkbox =4

    }

    /// <summary>
    /// Enumeration typefor survey's
    /// </summary>
    public enum FeedbackRating
    {
        /// <summary>
        /// Enum Type for Slow rating
        /// </summary>
        Slow = 1,

        /// <summary>
        /// Enum Type for Average rating
        /// </summary>
        Average = 2,

        /// <summary>
        /// Enum Type Fast rating 
        /// </summary>
        Fast = 3,

        /// <summary>
        /// Enum For Rajnikant Mode
        /// </summary>
        Exceptional = 4
    }
}
