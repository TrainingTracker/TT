
using System.Data.Entity;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class ReleaseRepository : Repository<EntityFramework.Release>, IReleaseRepository
    {
        private readonly TrainingTrackerEntities _context;
        public ReleaseRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public PagedResult<EntityFramework.Release> GetPagedFilteredSessions(string wildcard, int searchReleaseId, int pageNumber, int pageSize)
        {
            if (searchReleaseId == 0)
            {
                return BaseFilterSearchQuery(wildcard, searchReleaseId).AsNoTracking()
                                                                  .Page(pageNumber, pageSize);
            }
            var pagedResult = BaseFilterSearchQuery(wildcard, 0).AsNoTracking()
                                                                 .Page(pageNumber, pageSize);

            if (pagedResult.Results.Any(x => x.ReleaseId == searchReleaseId) && pagedResult.Results.IndexOf(pagedResult.Results.FirstOrDefault(x=>x.ReleaseId == searchReleaseId)) == 0)
            {
                return pagedResult;
            }

            pagedResult.Results.Remove(pagedResult.Results.FirstOrDefault(x => x.ReleaseId == searchReleaseId));
            var sessionResult = BaseFilterSearchQuery(wildcard, searchReleaseId).AsNoTracking()
                                                                 .Page(pageNumber, pageSize);
            pagedResult.Results.Insert(0,sessionResult.Results.FirstOrDefault());
            return pagedResult;
        }

        public EntityFramework.Release GetRecentRelease()
        {
            return _context.Releases.OrderByDescending(x => x.ReleaseId)
                                    .Where(x => x.IsPublished && (x.Major > 0 || x.Minor > 0 || x.Patch > 0))
                                    .Take(1)
                                    .FirstOrDefault();
        }

        private IQueryable<EntityFramework.Release> BaseFilterSearchQuery(string wildcard, int searchReleaseId)
        {
            bool wildcardFilter = !string.IsNullOrEmpty(wildcard);
            bool sessionFilter = searchReleaseId != 0;

            IQueryable<EntityFramework.Release> filterQuery = _context.Releases.Include(x => x.User).Where(x => x.IsPublished);

            if (wildcardFilter) filterQuery = filterQuery.Where(x => x.Title.Contains(wildcard) || x.Description.Contains(wildcard));
            if (sessionFilter) filterQuery = filterQuery.Where(x => x.ReleaseId == searchReleaseId);

            return filterQuery.OrderByDescending(x => x.ReleaseDate);
        }
    }
}
