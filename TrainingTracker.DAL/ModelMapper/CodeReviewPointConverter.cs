
namespace TrainingTracker.DAL.ModelMapper
{
    public class CodeReviewPointConverter : EntityConverter<DAL.EntityFramework.CodeReviewPoint, Common.Entity.CodeReviewPoint>
    {
        public override Common.Entity.CodeReviewPoint ConvertFromCore(DAL.EntityFramework.CodeReviewPoint source)
        {
            if (source == null) return new Common.Entity.CodeReviewPoint();

            return new Common.Entity.CodeReviewPoint
            {
                PointId = source.CodeReviewPointId,
                Description = source.Description,
                Title = source.PointTitle,
                Rating =source.CodeReviewPointType,
                CodeReviewTagId = source.CodeReviewTagId
            };
        }

        public override DAL.EntityFramework.CodeReviewPoint ConvertToCore(Common.Entity.CodeReviewPoint source)
        {
            throw new System.NotImplementedException();
        }

       
    }
}
