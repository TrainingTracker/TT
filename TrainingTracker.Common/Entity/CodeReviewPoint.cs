using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTracker.Common.Entity
{
    public class CodeReviewPoint
    {
        public int PointId { get; set; }

        public int? CodeReviewTagId { get; set; }

        public int CodeReviewMetadataId { get; set; }

        public int Rating { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public bool IsDeleted { get; set; }
    }
}
