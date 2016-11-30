//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingTracker.DAL.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class CourseSubtopic
    {
        public CourseSubtopic()
        {
            this.CourseSubtopicDiscussions = new HashSet<CourseSubtopicDiscussion>();
            this.SubtopicContents = new HashSet<SubtopicContent>();
        }
    
        public int Id { get; set; }
        public int CourseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AddedBy { get; set; }
        public int SortOrder { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual Course Course { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<CourseSubtopicDiscussion> CourseSubtopicDiscussions { get; set; }
        public virtual ICollection<SubtopicContent> SubtopicContents { get; set; }
    }
}
