﻿using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.BLL
{
    public class LearningPathBL : BussinessBase
    {
        public int AddCourse(Course courseToAdd)
        {
            courseToAdd.IsActive = true;
            courseToAdd.Icon = courseToAdd.Icon ?? Constants.DefaultCourseIcon;
            courseToAdd.CreatedOn = DateTime.Now;
            courseToAdd.Description = courseToAdd.Description ?? "";

            return (LearningPathDataAccessor.AddCourse(courseToAdd));
        }

        /// <summary>
        /// Fetches all courses based on filter 
        /// </summary>
        /// <param name="searchKeyword">search keyword for free text search</param>
        /// <returns>List of Courses on filter</returns>
        public List<Course> FilterCourses(string searchKeyword)
        {
            return LearningPathDataAccessor.FilterCourses(searchKeyword);
        }

        public bool UpdateCourse(Course courseToUpdate)
        {
            courseToUpdate.Description = courseToUpdate.Description ?? "";
            courseToUpdate.Icon = courseToUpdate.Icon ?? Constants.DefaultCourseIcon;
            return LearningPathDataAccessor.UpdateCourse(courseToUpdate);
        }


        public bool DeleteCourse(int id)
        {
            return LearningPathDataAccessor.DeleteCourse(id);
        }


        public int AddCourseSubtopic(CourseSubtopic subtopicToAdd)
        {
            subtopicToAdd.Description = subtopicToAdd.Description ?? "";
            subtopicToAdd.IsActive = true;
            subtopicToAdd.CreatedOn = DateTime.Now;

            return (LearningPathDataAccessor.AddCourseSubtopic(subtopicToAdd));
        }

        
        public bool UpdateCourseSubtopic(CourseSubtopic subtopicToUpdate)
        {
            subtopicToUpdate.Description = subtopicToUpdate.Description ?? "";
            return LearningPathDataAccessor.UpdateCourseSubtopic(subtopicToUpdate);
        }

       
        public bool DeleteCourseSubtopic(int id)
        {
            return LearningPathDataAccessor.DeleteCourseSubtopic(id);
        }


        public bool AddSubtopicContent(SubtopicContent dataToAdd, out int id)
        {
            dataToAdd.IsActive = true;
            dataToAdd.CreatedOn = DateTime.Now;
            dataToAdd.Description = dataToAdd.Description ?? String.Empty;
            return LearningPathDataAccessor.AddSubtopicContent(dataToAdd, out id);
        }


        public bool UpdateSubtopicContent(SubtopicContent dataToUpdate)
        {
            dataToUpdate.Description = dataToUpdate.Description ?? "";
            return LearningPathDataAccessor.UpdateSubtopicContent(dataToUpdate);
        }


        public bool DeleteSubtopicContent(int id)
        {

            return LearningPathDataAccessor.DeleteSubtopicContent(id);
        }


        public bool AddAssignment(Assignment dataToAdd, out int id)
        {
            dataToAdd.Description = dataToAdd.Description ?? "";
            dataToAdd.CreatedOn = DateTime.Now;
            dataToAdd.IsActive = true;

            return LearningPathDataAccessor.AddAssignment(dataToAdd, out id);
        }


        public bool UpdateAssignment(Assignment dataToUpdate)
        {
            dataToUpdate.Description = dataToUpdate.Description ?? "";
            return LearningPathDataAccessor.UpdateAssignment(dataToUpdate);
        }


        public bool DeleteAssignment(int id)
        {
            return LearningPathDataAccessor.DeleteAssignment(id);
        }


        public List<Course> GetAllCoursesWithSubtopics()
        {
            return LearningPathDataAccessor.GetAllCoursesWithSubtopics();
        }


        public List<SubtopicContent> GetSubtopicContents(int subtopicId)
        {
            return LearningPathDataAccessor.GetSubtopicContents(subtopicId);
        }

        public List<Assignment> GetAssignments(int subtopicContentId)
        {
            return LearningPathDataAccessor.GetAssignments(subtopicContentId);
        }
    }
}
