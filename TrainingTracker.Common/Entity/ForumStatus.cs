using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    public class ForumStatus
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public List<ForumPost> Posts { get; set; }
    }
}