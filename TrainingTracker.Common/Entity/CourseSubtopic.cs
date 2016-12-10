using System;
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    public class CourseSubtopic
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AddedBy { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }


        // ToDo: Add CourseSubtopicDiscussion and SubtopicContent classes in Entity
        //public virtual ICollection<CourseSubtopicDiscussion> CourseSubtopicDiscussions { get; set; }
        //public virtual ICollection<SubtopicContent> SubtopicContents { get; set; }
    }
}
