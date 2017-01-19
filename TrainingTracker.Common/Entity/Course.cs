using System;
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity class for Courses
    /// </summary>
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public int AddedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublished { get; set; }
        public int Duration { get; set; }
        public int SortOrder { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool IsStarted { get; set; }
        public DateTime? StartedDateTime { get; set; }
        public DateTime? CompletedDateTime { get; set; }
        public bool LoadAlert { get; set; } 
        public string AuthorName { get; set; }
        public string AuthorMailId { get; set; }
        public List<CourseSubtopic> CourseSubtopics { get; set; }
        public CourseTrackerDetails TrackerDetails { get; set; }
    }
}
