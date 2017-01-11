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
    
    public partial class Assignment
    {
        public Assignment()
        {
            this.AssignmentSubtopicMaps = new HashSet<AssignmentSubtopicMap>();
            this.AssignmentUserMaps = new HashSet<AssignmentUserMap>();
            this.AssignmentFeedbackMappings = new HashSet<AssignmentFeedbackMapping>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int AddedBy { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public string AssignmentAsset { get; set; }
    
        public virtual User User { get; set; }
        public virtual ICollection<AssignmentSubtopicMap> AssignmentSubtopicMaps { get; set; }
        public virtual ICollection<AssignmentUserMap> AssignmentUserMaps { get; set; }
        public virtual ICollection<AssignmentFeedbackMapping> AssignmentFeedbackMappings { get; set; }
    }
}
