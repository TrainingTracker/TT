using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
