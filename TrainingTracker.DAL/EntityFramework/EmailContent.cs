//------------------------------------------------------------------------------
// <auto-generated>
<<<<<<< HEAD
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
=======
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
>>>>>>> bcd07e70a879ecc5e032dd8d93dbcadf1cf27df8
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingTracker.DAL.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailContent
    {
<<<<<<< HEAD
=======
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
>>>>>>> bcd07e70a879ecc5e032dd8d93dbcadf1cf27df8
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
    
<<<<<<< HEAD
=======
        public virtual TaskSchedulerJob TaskSchedulerJob { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
>>>>>>> bcd07e70a879ecc5e032dd8d93dbcadf1cf27df8
        public virtual ICollection<EmailRecipient> EmailRecipients { get; set; }
    }
}
