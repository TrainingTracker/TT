//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TrainingTracker.DAL.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Survey
    {
        public Survey()
        {
            this.SurveyCompletedMetaDatas = new HashSet<SurveyCompletedMetaData>();
            this.SurveySections = new HashSet<SurveySection>();
            this.Teams = new HashSet<Team>();
        }
    
        public int SurveyId { get; set; }
        public string Description { get; set; }
        public bool IsOpen { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual ICollection<SurveyCompletedMetaData> SurveyCompletedMetaDatas { get; set; }
        public virtual ICollection<SurveySection> SurveySections { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
    }
}
