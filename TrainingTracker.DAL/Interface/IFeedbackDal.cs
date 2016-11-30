using System;
using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    /// <summary>
    /// Interface for Feedback dal
    /// </summary>
    public interface IFeedbackDal
    {
        /// <summary>
        /// Interface method to add feddback
        /// </summary>
        /// <param name="feedbackData">instance of new feedback data</param>
        /// <returns>id of added feedback</returns>
        int AddFeedback( Feedback feedbackData );

        List<Feedback> GetUserFeedback(int userId, int count, int? feedbackId = null, DateTime? startAddedOn = null, DateTime? endAddedOn = null);

        /// <summary>
        /// Interface method to get feedback's threads
        /// </summary>
        /// <param name="feedbackId">feedback Id</param>
        /// <returns>List Of Threads</returns>
        List<Threads> GetFeedbackThreads( int feedbackId );

        /// <summary>
        /// Interface method to get feedback with threads
        /// </summary>
        /// <param name="feedbackId">feedback Id</param>
        /// <returns>Instance of Feedback</returns>
        Feedback GetFeedbackWithThreads( int feedbackId );

        /// <summary>
        /// Interface signature Add New Thread to Feedback
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        bool AddNewThread(Threads thread);

        /// <summary>
        /// Get Trainee User Id by Feedback Id
        /// </summary>
        /// <param name="feedbackId">feedbackId</param>
        /// <returns>User Id</returns>
        int GetTraineebyFeedbackId(int feedbackId );

        /// <summary>
        /// Get Feedback AddedBy Trainers
        /// </summary>
        /// <param name="userId">Trainer Id</param>
        /// <param name="count">page size</param>
        /// <param name="skip">skip</param>
        /// <param name="feedbackId">any specefic id</param>
        /// <param name="startAddedOn">Date range start</param>
        /// <param name="endAddedOn">Date Range End</param>
        /// <returns>List Of feedback</returns>
        List<Feedback> GetFeedbackAddedByUser( int userId , int? count=5 , int? skip=0, int? feedbackId = null , DateTime? startAddedOn = null , DateTime? endAddedOn = null );

        /// <summary>
        /// interface signature for fetching Trainor synopsis
        /// </summary>
        /// <param name="trainerId">trainer Id</param>
        /// <returns>instances of Trainor synopsis</returns>
        TrainerFeedbackSynopsis GetTrainorFeedbackSynopsis(int trainerId);
    }
}
