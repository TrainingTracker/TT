using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    public class ForumCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public List<ForumPost> Posts { get; set; }
    }
}