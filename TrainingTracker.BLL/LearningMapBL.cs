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


        public List<Course> GetAllCourses()
        {
            return LearningPathDataAccessor.GetAllCourses();
        }


        public List<LearningMap> GetAllLearningMaps(int teamId)
        {
            return (LearningMapDataAccessor.GetAllLearningMaps(teamId));
        }


        public List<User> GetAllTrainees(int teamId)
        {
            return (LearningMapDataAccessor.GetAllTrainees(teamId));
        }


        public int AddLearningMap(LearningMap data)
        {
            return (LearningMapDataAccessor.AddLearningMap(data));
        }


        public bool UpdateLearningMap(LearningMap data)
        {
            return (LearningMapDataAccessor.UpdateLearningMap(data));
        }

    }
}
