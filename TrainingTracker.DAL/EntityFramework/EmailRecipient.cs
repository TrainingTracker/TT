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
    
    public partial class EmailRecipient
    {
        public int Id { get; set; }
        public int EmailContentId { get; set; }
        public string EmailAddress { get; set; }
        public int EmailRecipientType { get; set; }
    
        public virtual EmailContent EmailContent { get; set; }
        public virtual EmailRecipientType EmailRecipientType1 { get; set; }
    }
}
