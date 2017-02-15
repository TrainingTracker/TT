namespace TrainingTracker.Common.Entity
{
    public class ForumThread
    {
        public int ThreadId { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; }
        public int AddedBy { get; set; }
        public int AddedFor { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public User AddedByUser { get; set; }
    }
}