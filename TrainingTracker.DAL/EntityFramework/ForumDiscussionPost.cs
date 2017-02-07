//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingTracker.DAL.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class ForumDiscussionPost
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ForumDiscussionPost()
        {
            this.ForumDiscussionThreads = new HashSet<ForumDiscussionThread>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AddedBy { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual ForumDiscussionCategory ForumDiscussionCategory { get; set; }
        public virtual ForumDiscussionStatu ForumDiscussionStatu { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ForumDiscussionThread> ForumDiscussionThreads { get; set; }
    }
}