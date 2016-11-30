using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Entity class for Question
    /// </summary>
    public class Question
    {
        /// <summary>
        /// Gets and sets Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets and Sets Question text
        /// </summary>
        public string QuestionText { get; set; }

        /// <summary>
        /// Gets and Sets Questions' Description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets and Sets Skill Id </summary>
        public int SkillId { get; set; }
        
        /// <summary>
        /// Gets and Sets Questions' difficulty Level
        /// </summary>
        public short Level { get; set; }

        /// <summary>
        /// Gets and Sets Question's Added by user Id
        /// </summary>
        public int AddedBy { get; set; }

        /// <summary>
        /// Gets and Sets Question's creation date
        /// </summary>
        public System.DateTime AddedDate { get; set; }

        /// <summary>
        /// Gets and Sets List of Level Mapping of type QuestionLevelMapping Entity
        /// </summary>
        public List<QuestionLevelMapping> LevelMapping { get; set; }

        /// <summary>
        /// Gets and sets Skills for the Questions
        /// </summary>
        public Skill Skill { get; set; }

        /// <summary>
        /// Gets and Sets User for the Questions
        /// </summary>
        public User User { get; set; }
    }
}
