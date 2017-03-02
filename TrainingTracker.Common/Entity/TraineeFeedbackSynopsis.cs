
namespace TrainingTracker.Common.Entity
{
    public class TraineeFeedbackSynopsis
    {
        public FeedbackCount WeeklyFeedbackCount { get; set; }
        public FeedbackCount CodeReviewFeedbackCount { get; set; }
        public int RecentWeeklyRating { get; set; }
        public int RecentCodeReviewRating { get; set; }
    }
}
