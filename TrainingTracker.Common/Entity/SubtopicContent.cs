using System;
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    public class SubtopicContent
    {
        public int Id { get; set; }
        public int CourseSubtopicId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public int AddedBy { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
