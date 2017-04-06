using System.Collections.Generic;
using System.Linq;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using Skill = TrainingTracker.Common.Entity.Skill;

namespace TrainingTracker.DAL.Repositories
{
   public class SkillRepository : Repository<EntityFramework.Skill>,ISkillRepository
    {
        private readonly TrainingTrackerEntities _context;
        public SkillRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<Skill> GetAllSkillsForTrainee(int userId)
        {
          return  _context.Feedbacks.Where(x=>x.AddedFor == userId && x.FeedbackType == (int) Common.Enumeration.FeedbackType.Skill)
                                    .Join(_context.Skills, f => f.SkillId, s => s.SkillId, (f, s) => new { f, s })
                                    .GroupBy(a => a.f.SkillId)
                                    .Select(g => new { g.Key, Item = g.FirstOrDefault(), AverageRating = g.Average(x => x.f.Rating) })
                                    .Select(x=> new Skill
                                           {
                                             Name  = x.Item.s.Name
                                           })
                                     .OrderBy(x=>x.Name);
        }
    }
}
