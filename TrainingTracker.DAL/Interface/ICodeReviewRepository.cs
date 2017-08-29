using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Interface
{
    public interface ICodeReviewRepository : IRepository<CodeReviewMetaData>
    {
        CodeReviewMetaData GetCodeReviewWithAllData(int codeReviewMetaDataId);

        CodeReviewMetaData GetSavedCodeReviewForTrainee(int traineeId, int trainorId);

        IEnumerable<CodeReviewMetaData> GetPrevCodeReviewForTrainee(int traineeId, int count);

        IEnumerable<Skill> GetCommonlyUsedTags(int traineeId, int reviewCount);

        IEnumerable<CrRatingCalcConfig> GetCrRatingCalcConfig(int traineeId);

        IEnumerable<CrRatingCalcConfig> GetCrRatingCalcConfigForTeam(int teamId);
    }
}