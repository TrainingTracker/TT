
using System.Data.Entity;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class ReleaseRepository :Repository<EntityFramework.Release>, IReleaseRepository
    {
        private readonly TrainingTrackerEntities _context;
        public ReleaseRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public PagedResult<EntityFramework.Release> GetPagedFilteredSessions(string wildcard, int searchReleaseId, int pageNumber, int pageSize)
        {
            return BaseFilterSearchQuery(wildcard, searchReleaseId).AsNoTracking()
                                                                   .Page(pageNumber, pageSize);
        }

        private IQueryable<EntityFramework.Release> BaseFilterSearchQuery(string wildcard,  int searchReleaseId)
        {
            bool wildcardFilter = !string.IsNullOrEmpty(wildcard);
            bool sessionFilter = searchReleaseId != 0;

            IQueryable<EntityFramework.Release> filterQuery = _context.Releases.Include(x=>x.User).Where(x=>x.IsPublished);

            if (wildcardFilter) filterQuery = filterQuery.Where(x => x.Title.Contains(wildcard) || x.Description.Contains(wildcard));
            if (sessionFilter) filterQuery = filterQuery.Where(x => x.ReleaseId == searchReleaseId);

            return filterQuery.OrderByDescending(x => x.ReleaseDate);
        }
    }
}
