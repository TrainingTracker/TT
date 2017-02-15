using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class ForumUserHelpThreadConverter : EntityConverter<ForumUserHelpThread, ForumThread>
    {
        private UserConverter _userConverter;
        public UserConverter UserConverter
        {
            get { return _userConverter ?? (_userConverter = new UserConverter()); }
        }

        public override ForumThread ConvertFromCore(ForumUserHelpThread sourceForumThread)
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

        public override ForumUserHelpThread ConvertToCore(ForumThread sourceForumThread)
        {
            return new ForumUserHelpThread
            {
                Id = sourceForumThread.ThreadId,
                PostId = sourceForumThread.PostId,
                Description = sourceForumThread.Description,
                AddedBy = sourceForumThread.AddedBy,
                CreatedOn = sourceForumThread.CreatedOn
            };
        }
    }
}