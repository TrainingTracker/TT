using System;
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity class for Email Alert Subscription
    /// </summary>
    public class EmailAlertSubscription
    {
        public int Id { get; set; }
        public int SubscribedByUserId { get; set; }
        public int SubscribedForUserId { get; set; }
        public bool IsSubscribedForComment { get; set; }
        public bool IsSubscribedForWeeklyFeedback { get; set; }
        public bool IsSubscribedForCodeReview { get; set; }
        public bool IsSubscibedForAssignment { get; set; }
        public bool IsSubscibedForSkill { get; set; }
        public bool IsDeleted { get; set; }

        public bool IsModifiedOrAdded { get; set; }
    }
}
