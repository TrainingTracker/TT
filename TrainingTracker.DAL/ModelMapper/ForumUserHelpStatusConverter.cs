using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class ForumUserHelpStatusConverter : EntityConverter<ForumUserHelpStatu, ForumStatus>
    {
        public override ForumStatus ConvertFromCore(ForumUserHelpStatu sourceForumStatus)
        {
            return new ForumStatus
            {
                Id = sourceForumStatus.Id,
                Title = sourceForumStatus.Title,
                CreatedOn = sourceForumStatus.CreatedOn
            };
        }

        public override ForumUserHelpStatu ConvertToCore(ForumStatus sourceForumStatus)
        {
            return new ForumUserHelpStatu
            {
                Id = sourceForumStatus.Id,
                Title = sourceForumStatus.Title,
                CreatedOn = sourceForumStatus.CreatedOn
            };
        }
    }
}