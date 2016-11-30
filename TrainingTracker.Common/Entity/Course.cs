using System;
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int AddedBy { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }

        public List<CourseSubtopic> CourseSubtopics { get; set; }
    }
}
