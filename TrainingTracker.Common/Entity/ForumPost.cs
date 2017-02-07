using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    public class ForumPost
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AddedBy { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public User AddedByUser { get; set; }
        public ForumCategory Category { get; set; }
        public ForumStatus Status { get; set; }
        public List<ForumThread> Threads { get; set; }
    }
}