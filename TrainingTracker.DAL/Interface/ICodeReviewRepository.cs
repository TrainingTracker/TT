using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Interface
{
    public interface ICodeReviewRepository : IRepository<EntityFramework.CodeReviewMetaData>
    {

        DAL.EntityFramework.CodeReviewMetaData GetCodeReviewWithAllData(int codeReviewMetaDataId);

        DAL.EntityFramework.CodeReviewMetaData GetSavedCodeReviewForTrainee(int traineeId, int trainorId);

        IEnumerable<DAL.EntityFramework.CodeReviewMetaData> GetPrevCodeReviewForTrainee(int traineeId,int count);

        IEnumerable<Skill> GetCommonlyUsedTags(int traineeId, int reviewCount);
    }
}
