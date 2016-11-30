using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TrainingTracker.Common.Entity
{
    class UserNotification
    {
        /// <summary>
        /// Get and set UserId
        /// </summary>
        [Required]
        public int UserId { get; set; }

        /// <summary>
        /// Get and set NotificationId
        /// </summary>
        [Required]
        public int NotificationId { get; set; }

        /// <summary>
        /// Get and set Seen
        /// </summary>
        [Required]
        public bool Seen { get; set; }
    }
}
