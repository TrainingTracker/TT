using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Common.ViewModel
{
    public class QuestionVm
    {
        public List<Skill> Categories { get; set; }
        public List<Question> Questions { get; set; }
    }
}
