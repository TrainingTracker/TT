using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Interface
{
    public interface IForumUserHelpPostRepository : IRepository<ForumUserHelpPost>
    {
        ForumUserHelpPost GetPostWithThreads(int postId);

        PagedResult<ForumUserHelpPost> GetPagedFilteredPosts(string wildcard, int categoryId, int statusId, int searchPostId,
            int pageNumber, int pageSize);
    }
}