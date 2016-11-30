using System;
using System.Collections.Generic;

namespace TrainingTracker.Common.Entity
{
    public class Session
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Presenter { get; set; }
        public DateTime AddedOn { get; set; }
        public DateTime Date { get; set; }
        public string PresenterFullName { get; set; }
        public string[] Attendee { get; set; }
        public List<User> SessionAttendees { get; set; }
        public string VideoFileName { get; set; }
        public string SlideName { get; set; }
        public bool IsNeW{get; set;}
    }
}