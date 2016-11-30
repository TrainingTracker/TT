using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TrainingTracker.Common.Enumeration;

namespace TrainingTracker.Common.Entity
{
    public class Notification
    {
        /// <summary>
        /// Get and set NotificationId
        /// </summary>
        [Required]
        public int NotificationId { get; set; }

        /// <summary>
        /// Get and set Description
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// Get and set Link
        /// </summary>
        [Required]
        public string Link { get; set; }

        /// <summary>
        /// Get and set Type
        /// </summary>
        [Required]
        public NotificationType TypeOfNotification { get; set; }

        /// <summary>
        /// Get and set AddedBy
        /// </summary>
        [Required]
        public int AddedBy { get; set; }

        /// <summary>
        /// Get and set AddedOn
        /// </summary>
        [Required]
        public DateTime AddedOn { get; set; }

        /// <summary>
        /// Get and set AddedTo
        /// </summary>
        public string[] AddedTo { get; set; }

        /// <summary>
        /// get and set Title
        /// </summary>
        [Required]
        public string Title { get; set; }

        public User UserDetails { get; set; }
    }

  
}
