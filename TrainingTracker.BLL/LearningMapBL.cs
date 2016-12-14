using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;


namespace TrainingTracker.BLL
{
    public class LearningMapBL : BussinessBase
    {
        public LearningMap GetLearningMapWithAllData(int id)
        {
            return (LearningMapDataAccessor.GetLearningMapWithAllData(id));
        }
    }
}
