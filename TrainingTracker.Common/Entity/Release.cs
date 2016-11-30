using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace TrainingTracker.Common.Entity
{
    public class Release
    {
        /// <summary>
        /// Get and set ReleaseId
        /// </summary>
        public int ReleaseId { get; set; }

        /// <summary>
        /// Get and set Major
        /// </summary>
         public int Major { get; set; }

        /// <summary>
        /// Get and set Minor
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        /// Get and set Patch
        /// </summary>
        public int Patch { get; set; }

        /// <summary>
        /// Get and set Title
        /// </summary>
        public string ReleaseTitle { get; set; }

        /// <summary>
        /// Get and set Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get and set ReleaseDate
        /// </summary>
        public DateTime? ReleaseDate { get; set; }

        /// <summary>
        /// Get and set IsPublished
        /// </summary>
        public bool IsPublished { get; set; }

        /// <summary>
        /// Get and set SortOrder
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets and Sets AddedBy
        /// </summary>
        public User AddedBy { get; set; }

        /// <summary>
        /// Gets and Sets AddedBy
        /// </summary>
        public bool IsNew { get; set; }
    }
}
