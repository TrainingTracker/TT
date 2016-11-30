namespace TrainingTracker.Common.Entity
{
    public class QuestionLevelMapping
    {
        public int Id { get; set; }
        public int QuestionId { get; set; }
        public short Level { get; set; }
        public short ExperienceStartRange { get; set; }
        public short ExperienceEndRange { get; set; }
    }
}