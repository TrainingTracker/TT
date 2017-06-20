using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class CodeReviewRepository : Repository<EntityFramework.CodeReviewMetaData>, ICodeReviewRepository
    {
        private readonly TrainingTrackerEntities _context;

        public CodeReviewRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

       public DAL.EntityFramework.CodeReviewMetaData GetCodeReviewWithAllData(int codeReviewMetaDataId)
       {
          // intentional use of First, let the system break in case of multiple data set
           return _context.CodeReviewMetaDatas.Include(x => x.CodeReviewTags)
                                                .Include(x=>x.CodeReviewTags.Select(y=>y.Skill))
                                              .Include(x => x.CodeReviewTags
                                                             .Select(y=>y.CodeReviewPoints))
                                              .First(x => x.CodeReviewMetaDataId == codeReviewMetaDataId);
       }

       public DAL.EntityFramework.CodeReviewMetaData GetSavedCodeReviewForTrainee(int traineeId,int trainorId)
       {
          
           return _context.CodeReviewMetaDatas.Include(x => x.CodeReviewTags)
                                              .Include(x => x.CodeReviewTags
                                                             .Select(y => y.CodeReviewPoints))
                                              .FirstOrDefault(x => x.AddedBy == trainorId 
                                                          && x.AddedFor == traineeId 
                                                          && x.IsDiscarded == false 
                                                          && !x.FeedbackId.HasValue);
       }
    }
}
