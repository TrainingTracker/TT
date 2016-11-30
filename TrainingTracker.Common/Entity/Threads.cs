
using System;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity class for threads
    /// </summary>
    public class Threads
    {
        /// <summary>
        /// Gets and Sets Thread Id
        /// </summary>
        public int ThreadId { get; set; }

        /// <summary>
        /// Gets and Sets Comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets and SetsThread Added by
        /// </summary>
        public User AddedBy { get; set; }

        /// <summary>
        /// Gets And Sets Feedback's Id
        /// </summary>
        public int FeedbackId { get; set; }

        /// <summary>
        /// Gets And Sets DateInserted
        /// </summary>
        public DateTime DateInserted { get; set; }
    }
}   
