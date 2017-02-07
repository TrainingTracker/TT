using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class ForumDiscussionStatusConverter : EntityConverter<ForumDiscussionStatu, ForumStatus>
    {
        public override ForumStatus ConvertFromCore(ForumDiscussionStatu sourceForumStatus)
        {
            return new ForumStatus
            {
                Id = sourceForumStatus.Id,
                Title = sourceForumStatus.Title,
                CreatedOn = sourceForumStatus.CreatedOn
            };
        }

        public override ForumDiscussionStatu ConvertToCore(ForumStatus sourceForumStatus)
        {
            return new ForumDiscussionStatu
            {
                Id = sourceForumStatus.Id,
                Title = sourceForumStatus.Title,
                CreatedOn = sourceForumStatus.CreatedOn
            };
        }
    }
}