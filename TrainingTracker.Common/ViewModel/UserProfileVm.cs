using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Common.ViewModel
{
    /// <summary>
    /// class for User Profile Vm
    /// </summary>
    public class UserProfileVm
    {
        /// <summary>
        /// Gets and Sets instance of current User
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Gets and Sets List of Session
        /// </summary>
        public List<Session> Sessions { get; set; }

        /// <summary>
        /// Gets And Sets instances of Project
        /// </summary>
        public List<Project> Projects { get; set; }

        /// <summary>
        /// Gets and sets instnaces of skills
        /// </summary>
        public List<Skill> Skills { get; set; }

        /// <summary>
        /// Gets and Sets List of feedback
        /// </summary>
        public List<Feedback> Feedbacks { get; set; }

        /// <summary>
        /// Gets and sets List Of Feedback Type
        /// </summary>
        public List<FeedbackType> FeedbackTypes { get; set; }

        /// <summary>
        /// Gets and Sets Trainee Synopsis
        /// </summary>
        public TraineeFeedbackSynopsis TraineeSynopsis { get; set; }

        /// <summary>
        /// Gets and Sets Trainor Synopsis
        /// </summary>
        public TrainerFeedbackSynopsis TrainorSynopsis { get; set; }

        /// <summary>
        /// Gets And Sets All assigned Courses
        /// </summary>
        public List<CourseTrackerDetails> AllAssignedCourses { get; set; }

    //    public string SavedCodeReviewData { get; set; }

        public CodeReview SavedCodeReview { get; set; }

        public List<CodeReviewTag> CommonTags { get; set; }
    }
}