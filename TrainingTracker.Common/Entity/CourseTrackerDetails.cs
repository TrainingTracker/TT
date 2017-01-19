
using System;

namespace TrainingTracker.Common.Entity
{
    /// <summary>
    /// Instance class for tracker details for course
    /// </summary>
    public class CourseTrackerDetails
    {
        /// <summary>
        /// gets and sets course id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// gets and sets Course Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// gets and sets total subtopic content
        /// </summary>
        public int TotalSubTopicCount { get; set; }

        /// <summary>
        /// gets and sets covered subtopic content
        /// </summary>
        public int CoveredSubTopicCount { get; set; }

        /// <summary>
        /// gets and sets total Assignments 
        /// </summary>
        public int TotalAssignmentCount { get; set; }

        /// <summary>
        /// gets and sets covered assignment content
        /// </summary>
        public int CompletedAssignmentCount { get; set; }

        /// <summary>
        /// gets and sets pending assignment
        /// </summary>
        public int PendingAssignmentCount { get; set; }

        /// <summary>
        /// Gets and Sets course Started Date time
        /// </summary>
        public DateTime CourseStarted { get; set; }

        /// <summary>
        /// Gets and Sets course completed Date time
        /// </summary>
        public DateTime? CourseCompleted { get; set; }

        /// <summary>
        /// Course User Details
        /// </summary>
        public User UserDetails { get; set; }

        /// <summary>
        /// gets and sets percentage course covered
        /// </summary>
        public double PercentageCompleted 
        {
            get
            {
                if ((TotalSubTopicCount > 0 || TotalAssignmentCount > 0)
                    &&  (CoveredSubTopicCount <= TotalSubTopicCount && CompletedAssignmentCount <= TotalAssignmentCount))
                {
                    return Math.Round(((float) (CoveredSubTopicCount + CompletedAssignmentCount) * 100 / (TotalSubTopicCount + TotalAssignmentCount)) , 0 , MidpointRounding.AwayFromZero);
                }               
                return 100;
            } 
        }

    }
}
