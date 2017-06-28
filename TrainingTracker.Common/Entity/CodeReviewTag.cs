using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTracker.Common.Entity
{
  public  class CodeReviewTag
    {
        public int CodeReviewTagId { get; set; }

        public Skill Skill { get; set; }

        public List<CodeReviewPoint> ReviewPoints { get; set; }
    }
}
