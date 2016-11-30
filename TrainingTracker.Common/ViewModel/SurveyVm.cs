
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Common.ViewModel
{
    /// <summary>
    /// class to return survey Vmentities
    /// </summary>
    public  class SurveyVm
    {
        /// <summary>
        /// gets and sets Survey instance
        /// </summary>
         public Survey Survey { get; set; }

        /// <summary>
        /// gets and sets Wether code reviewd or Not
        /// </summary>
        public bool IsCodeReviewedForTrainee { get; set; } 
    }
}
