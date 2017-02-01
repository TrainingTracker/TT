using System.Data.Entity;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class ForumUserHelpPostRepository : Repository<ForumUserHelpPost>, IForumUserHelpPostRepository
    {
        private readonly TrainingTrackerEntities _context;

        public ForumUserHelpPostRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public ForumUserHelpPost GetPostWithThreads(int postId)
        {
            return _context.ForumUserHelpPosts.Include(t => t.ForumUserHelpThreads).Include(x=>x.User).SingleOrDefault(x => x.Id == postId);
        }

        public PagedResult<ForumUserHelpPost> GetPagedFilteredPosts(string wildcard, int categoryId, int statusId,
            int pageNumber, int pageSize)
        {
            return BaseFilterSearchQuery(wildcard, categoryId, statusId).Page(pageNumber, pageSize);
        }

        private IQueryable<ForumUserHelpPost> BaseFilterSearchQuery(string wildcard, int categoryId, int statusId)
        {
            var wildcardFilter = !string.IsNullOrEmpty(wildcard);
            var categoryFilter = categoryId != 0;
            var statusFilter = statusId != 0;

            IQueryable<ForumUserHelpPost> filterQuery = _context.ForumUserHelpPosts.Include(x => x.User)
                .Include(x => x.ForumUserHelpCategory).Include(x => x.ForumUserHelpStatu);
            if (wildcardFilter) filterQuery = filterQuery.Where(x => x.Title.Contains(wildcard) || x.Description.Contains(wildcard));
            if (categoryFilter) filterQuery = filterQuery.Where(x => x.CategoryId == categoryId);
            if (statusFilter) filterQuery = filterQuery.Where(x => x.StatusId == statusId);

            return filterQuery.OrderByDescending(x => x.CreatedOn);
        }
    }
}