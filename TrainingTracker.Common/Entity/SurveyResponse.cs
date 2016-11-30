
using System;
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity Class for Weekly Survey Response 
    /// </summary>
    public class SurveyResponse
    {
        /// <summary>
        /// Instance of User added the feedback
        /// </summary>
        public User AddedBy { get; set; }

        /// <summary>
        /// Instance of User for whom feedback was added
        /// </summary>
        public User AddedFor { get; set; }

        /// <summary>
        /// List of response
        /// </summary>
        public List<ResponseMapping> Response { get; set; }

        /// <summary>
        /// Gets and Sets Feedback 
        /// </summary>
        public Feedback Feedback { get; set; }

        /// <summary>
        /// Gets And Sets Code review feedback for the week.
        /// </summary>
        public List<Feedback> CodeReviewForTheWeek { get; set; }

    }

    public class ResponseMapping
    {
        /// <summary>
        /// Gets sets Answer id
        /// </summary>
        public int? AnswerId { get; set; }

        /// <summary>
        /// Gets and sets question text
        /// </summary>
        public string AnswerText { get; set; }

        /// <summary>
        /// Gets and Sets Question Id
        ///  </summary>
        public int QuestionId { get; set; }

        /// <summary>
        /// Gest and Sets Question text
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// Gets and sets additional Notes for the feedback
        /// </summary>
        public string AdditionalNotes { get; set; }

    
    }
}
