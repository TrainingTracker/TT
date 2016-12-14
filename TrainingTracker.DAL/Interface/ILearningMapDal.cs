using System;
using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    public interface ILearningMapDal
    {
        LearningMap GetLearningMapWithAllData(int id);
        //List<LearningMap> GetAllLearningMaps();
        //List<User> GetAllTrainees();
        //List<Course> GetAllCourses();
        //List<User> GetTraineesOfLearningMap();
        //List<Course> GetCoursesOfLearningMap();

        //int AddLearningMap(LearningMap data);
        //int AddTraineesInLearningMap(List<User> traineeList);
        
        //bool UpdateLearningMap(LearningMap data);
        //bool UpdateCoursesOfLearningMap(List<Course> courseList);
    }
}
