﻿namespace TrainingTracker.Common.Enumeration
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
        NewFeatureRequestNotification = 3,

        /// <summary>
        /// set value of ReleaseNotification to 4.
        /// </summary>
        FeatureModifiedNotification = 4,

        /// <summary>
        ///  set value of WeeklyNotification to 5.
        /// </summary>
        WeeklyFeedbackNotification = 5,

        /// <summary>
        ///  set value of FeedbackNotification to 6.
        /// </summary>
        AssignmentFeedbackNotification = 6,

        /// <summary>
        ///  set value of FeedbackNotification to 7.
        /// </summary>
        CodeReviewFeedbackNotification = 7,

        /// <summary>
        ///  set value of FeedbackNotification to 8.
        /// </summary>
        SkillFeedbackNotification = 8,

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
        NewNoteToFeedback = 11,

        /// <summary>
        /// Enum for New course Notification
        /// </summary>
        NewCourseAssigned = 12,

        /// <summary>
        /// Enum For Course Feedback Notification
        /// </summary>
        CourseFeedbackNotification = 13,

        /// <summary>
        /// Enum For New Action to Perform
        /// </summary>
        NewActionToPerform = 14,

        /// <summary>
        /// Enum For New Discussion Post Notification
        /// </summary>
        NewDiscussionPostNotification = 15,

        /// <summary>
        /// Enum For New Discussion Thtread Notification
        /// </summary>
        NewDiscussionThreadNotification = 16,

        /// <summary>
        /// Enum For New Random Reviews
        /// </summary>
        RandomReviewFeedbackNotification = 17,
        
        /// <summary>
        /// Enum For New Users
        /// </summary>
        NewUserNotification = 18,
        
        /// <summary>
        /// Enum For Activated Users
        /// </summary>
        UserActivatedNotification = 19
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
        Weekly=5 ,

        /// <summary>
        /// Course Feedback
        /// </summary>
        Course = 6,

        /// <summary>
        /// Random Review
        /// </summary>
        RandomReview = 7
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

    /// <summary>
    /// Common Enumerations used In Scheduler
    ///</summary>
    public enum EmailRecipientType
    {
        /// <summary>
        /// RecipientType for To Recipient
        /// </summary>
        To = 1,

        /// <summary>
        /// RecipientType for CC Recipient
        /// </summary>
        CarbonCopy = 2,

        /// <summary>
        /// RecipientType for BCC Recipient
        /// </summary>
        BlindCarbonCopy = 3

    }

    /// <summary>
    /// Common Enumeration for Help Categories
    /// </summary>
    public enum ForumUserHelpCategories
    {
        // Help category Bug
        Bug = 1,

        // help category Idea
        Idea = 2,

        //Help category Help
        Help = 3,
    }

    public enum SessionType
    {
        All = 0,

        // Enumeration for Sessions to be presented
        ToBePresented = 1 ,

        // Enumeration for session already Presented
        AlreadyPresented=2

    }

    public enum ReleaseType
    {
        Patch = 1 ,
        Minor = 2 ,
        Major = 3 
    }

    public enum CodeReviewRating
    {
        Exceptional = 1,
        Good = 2,
        Corrected = 3,
        Poor = 4,
        Critical = 5,
        Suggestion= 6
    }
}
