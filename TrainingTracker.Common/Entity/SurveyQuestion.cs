using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity class for Survey's Question
    /// </summary>
    public class SurveyQuestion
    {
        /// <summary>
        /// Gets and Sets Question's Id
        /// </summary>
        public int SurveyQuestionId { get; set; }

        /// <summary>
        /// Gets and Sets Question's Text
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// Gets And Sets Question's Hint
        /// </summary>
        public string HelpText { get; set; }

        /// <summary>
        /// Gets and Sets Questions is Mandatory to Attend in Survey
        /// </summary>
        public bool IsMandatory { get; set; }

        /// <summary>
        /// Gets And Sets Questions should have note added while attending Or Not
        /// </summary>
        public bool AdditionalNoteRequired { get; set; }

        /// <summary>
        /// Gets And Sets Response Type Id
        /// </summary>
        public int ResponseTypeId { get; set; }

        /// <summary>
        /// Gets and Sets Sort Order Of Question
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets And Sets Question's answer
        /// </summary>
        public List<SurveyAnswer> Answers { get; set; }
    }
}
