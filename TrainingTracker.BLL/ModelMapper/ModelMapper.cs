using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.Common.Constants;
using EFModel = TrainingTracker.DAL.EntityFramework;
using CommonModel = TrainingTracker.Common.Entity;


namespace TrainingTracker.BLL.ModelMapper
{
    /// <summary>
    /// This class contains all the mapper methods which maps the EF generated model to the custom model(Common Entity classes) and vice versa
    /// </summary>
    public class ModelMapper
    {

        /// <summary>
        /// Map the Course Model(EF generated) object to custom entity class Course object
        /// </summary>
        /// <param name="objectToMap">EF generated course model object which will be used for mapped</param>
        /// <returns>Mapped custom entity course object if inputted parameter objectToMap is not null otherwise returns null</returns>
        public CommonModel.Course MapFromEfCourseModel(EFModel.Course objectToMap)
        {
            if (objectToMap == null)
                return null;

            return new CommonModel.Course
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
                CourseSubtopics = objectToMap.CourseSubtopics == null ? null : objectToMap.CourseSubtopics.Select(s =>
                {
                    var subtopic = MapFromEfCourseSubtopic(s);
                    return subtopic;
                }).ToList()
               
            };

            
        }

        /// <summary>
        /// Map custom entity class Course object to EF generated class Course object
        /// </summary>
        /// <param name="objectToMap">Custom entity Course object</param>
        /// <returns>EF generated course object if inputted parameter objectToMap is not null otherwise returns null</returns>
        public EFModel.Course MapToEfCourseModel(CommonModel.Course objectToMap)
        {
            if (objectToMap == null)
                return null;

            return new EFModel.Course
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
        public EFModel.CourseSubtopic MapToEfCourseSubtopic(CommonModel.CourseSubtopic objectToMap)
        {
            if (objectToMap == null)
                return null;

            return new EFModel.CourseSubtopic
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
        public CommonModel.CourseSubtopic MapFromEfCourseSubtopic(EFModel.CourseSubtopic objectToMap)
        {
            if (objectToMap == null)
                return null;

            return new CommonModel.CourseSubtopic
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
