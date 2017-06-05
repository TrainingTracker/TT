using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTracker.Common.Entity
{
    public class CodeReview
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public User AddedBy { get; set; }

        public User AddedFor { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime CreatedOn { get; set; }

        public List<CodeReviewTag> Tags { get; set; }

    }

    public class CodeReviewTag
    {
        public int CodeReviewTagId { get; set; }

        public Skill Skill { get; set; }

        public List<CodeReviewPoint> ReviewPoints { get; set; }
    }

    public class CodeReviewPoint
    {
        public int PointId { get; set; }

        public int Rating { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }
}
