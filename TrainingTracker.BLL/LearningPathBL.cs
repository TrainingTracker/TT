using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;

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

            bool fileCopied = true;
            if (!String.IsNullOrEmpty(dataToAdd.AssignmentAsset) && (fileCopied = UtilityFunctions.CopyFile(dataToAdd.AssignmentAsset, LearningAssetsPath.TempFile, LearningAssetsPath.Assignment)))
            {
                UtilityFunctions.DeleteFile(dataToAdd.AssignmentAsset, LearningAssetsPath.TempFile);
            }
            
            return LearningPathDataAccessor.AddAssignment(dataToAdd, out id) && fileCopied;
        }


        public bool UpdateAssignment(Assignment dataToUpdate)
        {
            dataToUpdate.Description = dataToUpdate.Description ?? "";

            bool fileCopied = true;
            if (!String.IsNullOrEmpty(dataToUpdate.AssignmentAsset) && (fileCopied = UtilityFunctions.CopyFile(dataToUpdate.AssignmentAsset, LearningAssetsPath.TempFile, LearningAssetsPath.Assignment)))
            {
                UtilityFunctions.DeleteFile(dataToUpdate.AssignmentAsset, LearningAssetsPath.TempFile);
            }

            return LearningPathDataAccessor.UpdateAssignment(dataToUpdate) && fileCopied;
        }


        public bool DeleteAssignment(int id)
        {
            return LearningPathDataAccessor.DeleteAssignment(id);
        }


        public List<Course> GetAllCourses()
        {
            return LearningPathDataAccessor.GetAllCourses();
        }

        public Course GetCourseWithSubtopics(int courseId)
        {
            return LearningPathDataAccessor.GetCourseWithSubtopics(courseId);
        }
        public Course GetCourseWithAllData(int courseId)
        {
            return LearningPathDataAccessor.GetCourseWithAllData(courseId);
        }
        public List<SubtopicContent> GetSubtopicContents(int subtopicId)
        {
            return LearningPathDataAccessor.GetSubtopicContents(subtopicId);
        }

        public List<Assignment> GetAssignments(int subtopicContentId)
        {
            return LearningPathDataAccessor.GetAssignments(subtopicContentId);
        }

        public bool SaveSubtopicOrder(List<CourseSubtopic> data) 
        {
            return LearningPathDataAccessor.SaveSubtopicOrder(data);
        }

        public bool SaveSubtopicContentOrder(List<SubtopicContent> data)
        {
            return LearningPathDataAccessor.SaveSubtopicContentOrder(data);
        }

        public bool PublishCourse(int id) {

            return LearningPathDataAccessor.PublishCourse(id);
        }

        
        public bool CopyFile(string fileName, string sourcePath, string targetPath)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory;

                targetPath = strPath + targetPath;
                sourcePath = strPath + sourcePath;

                string targetFile =  targetPath + fileName;
                string sourceFile = sourcePath + fileName;

                if (!File.Exists(sourceFile))
                {
                    return File.Exists(targetFile);
                }

                if (!Directory.Exists(targetPath))
                {
                    Directory.CreateDirectory(targetPath);
                }

                File.Copy(sourceFile, targetFile, true);
                return File.Exists(targetFile);
                
            }
            return false;
        }

        public bool DeleteFile(string fileName, string filePath)
        {
            string strPath = AppDomain.CurrentDomain.BaseDirectory;
            filePath = strPath + filePath;
            string sourceFile = filePath + fileName;
            try
            {
                File.Delete(sourceFile);
            }
            catch (Exception e)
            {
                LogUtility.ErrorRoutine(e);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method validates wether is allowed to access the course or not
        /// </summary>
        /// <param name="requestedCourseId">course id to validated to allow access</param>
        /// <param name="currentUser">requested user instance</param>
        /// <returns>success flag for user permission to acces the page</returns>
        public bool AuthorizeCurrentUserForCourse( int requestedCourseId , User currentUser )
        {
            // skip check for non trainee role
            if (currentUser.IsTrainer || currentUser.IsManager || currentUser.IsAdministrator) return true;

            return LearningPathDataAccessor.AuthorizeUserForCourse(requestedCourseId, currentUser);
        }
    }
}
