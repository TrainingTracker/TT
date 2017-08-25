namespace TrainingTracker.Common.Entity
{
    using System;
    using System.Collections.Generic;
    
    public  class CrRatingCalcConfig
    {
        public CrRatingCalcConfig()
        {
            CrRatingCalcRangeConfigs = new List<CrRatingCalcRangeConfig>();
            CrRatingCalcWeightConfigs = new List<CrRatingCalcWeightConfig>();
        }
    
        public int Id { get; set; }
        public int TeamId { get; set; }
    
        public virtual List<CrRatingCalcRangeConfig> CrRatingCalcRangeConfigs { get; set; }
        public virtual List<CrRatingCalcWeightConfig> CrRatingCalcWeightConfigs { get; set; }
    }
}
