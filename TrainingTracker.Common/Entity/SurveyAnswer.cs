using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity Class for Survey's Answer
    /// </summary>
    public class SurveyAnswer
    {
        /// <summary>
        /// Gets and Sets Answer's Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets and Sets option Text
        /// </summary>
        public string OptionText { get; set; }

        /// <summary>
        /// Gets And Sets option's Sort Order
        /// </summary>
        public int SortOrder { get; set; }
    }
}
