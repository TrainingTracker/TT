using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class ForumUserHelpPostConverter : EntityConverter<ForumUserHelpPost, ForumPost>
    {
        private ForumUserHelpThreadConverter _threadConverter;
        public ForumUserHelpThreadConverter ThreadConverter
        {
            get { return _threadConverter ?? (_threadConverter = new ForumUserHelpThreadConverter()); }
        }

        private UserConverter _userConverter;
        public UserConverter UserConverter
        {
            get { return _userConverter ?? (_userConverter = new UserConverter()); }
        }

        public override ForumPost ConvertFromCore(ForumUserHelpPost sourceForumPost)
        {
            return new ForumPost
            {
                PostId = sourceForumPost.Id,
                StatusId = sourceForumPost.StatusId,
                CategoryId = sourceForumPost.CategoryId,
                Title = sourceForumPost.Title,
                Description = sourceForumPost.Title,
                AddedBy = sourceForumPost.AddedBy,
                Threads = sourceForumPost.ForumUserHelpThreads == null ? null : ThreadConverter.ConvertListFromCore(sourceForumPost.ForumUserHelpThreads.ToList()),
                AddedByUser = sourceForumPost.User == null ? null : UserConverter.ConvertFromCore(sourceForumPost.User)
            };
        }

        public override ForumUserHelpPost ConvertToCore(ForumPost sourceForumPost)
        {
            return new ForumUserHelpPost
            {
                Id = sourceForumPost.PostId,
                StatusId = sourceForumPost.StatusId,
                CategoryId = sourceForumPost.CategoryId,
                Title = sourceForumPost.Title,
                AddedBy = sourceForumPost.AddedBy,
                Description = sourceForumPost.Title
            };
        }
    }
}