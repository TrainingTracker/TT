//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingTracker.TaskScheduler.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailRecipientType
    {
        public EmailRecipientType()
        {
            this.EmailRecipients = new HashSet<EmailRecipient>();
        }
    
        public int Id { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<EmailRecipient> EmailRecipients { get; set; }
    }
}