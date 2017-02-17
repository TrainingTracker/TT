using SubtopicContentModel =TrainingTracker.Common.Entity.SubtopicContent;
using TrainingTracker.DAL.EntityFramework;


namespace TrainingTracker.DAL.ModelMapper
{
    public class SubtopicContentConverter : EntityConverter<SubtopicContent, SubtopicContentModel>
    {

        public override SubtopicContentModel ConvertFromCore(SubtopicContent sourceSubtopicContent)
        {
            return new SubtopicContentModel
            {
                Id = sourceSubtopicContent.Id,
                CourseSubtopicId = sourceSubtopicContent.CourseSubtopicId,
                Name = sourceSubtopicContent.Name,
                Description = sourceSubtopicContent.Description,
                Url = sourceSubtopicContent.Url,
                CreatedOn = sourceSubtopicContent.CreatedOn,
                IsActive = sourceSubtopicContent.IsActive,
                AddedBy = sourceSubtopicContent.AddedBy,
                SortOrder = sourceSubtopicContent.SortOrder
            };
        }

        public override SubtopicContent ConvertToCore(SubtopicContentModel sourceSubtopicContent)
        {
            return new SubtopicContent
            {
                Id = sourceSubtopicContent.Id,
                CourseSubtopicId = sourceSubtopicContent.CourseSubtopicId,
                Name = sourceSubtopicContent.Name,
                Description = sourceSubtopicContent.Description,
                Url = sourceSubtopicContent.Url,
                CreatedOn = sourceSubtopicContent.CreatedOn,
                IsActive = sourceSubtopicContent.IsActive,
                AddedBy = sourceSubtopicContent.AddedBy,
                SortOrder = sourceSubtopicContent.SortOrder
            };
        }
    }
}
