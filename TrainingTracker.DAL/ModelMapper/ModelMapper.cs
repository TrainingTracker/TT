using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using Course = TrainingTracker.Common.Entity.Course;
using LearningMap = TrainingTracker.Common.Entity.LearningMap;
using CourseSubtopic = TrainingTracker.Common.Entity.CourseSubtopic;
using User = TrainingTracker.Common.Entity.User;
using TrainingTracker.Common.Constants;
using TrainingTracker.DAL.EntityFramework;

namespace TrainingTracker.DAL.ModelMapper
{
    /// <summary>
    /// This class contains all the mapper methods which maps the EF generated model to the custom model(Common Entity classes)
    /// </summary>
    public class ModelMapper
    {

        /// <summary>
        /// Map the Course Model(EF generated) object to custom entity class Course object
        /// </summary>
        /// <param name="objectToMap">EF generated course model object which will be used for mapped</param>
        /// <returns>Mapped custom entity course object if inputted parameter objectToMap is not null otherwise returns null</returns>
        public Course MapFromCourseModel(EntityFramework.Course objectToMap)
        {
            if (objectToMap == null)
                return null;

            return new Course
            {
                Id = objectToMap.Id,
                Name = objectToMap.Name,
                Description = objectToMap.Description,
                Icon = objectToMap.Icon,
                AddedBy = objectToMap.AddedBy,
                IsActive = objectToMap.IsActive,
                IsPublished = objectToMap.IsPublished,
                Duration = objectToMap.Duration,
                CreatedOn = objectToMap.CreatedOn,

            };

            
        }

        /// <summary>
        /// Map custom entity class Course object to EF generated class Course object
        /// </summary>
        /// <param name="objectToMap">Custom entity Course object</param>
        /// <returns>EF generated course object if inputted parameter objectToMap is not null otherwise returns null</returns>
        public EntityFramework.Course MapToCourseModel(Course objectToMap)
        {
            if (objectToMap == null)
                return null;

            return new EntityFramework.Course
            {
                Id = objectToMap.Id,
                Name = objectToMap.Name,
                Description = objectToMap.Description,
                Icon = objectToMap.Icon,
                AddedBy = objectToMap.AddedBy,
                IsActive = objectToMap.IsActive,
                IsPublished = objectToMap.IsPublished,
                Duration = objectToMap.Duration,
                CreatedOn = objectToMap.CreatedOn

            };
        }

        /// <summary>
        /// Map custom entity class CourseSubtopic object to EF generated class CourseSubtopic object
        /// </summary>
        /// <param name="objectToMap">Custom entity CourseSubtopic object</param>
        /// <returns>EF generated course object if inputted parameter objectToMap is not null otherwise returns null</returns>
        public EntityFramework.CourseSubtopic MapToCourseSubtopic(CourseSubtopic objectToMap)
        {
            if (objectToMap == null)
                return null;

            return new EntityFramework.CourseSubtopic
            {
                Id = objectToMap.Id,
                CourseId = objectToMap.CourseId,
                Name = objectToMap.Name,
                Description = objectToMap.Description,
                AddedBy = objectToMap.AddedBy,
                SortOrder = objectToMap.SortOrder,
                IsActive = objectToMap.IsActive,
                CreatedOn = objectToMap.CreatedOn
            };

        }

        /// <summary>
        ///  Map the CourseSubtopic Model(EF generated) object to custom entity class CourseSubtopic object
        /// </summary>
        /// <param name="objectToMap">EF generated CourseSubtopic model object which will be used for mapping</param>
        /// <returns>Custom Entity class CourseSubtopic object if inputted parameter objectToMap is not null otherwise returns null</returns>
        public CourseSubtopic MapFromCourseSubtopic(EntityFramework.CourseSubtopic objectToMap)
        {
            if (objectToMap == null)
                return null;

            return new CourseSubtopic
            {
                Id = objectToMap.Id,
                CourseId = objectToMap.CourseId,
                Name = objectToMap.Name,
                Description = objectToMap.Description,
                AddedBy = objectToMap.AddedBy,
                SortOrder = objectToMap.SortOrder,
                IsActive = objectToMap.IsActive,
                CreatedOn = objectToMap.CreatedOn,
                
            };

        }
    }
}
