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
    
    public partial class CodeReviewPoint
    {
        public int CodeReviewPointId { get; set; }
        public int CodeReviewTagId { get; set; }
        public string ReviewPoint { get; set; }
        public int CodeReviewPointType { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public System.DateTime ModifiedOn { get; set; }
    
        public virtual CodeReviewPointType CodeReviewPointType1 { get; set; }
        public virtual CodeReviewTag CodeReviewTag { get; set; }
    }
}