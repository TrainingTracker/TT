using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity Class for Survey
    /// </summary>
    public class Survey
    {
        /// <summary>
        /// Gets and Sets survey Id
        /// </summary>
        public int SurveyId { get; set; }

        /// <summary>
        /// Gets and Sets Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// gets and sets Servey Status
        /// </summary>
        public bool IsOpen { get; set; }

        /// <summary>
        /// Date Time Survey was Created
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets And Sets Survey's Sections
        /// </summary>
        public List<SurveySection> SurveySubSections { get; set; }
    }
}
