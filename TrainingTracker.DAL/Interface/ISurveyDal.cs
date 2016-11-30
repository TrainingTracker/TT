
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Interface
{
    /// <summary>
    /// interface for Survey data access
    /// </summary>
    public interface ISurveyDal
    {
        /// <summary>
        /// interface method for Weekly survey
        /// </summary>
        /// <param name="teamId">team id</param>
        /// <returns>instance of survey</returns>
        Common.Entity.Survey GetWeeklySurveySetForTeam(int teamId);

        /// <summary>
        /// Interface method for Saving response
        /// </summary>
        /// <param name="response">instance of response</param>
        /// <param name="survey">instance of survey</param>
        /// <returns>saved feedback id</returns>
        int SaveWeeklySurveyResponseForTrainee( Common.Entity.SurveyResponse response , Common.Entity.Survey survey );
    }
}
