using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.Interface
{
    public interface IForumDiscussionPostRepository : IRepository<ForumDiscussionPost>
    {
        ForumDiscussionPost GetPostWithThreads(int postId);

        PagedResult<ForumDiscussionPost> GetPagedFilteredPosts(string wildcard, int categoryId, int statusId, int searchPostId,
            int pageNumber, int pageSize);
    }
}