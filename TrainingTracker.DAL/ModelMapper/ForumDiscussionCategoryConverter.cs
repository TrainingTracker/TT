using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class ForumDiscussionCategoryConverter : EntityConverter<ForumDiscussionCategory, ForumCategory>
    {
        public override ForumCategory ConvertFromCore(ForumDiscussionCategory sourceForumCategory)
        {
            return new ForumCategory
            {
                Id = sourceForumCategory.Id,
                Title = sourceForumCategory.Title,
                Description = sourceForumCategory.Title,
                CreatedOn = sourceForumCategory.CreatedOn
            };
        }

        public override ForumDiscussionCategory ConvertToCore(ForumCategory sourceForumCategory)
        {
            return new ForumDiscussionCategory
            {
                Id = sourceForumCategory.Id,
                Title = sourceForumCategory.Title,
                Description = sourceForumCategory.Title,
                CreatedOn = sourceForumCategory.CreatedOn
            };
        }
    }
}