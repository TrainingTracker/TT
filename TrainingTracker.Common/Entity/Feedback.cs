using System;
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    public class Feedback
    {
        public int FeedbackId { get; set; }
        public string FeedbackText { get; set; }
        public string Title { get; set; }
        public int Rating { get; set; }
        public FeedbackType FeedbackType { get; set; }
        public Project Project { get; set; }
        public Skill Skill { get; set; }
        public User AddedBy { get; set; }
        public User AddedFor { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string WeekForFeedbackPresent { get; set; }

        public List<Threads> Threads { get; set; } 
        public int ThreadCount { get; set; }
    }
}