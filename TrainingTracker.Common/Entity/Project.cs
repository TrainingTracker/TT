using System;

namespace TrainingTracker.Common.Entity
{
    public class Project
    {
        public int ProjectId { get; set; }
        public string Title { get; set; }
        public string ProjectDescription { get; set; }
        public int CreatedBy { get; set; }
        public string ProjectType { get; set; }
        public string ClientName { get; set; }
        public DateTime AddedOn { get; set; }
        public int UserProjectId { get; set; }
    }
}