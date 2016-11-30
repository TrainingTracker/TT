using System;

namespace TrainingTracker.Common.Entity
{
    public class Skill
    {
        public int SkillId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime AddedOn { get; set; }
        public int AddedBy { get; set; }
        public int Rating { get; set; }
        public int Count { get; set; }
    }
}