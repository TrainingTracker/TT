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
    
    public partial class SubscribedTrainee
    {
        public int Id { get; set; }
        public int SubscribedByUserId { get; set; }
        public int SubscribedForUserId { get; set; }
        public bool IsDeleted { get; set; }
    
        public virtual User User { get; set; }
        public virtual User User1 { get; set; }
    }
}
