//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingTracker.DAL.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class User
    {
        public User()
        {
            this.Assignments = new HashSet<Assignment>();
            this.AssignmentUserMaps = new HashSet<AssignmentUserMap>();
            this.AssignmentUserMaps1 = new HashSet<AssignmentUserMap>();
            this.Courses = new HashSet<Course>();
            this.CourseSubtopics = new HashSet<CourseSubtopic>();
            this.CourseSubtopicDiscussions = new HashSet<CourseSubtopicDiscussion>();
            this.Feedbacks = new HashSet<Feedback>();
            this.Feedbacks1 = new HashSet<Feedback>();
            this.FeedbackThreads = new HashSet<FeedbackThread>();
            this.LearningMaps = new HashSet<LearningMap>();
            this.LearningMapUserMappings = new HashSet<LearningMapUserMapping>();
            this.Notifications = new HashSet<Notification>();
            this.Questions = new HashSet<Question>();
            this.Releases = new HashSet<Release>();
            this.Sessions = new HashSet<Session>();
            this.SubtopicContents = new HashSet<SubtopicContent>();
            this.SubtopicContentUserMaps = new HashSet<SubtopicContentUserMap>();
            this.SurveyCompletedMetaDatas = new HashSet<SurveyCompletedMetaData>();
            this.Teams = new HashSet<Team>();
            this.UserNotificationMappings = new HashSet<UserNotificationMapping>();
            this.UserSessionMappings = new HashSet<UserSessionMapping>();
            this.UserSessionMappings1 = new HashSet<UserSessionMapping>();
        }
    
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Designation { get; set; }
        public string ProfilePictureName { get; set; }
        public Nullable<bool> IsFemale { get; set; }
        public Nullable<bool> IsAdministrator { get; set; }
        public Nullable<bool> IsTrainer { get; set; }
        public Nullable<bool> IsTrainee { get; set; }
        public Nullable<bool> IsManager { get; set; }
        public Nullable<System.DateTime> DateAddedToSystem { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<int> TeamId { get; set; }
    
        public virtual ICollection<Assignment> Assignments { get; set; }
        public virtual ICollection<AssignmentUserMap> AssignmentUserMaps { get; set; }
        public virtual ICollection<AssignmentUserMap> AssignmentUserMaps1 { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<CourseSubtopic> CourseSubtopics { get; set; }
        public virtual ICollection<CourseSubtopicDiscussion> CourseSubtopicDiscussions { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Feedback> Feedbacks1 { get; set; }
        public virtual ICollection<FeedbackThread> FeedbackThreads { get; set; }
        public virtual ICollection<LearningMap> LearningMaps { get; set; }
        public virtual ICollection<LearningMapUserMapping> LearningMapUserMappings { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Release> Releases { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        public virtual ICollection<SubtopicContent> SubtopicContents { get; set; }
        public virtual ICollection<SubtopicContentUserMap> SubtopicContentUserMaps { get; set; }
        public virtual ICollection<SurveyCompletedMetaData> SurveyCompletedMetaDatas { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<UserNotificationMapping> UserNotificationMappings { get; set; }
        public virtual ICollection<UserSessionMapping> UserSessionMappings { get; set; }
        public virtual ICollection<UserSessionMapping> UserSessionMappings1 { get; set; }
    }
}
