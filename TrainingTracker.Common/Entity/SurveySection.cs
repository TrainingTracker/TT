using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity Class for Survey Section
    /// </summary>
    public class SurveySection
    {
        /// <summary>
        /// Gets and Sets  Section Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets and Sets Section's Header
        /// </summary>
        public string Header { get; set; }

        /// <summary>
        /// Gets and Sets Section's Sort Order
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets and Sets Section's Questions
        /// </summary>
        public List<SurveyQuestion> Questions { get; set; } 

    }
}
