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
            int duration = 0;
            if (data.Courses == null || (duration = data.Courses.Sum(x => x.Duration)) != data.Duration)
            {
                // duration will be 0 if data.Courses is null else sum of duration of all courses
                data.Duration = duration;
            }
            
            return (LearningMapDataAccessor.AddLearningMap(data));
        }


        public bool UpdateLearningMap(LearningMap data)
        {
            int duration = 0;
            if (data.Courses == null || (duration = data.Courses.Sum(x => x.Duration)) != data.Duration)
            {
                // duration will be 0 if data.Courses is null else sum of duration of all courses
                data.Duration = duration;
            }
           
            return (LearningMapDataAccessor.UpdateLearningMap(data));
        }

        public bool DeleteLearningMap(int id)
        {
            if (id > 0 )
            {
                return LearningMapDataAccessor.DeleteLearningMap(id);
            }
            return false;
        }

    }
}
