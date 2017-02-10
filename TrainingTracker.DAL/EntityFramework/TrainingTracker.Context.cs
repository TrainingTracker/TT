﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingTracker.DAL.EntityFramework
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class TrainingTrackerEntities : DbContext
    {
        public TrainingTrackerEntities()
            : base("name=TrainingTrackerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Assignment> Assignments { get; set; }
        public virtual DbSet<AssignmentFeedbackMapping> AssignmentFeedbackMappings { get; set; }
        public virtual DbSet<AssignmentSubtopicMap> AssignmentSubtopicMaps { get; set; }
        public virtual DbSet<AssignmentUserMap> AssignmentUserMaps { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        public virtual DbSet<CourseSubtopic> CourseSubtopics { get; set; }
        public virtual DbSet<CourseSubtopicDiscussion> CourseSubtopicDiscussions { get; set; }
        public virtual DbSet<CourseUserMapping> CourseUserMappings { get; set; }
        public virtual DbSet<EmailAlertSubscription> EmailAlertSubscriptions { get; set; }
        public virtual DbSet<EmailContent> EmailContents { get; set; }
        public virtual DbSet<EmailRecipient> EmailRecipients { get; set; }
        public virtual DbSet<EmailRecipientType> EmailRecipientTypes { get; set; }
        public virtual DbSet<Feedback> Feedbacks { get; set; }
        public virtual DbSet<FeedbackThread> FeedbackThreads { get; set; }
        public virtual DbSet<FeedbackType> FeedbackTypes { get; set; }
        public virtual DbSet<ForumDiscussionCategory> ForumDiscussionCategories { get; set; }
        public virtual DbSet<ForumDiscussionPost> ForumDiscussionPosts { get; set; }
        public virtual DbSet<ForumDiscussionStatu> ForumDiscussionStatus { get; set; }
        public virtual DbSet<ForumDiscussionThread> ForumDiscussionThreads { get; set; }
        public virtual DbSet<ForumUserHelpCategory> ForumUserHelpCategories { get; set; }
        public virtual DbSet<ForumUserHelpPost> ForumUserHelpPosts { get; set; }
        public virtual DbSet<ForumUserHelpStatu> ForumUserHelpStatus { get; set; }
        public virtual DbSet<ForumUserHelpThread> ForumUserHelpThreads { get; set; }
        public virtual DbSet<LearningMap> LearningMaps { get; set; }
        public virtual DbSet<LearningMapCourseMapping> LearningMapCourseMappings { get; set; }
        public virtual DbSet<LearningMapUserMapping> LearningMapUserMappings { get; set; }
        public virtual DbSet<LearningSource> LearningSources { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationType> NotificationTypes { get; set; }
        public virtual DbSet<Plan> Plans { get; set; }
        public virtual DbSet<PlanSkillMapping> PlanSkillMappings { get; set; }
        public virtual DbSet<Project> Projects { get; set; }
        public virtual DbSet<ProjectPlanMapping> ProjectPlanMappings { get; set; }
        public virtual DbSet<QuestionLevelMapping> QuestionLevelMappings { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<Release> Releases { get; set; }
        public virtual DbSet<Session> Sessions { get; set; }
        public virtual DbSet<Skill> Skills { get; set; }
        public virtual DbSet<SubtopicContent> SubtopicContents { get; set; }
        public virtual DbSet<SubtopicContentUserMap> SubtopicContentUserMaps { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<SurveyAnswer> SurveyAnswers { get; set; }
        public virtual DbSet<SurveyCompletedMetaData> SurveyCompletedMetaDatas { get; set; }
        public virtual DbSet<SurveyQuestion> SurveyQuestions { get; set; }
        public virtual DbSet<SurveyQuestionResponseType> SurveyQuestionResponseTypes { get; set; }
        public virtual DbSet<SurveyResponse> SurveyResponses { get; set; }
        public virtual DbSet<SurveySection> SurveySections { get; set; }
        public virtual DbSet<TaskSchedulerJob> TaskSchedulerJobs { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserNotificationMapping> UserNotificationMappings { get; set; }
        public virtual DbSet<UserProjectMapping> UserProjectMappings { get; set; }
        public virtual DbSet<UserSessionMapping> UserSessionMappings { get; set; }
        public virtual DbSet<UserSkillMapping> UserSkillMappings { get; set; }
        public virtual DbSet<WeeklyFeedbackSurveyMapping> WeeklyFeedbackSurveyMappings { get; set; }
    }
}
