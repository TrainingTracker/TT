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
    
    public partial class Project
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public string ProjectType { get; set; }
        public string ClientName { get; set; }
        public Nullable<System.DateTime> AddedOn { get; set; }
    }
}
