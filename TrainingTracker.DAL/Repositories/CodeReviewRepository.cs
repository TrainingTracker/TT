using System;
using System.Collections.Generic;
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
            return _context.CodeReviewMetaDatas.Include(x => x.CodeReviewTags)
                           .Include(x => x.CodeReviewTags.Select(y => y.Skill))
                           .Include(x => x.CodeReviewTags
                                          .Select(y => y.CodeReviewPoints))
                           .First(x => x.CodeReviewMetaDataId == codeReviewMetaDataId);
        }

        public CodeReviewMetaData GetSavedCodeReviewForTrainee(int traineeId, int trainorId)
        {
            return _context.CodeReviewMetaDatas.Include(x => x.CodeReviewTags)
                           .Include(x => x.CodeReviewTags
                                          .Select(y => y.CodeReviewPoints))
                           .FirstOrDefault(x => x.AddedBy == trainorId
                                                && x.AddedFor == traineeId
                                                && x.IsDiscarded == false
                                                && !x.FeedbackId.HasValue);
        }


        public IEnumerable<CodeReviewMetaData> GetPrevCodeReviewForTrainee(int traineeId, int count)
        {
            return _context.CodeReviewMetaDatas
                           .Include(cr => cr.CodeReviewTags)
                           .Include(cr => cr.CodeReviewTags.Select(t => t.CodeReviewPoints))
                           .Include(cr => cr.Feedback)
                           .Where(cr => cr.AddedFor == traineeId
                                        && cr.IsDiscarded == false
                                        && cr.FeedbackId.HasValue)
                           .OrderByDescending(cr => cr.CreatedOn)
                           .Take(count)
                           .Select(cr => new
                                         {
                                             cr.CodeReviewMetaDataId,
                                             cr.Description,
                                             cr.ProjectName,
                                             cr.IsDiscarded,
                                             cr.CreatedOn,
                                             Feedback = new
                                                        {
                                                            cr.Feedback.FeedbackId,
                                                            cr.Feedback.AddedOn,
                                                            cr.Feedback.FeedbackType,
                                                            cr.Feedback.User,
                                                            cr.Feedback.Rating
                                                        },
                                             CodeReviewTags = cr.CodeReviewTags.Where(t => !t.IsDeleted).OrderBy(t=>t.SkillId)
                                         })
                           .ToList()
                           .Select(cr => new CodeReviewMetaData
                                         {
                                             CodeReviewMetaDataId = cr.CodeReviewMetaDataId,
                                             Description = cr.Description,
                                             ProjectName = cr.ProjectName,
                                             IsDiscarded = cr.IsDiscarded,
                                             CreatedOn = cr.CreatedOn,
                                             Feedback = new Feedback
                                                        {
                                                            FeedbackId = cr.Feedback.FeedbackId,
                                                            AddedOn = cr.Feedback.AddedOn,
                                                            FeedbackText = string.Empty,
                                                            FeedbackType = cr.Feedback.FeedbackType,
                                                            User = cr.Feedback.User,
                                                            Rating = cr.Feedback.Rating
                                                        },
                                             CodeReviewTags = cr.CodeReviewTags.ToList()
                                         });
        }
    }
}