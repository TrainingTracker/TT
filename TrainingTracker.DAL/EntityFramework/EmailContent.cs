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
    
    public partial class EmailContent
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EmailContent()
        {
            this.EmailRecipients = new HashSet<EmailRecipient>();
        }
    
        public int Id { get; set; }
        public int TaskSchedulerJobId { get; set; }
        public string SubjectText { get; set; }
        public string BodyText { get; set; }
        public bool IsRichBody { get; set; }
        public string FromAddress { get; set; }
        public bool IsSent { get; set; }
        public Nullable<System.DateTime> SentTimeStamp { get; set; }
        public sbyte Attempts { get; set; }
    
        public virtual TaskSchedulerJob TaskSchedulerJob { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EmailRecipient> EmailRecipients { get; set; }
    }
}