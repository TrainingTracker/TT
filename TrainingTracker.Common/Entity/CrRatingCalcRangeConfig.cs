namespace TrainingTracker.Common.Entity
{
    public class CrRatingCalcRangeConfig
    {
        public int Id { get; set; }
        public int FeedbackTypeId { get; set; }
        public float RangeMin { get; set; }
        public float RangeMax { get; set; }
        public int CrRatingCalcConfigId { get; set; }
    
        public CrRatingCalcConfig CrRatingCalcConfig { get; set; }
        public Enumeration.FeedbackType FeedbackType { get; set; }
    }
}
