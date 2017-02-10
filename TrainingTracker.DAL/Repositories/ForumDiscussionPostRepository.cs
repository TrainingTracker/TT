using System.Data.Entity;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.Repositories
{
    public class ForumDiscussionPostRepository : Repository<ForumDiscussionPost>, IForumDiscussionPostRepository
    {
        private readonly TrainingTrackerEntities _context;
        public ForumDiscussionPostRepository(TrainingTrackerEntities context)
            : base(context)
        {
            _context = context;
        }

        public ForumDiscussionPost GetPostWithThreads(int postId)
        {
            return _context.ForumDiscussionPosts.Include(t => t.ForumDiscussionThreads).Include(x => x.User).SingleOrDefault(x => x.Id == postId);
        }

        public PagedResult<ForumDiscussionPost> GetPagedFilteredPosts(string wildcard, int categoryId, int statusId, int searchPostId, int addedBy,
            int pageNumber, int pageSize)
        {
            return BaseFilterSearchQuery(wildcard, categoryId, statusId, searchPostId,  addedBy).Page(pageNumber, pageSize);
        }

        private IQueryable<ForumDiscussionPost> BaseFilterSearchQuery(string wildcard, int categoryId, int statusId, int searchPostId, int addedBy)
        {
            var wildcardFilter = !string.IsNullOrEmpty(wildcard);
            var categoryFilter = categoryId != 0;
            var statusFilter = statusId != 0;
            var postFilter = searchPostId != 0;
            var addedByFilter = addedBy != 0;

            IQueryable<ForumDiscussionPost> filterQuery = _context.ForumDiscussionPosts.Include(x => x.User)
                .Include(x => x.ForumDiscussionCategory).Include(x => x.ForumDiscussionStatu);
            if (wildcardFilter) filterQuery = filterQuery.Where(x => x.Title.Contains(wildcard) || x.Description.Contains(wildcard));
            if (categoryFilter) filterQuery = filterQuery.Where(x => x.CategoryId == categoryId);
            if (statusFilter) filterQuery = filterQuery.Where(x => x.StatusId == statusId);
            if (postFilter) filterQuery = filterQuery.Where(x => x.Id == searchPostId);
            if (addedByFilter) filterQuery = filterQuery.Where(x => x.AddedBy == addedBy);

            return filterQuery.OrderByDescending(x => x.CreatedOn);
        }
    }
}