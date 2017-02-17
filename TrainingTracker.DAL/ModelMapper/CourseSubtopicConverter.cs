using System.Linq;
using CourseSubtopicModel = TrainingTracker.Common.Entity.CourseSubtopic;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    public class CourseSubtopicConverter : EntityConverter<CourseSubtopic, CourseSubtopicModel>
    {
        
        public override CourseSubtopicModel ConvertFromCore(CourseSubtopic sourceSubtopic)
        {
            return new CourseSubtopicModel
            {
                Id = sourceSubtopic.Id,
                Name = sourceSubtopic.Name,
                CourseId = sourceSubtopic.CourseId,
                Description = sourceSubtopic.Description,
                SortOrder = sourceSubtopic.SortOrder,
                AddedBy = sourceSubtopic.AddedBy,
                IsActive = sourceSubtopic.IsActive,
                CreatedOn = sourceSubtopic.CreatedOn
            };
        }


        public override CourseSubtopic ConvertToCore(CourseSubtopicModel sourceSubtopic)
        {
            return new CourseSubtopic
            {
                Id = sourceSubtopic.Id,
                Name = sourceSubtopic.Name,
                CourseId = sourceSubtopic.CourseId,
                Description = sourceSubtopic.Description,
                SortOrder = sourceSubtopic.SortOrder,
                AddedBy = sourceSubtopic.AddedBy,
                IsActive = sourceSubtopic.IsActive,
                CreatedOn = sourceSubtopic.CreatedOn
            };
        }
    }
}
