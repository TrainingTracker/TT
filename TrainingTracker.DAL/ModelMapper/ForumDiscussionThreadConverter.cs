using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class ForumDiscussionThreadConverter : EntityConverter<ForumDiscussionThread, ForumThread>
    {
        private UserConverter _userConverter;
        public UserConverter UserConverter
        {
            get { return _userConverter ?? (_userConverter = new UserConverter()); }
        }

        public override ForumThread ConvertFromCore(ForumDiscussionThread sourceForumThread)
        {
            return new ForumThread
            {
                ThreadId = sourceForumThread.Id,
                PostId = sourceForumThread.PostId,
                Description = sourceForumThread.Description,
                AddedBy = sourceForumThread.AddedBy,
                CreatedOn = sourceForumThread.CreatedOn,
                AddedByUser = sourceForumThread.User == null ? null : UserConverter.ConvertFromCore(sourceForumThread.User)
            };
        }

        public override ForumDiscussionThread ConvertToCore(ForumThread sourceForumThread)
        {
            return new ForumDiscussionThread
            {
                Id = sourceForumThread.ThreadId,
                PostId = sourceForumThread.PostId,
                Description = sourceForumThread.Description,
                AddedBy = sourceForumThread.AddedBy,
                CreatedOn = sourceForumThread.CreatedOn
            };
        }
}