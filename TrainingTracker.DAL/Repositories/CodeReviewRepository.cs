using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class CodeReviewRepository : Repository<CodeReviewMetaData>, ICodeReviewRepository
    {
        private readonly TrainingTrackerEntities _context;

        public CodeReviewRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public CodeReviewMetaData GetCodeReviewWithAllData(int codeReviewMetaDataId)
        {
            // intentional use of First, let the system break in case of multiple data set
            var codeReview = _context.CodeReviewMetaDatas.Include(x => x.CodeReviewTags)
                                     .Include(x => x.CodeReviewTags.Select(y => y.Skill))
                                     .Include(x => x.CodeReviewTags
                                                    .Select(y => y.CodeReviewPoints))
                                     .Where(cr => !(cr.IsDiscarded ?? false))
                                     .First(x => x.CodeReviewMetaDataId == codeReviewMetaDataId);

            codeReview.CodeReviewTags = codeReview.CodeReviewTags.OrderBy(t => t.SkillId).ToList();
            return codeReview;
        }

        public CodeReviewMetaData GetSavedCodeReviewForTrainee(int traineeId, int trainorId)
        {
            var codeReview = _context.CodeReviewMetaDatas.Include(x => x.CodeReviewTags)
                                     .Include(x => x.CodeReviewTags
                                                    .Select(y => y.CodeReviewPoints))
                                     .AsEnumerable()
                                     .Select(cr =>
                                             {
                                                 cr.CodeReviewTags = cr.CodeReviewTags.OrderBy(t => t.SkillId).ToList();
                                                 return cr;
                                             })
                                     .FirstOrDefault(x => x.AddedBy == trainorId
                                                          && x.AddedFor == traineeId
                                                          && !(x.IsDiscarded ?? false)
                                                          && !x.FeedbackId.HasValue);

            if (codeReview != null)
                codeReview.CodeReviewTags = codeReview.CodeReviewTags.OrderBy(t => t.SkillId).ToList();

            return codeReview;
        }


        public IEnumerable<CodeReviewMetaData> GetPrevCodeReviewForTrainee(int traineeId, int count)
        {
            return _context.CodeReviewMetaDatas
                           .Include(cr => cr.CodeReviewTags)
                           .Include(cr => cr.CodeReviewTags.Select(t => t.Skill))
                           .Include(cr => cr.CodeReviewTags.Select(t => t.CodeReviewPoints))
                           .Include(cr => cr.Feedback)
                           .Include(cr => cr.Feedback.User)
                           .Where(cr => cr.AddedFor == traineeId
                                        && !(cr.IsDiscarded ?? false)
                                        && cr.FeedbackId.HasValue)
                           .OrderByDescending(cr => cr.CreatedOn)
                           .Take(count);
        }

        public IEnumerable<Skill> GetCommonlyUsedTags(int traineeId, int reviewCount)
        {
            return GetPrevCodeReviewForTrainee(traineeId, reviewCount)
                .SelectMany(cr => cr.CodeReviewTags.Where(t => !t.IsDeleted && t.Skill != null).Select(t => t.Skill))
                .Distinct();
        }

        public IEnumerable<CrRatingCalcConfig> GetCrRatingCalcConfig(int traineeId)
        {
            return _context.Users
                           .First(u => u.UserId == traineeId)
                           .Team
                           .CrRatingCalcConfigs
                           .AsQueryable()
						   .Include("CrRatingCalcRangeConfigs,CrRatingCalcWeightConfigs")
                           .AsEnumerable()
                           .Select(c=>new CrRatingCalcConfig
                                      {
                                          Id = c.Id,
                                          CrRatingCalcWeightConfigs = c.CrRatingCalcWeightConfigs.OrderByDescending(w=>w.Weight).ToList(),
                                          CrRatingCalcRangeConfigs = c.CrRatingCalcRangeConfigs.OrderBy(w=>w.RangeMin).ToList()
                                      });
        }

        public void UpdateCrRatingCalcConfig(CrRatingCalcConfig config)
        {
            _context.CrRatingCalcConfigs.AddOrUpdate(config);
        }
    }
}