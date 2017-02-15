using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class ForumUserHelpCategoryConverter : EntityConverter<ForumUserHelpCategory, ForumCategory>
    {
        public override ForumCategory ConvertFromCore(ForumUserHelpCategory sourceForumCategory)
        {
            return new ForumCategory
            {
                Id = sourceForumCategory.Id,
                Title = sourceForumCategory.Title,
                Description = sourceForumCategory.Title,
                CreatedOn = sourceForumCategory.CreatedOn
            };
        }

        public override ForumUserHelpCategory ConvertToCore(ForumCategory sourceForumCategory)
        {
            return new ForumUserHelpCategory
            {
                Id = sourceForumCategory.Id,
                Title = sourceForumCategory.Title,
                Description = sourceForumCategory.Title,
                CreatedOn = sourceForumCategory.CreatedOn
            };
        }
    }
}
