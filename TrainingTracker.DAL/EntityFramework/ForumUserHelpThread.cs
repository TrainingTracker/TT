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
    
    public partial class ForumUserHelpThread
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Description { get; set; }
        public int AddedBy { get; set; }
        public System.DateTime CreatedOn { get; set; }
    
        public virtual ForumUserHelpPost ForumUserHelpPost { get; set; }
        public virtual User User { get; set; }
    }
}