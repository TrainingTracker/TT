using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class ForumDiscussionPostConverter : EntityConverter<ForumDiscussionPost, ForumPost>
    {
        private ForumDiscussionThreadConverter _threadConverter;
        public ForumDiscussionThreadConverter ThreadConverter
        {
            get { return _threadConverter ?? (_threadConverter = new ForumDiscussionThreadConverter()); }
        }

        private UserConverter _userConverter;
        public UserConverter UserConverter
        {
            get { return _userConverter ?? (_userConverter = new UserConverter()); }
        }

        private ForumDiscussionStatusConverter _forumStatusConverter;
        public ForumDiscussionStatusConverter ForumStatusConverter
        {
            get { return _forumStatusConverter ?? (_forumStatusConverter = new ForumDiscussionStatusConverter()); }
        }

        private ForumDiscussionCategoryConverter _forumCategoryConverter;
        public ForumDiscussionCategoryConverter ForumCategoryConverter
        {
            get { return _forumCategoryConverter ?? (_forumCategoryConverter = new ForumDiscussionCategoryConverter()); }
        }

        public override ForumPost ConvertFromCore(ForumDiscussionPost sourceForumPost)
        {
            return new ForumPost
            {
                PostId = sourceForumPost.Id,
                StatusId = sourceForumPost.StatusId,
                CategoryId = sourceForumPost.CategoryId,
                Title = sourceForumPost.Title,
                Description = sourceForumPost.Description,
                AddedBy = sourceForumPost.AddedBy,
                CreatedOn = sourceForumPost.CreatedOn,
                AddedByUser = sourceForumPost.User == null ? null : UserConverter.ConvertFromCore(sourceForumPost.User),
                Status = sourceForumPost.ForumDiscussionStatu == null ? null : ForumStatusConverter.ConvertFromCore(sourceForumPost.ForumDiscussionStatu),
                Category = sourceForumPost.ForumDiscussionCategory == null ? null : ForumCategoryConverter.ConvertFromCore(sourceForumPost.ForumDiscussionCategory)
            };
        }

        public override ForumDiscussionPost ConvertToCore(ForumPost sourceForumPost)
        {
            return new ForumDiscussionPost
            {
                Id = sourceForumPost.PostId,
                StatusId = sourceForumPost.StatusId,
                CategoryId = sourceForumPost.CategoryId,
                Title = sourceForumPost.Title,
                AddedBy = sourceForumPost.AddedBy,
                Description = sourceForumPost.Description
            };
        }
    }
}
