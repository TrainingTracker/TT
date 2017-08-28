
using TrainingTracker.Common.Enumeration;

namespace TrainingTracker.Common.Entity
{
    public partial class CrRatingCalcWeightConfig
    {
        public int Id { get; set; }
        public int ReviewPointTypeId { get; set; }
        public float Weight { get; set; }
        public int CrRatingCalcConfigId { get; set; }
    
        public  CrRatingCalcConfig CrRatingCalcConfig { get; set; }
        public  CodeReviewRating ReviewPointType { get; set; }
    }
}
