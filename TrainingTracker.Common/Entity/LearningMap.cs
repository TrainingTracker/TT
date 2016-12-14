using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainingTracker.Common.Entity
{
    public class LearningMap
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Notes { get; set; }
        public int Duration { get; set; }
        public bool IsCourseRestricted { get; set; }
        public int TeamId { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedBy { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual ICollection<Course> Courses { get; set; }
        public virtual ICollection<User> Trainees { get; set; }
    }
}
