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
        /// Gets and Sets All skills
        /// </summary>
        public List<Skill> AllSkills { get; set; }

        /// <summary>
        /// Gets and Sets Recent Feedbacks 
        /// </summary>
        public List<Feedback> RecentCrFeedback { get; set;}
        
        /// <summary>
        /// Gets and sets List of weekly feedbacks
        /// </summary>
        public List<Feedback> RecentWeeklyFeedback { get; set; }

        /// <summary>
        /// Gets and Sets All Trainer
        /// </summary>
        public List<User> AllTrainer { get; set; }

        /// <summary>
        /// Gets and Sets Trainor Synopsis
        /// </summary>
        public TrainerFeedbackSynopsis TrainorSynopsis { get; set; }
    }
}