using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using CommonModel = TrainingTracker.Common.Entity;
using EFModel = TrainingTracker.DAL.EntityFramework;
using System.Linq;

namespace TrainingTracker.BLL
{
    public class CourseBl : BussinessBase
    {
        public int AddOrUpdateCourse(CommonModel.Course courseData)
        {
            var data = ModelMapper.MapToEFCourseModel(courseData);
            
            data.Icon = courseData.Icon ?? Constants.DefaultCourseIcon;
            data.Description = courseData.Description ?? "";

            if (data.Id == 0)
            {
                data.IsActive = true;
                data.CreatedOn = DateTime.Now;
                UnitOfWork.CourseRepository.Add(data);
            }

            return  UnitOfWork.Commit() > 0 ? data.Id : 0;
        }
    }
}
