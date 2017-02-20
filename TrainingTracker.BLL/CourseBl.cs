using System;
using System.Collections.Generic;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using CommonModel = TrainingTracker.Common.Entity;
using EFModel = TrainingTracker.DAL.EntityFramework;


namespace TrainingTracker.BLL
{
    public class CourseBl : BussinessBase
    {
       
        public int AddOrUpdateCourse(CommonModel.Course courseData, int currentUserId)
        {
           // var data = ModelMapper.MapToEfCourseModel(courseData);
            var courseEntity = new EFModel.Course();
            if (courseData.Id == 0) // Add
            {
                courseEntity.Id = 0;
                courseEntity.Name = courseData.Name;
                courseEntity.Description = courseData.Description ?? "";
                courseEntity.Icon = courseData.Icon ?? Constants.DefaultCourseIcon;
                courseEntity.Duration = courseData.Duration;
                courseEntity.CreatedOn = DateTime.Now;
                courseEntity.AddedBy = currentUserId;
                courseEntity.IsActive = true;
                courseEntity.IsPublished = false;

                UnitOfWork.CourseRepository.Add(courseEntity);
            }
            else    //Update
            {
                courseEntity = UnitOfWork.CourseRepository.Get(courseData.Id);
                if (courseEntity == null)
                {
                    return 0;
                }
                // ToDo : Try attach/entry for updating(save one trip to DB) or update only modified property
                // ToDo : Set IsPublished and IsActive
                courseEntity.Name = courseData.Name;
                courseEntity.Icon = courseData.Icon ?? Constants.DefaultCourseIcon;
                courseEntity.Description = courseData.Description ?? "";
                courseEntity.Duration = courseData.Duration;
            }

            return  UnitOfWork.Commit() > 0 ? courseEntity.Id : 0;
        }
    }
}
