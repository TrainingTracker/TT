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
    
    public partial class ForumUserHelpCategory
    {
        public ForumUserHelpCategory()
        {
            this.ForumUserHelpPosts = new HashSet<ForumUserHelpPost>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual ICollection<ForumUserHelpPost> ForumUserHelpPosts { get; set; }
    }
}
