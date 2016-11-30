using System;
using System.Collections.Generic;


namespace TrainingTracker.Common.Entity
{
    public class Assignment
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AddedBy { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }

        public int SubtopicContentId { get; set; }
    }
}
