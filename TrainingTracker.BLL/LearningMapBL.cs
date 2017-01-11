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


        public int AddLearningMap(LearningMap data , User currentUser)
        {
            int duration = 0;
            if (data.Courses == null || (duration = data.Courses.Sum(x => x.Duration)) != data.Duration)
            {
                // duration will be 0 if data.Courses is null else sum of duration of all courses
                data.Duration = duration;
            }

            int learningMapId = (LearningMapDataAccessor.AddLearningMap(data)) ;

            if (learningMapId > 0)
            {
                new NotificationBl().AddNewCourseNotification(data.Trainees.ToList(), currentUser.UserId);
            }
            return learningMapId;
        }


        public bool UpdateLearningMap( LearningMap data , User currentUser )
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
            return id > 0 && LearningMapDataAccessor.DeleteLearningMap(id);
        }
    }
}
