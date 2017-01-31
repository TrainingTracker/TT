using System;
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity class for Courses
    /// </summary>
    public class SubscribedTrainee
    {
        public int Id { get; set; }
        public int SubscribedByUserId { get; set; }
        public int SubscribedForUserId { get; set; }
        public bool IsDeleted { get; set; }

    }
}
