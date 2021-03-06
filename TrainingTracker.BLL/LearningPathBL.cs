﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;
using System.Linq;

namespace TrainingTracker.BLL
{
    public class LearningPathBL : BusinessBase
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
        /// <param name="currentUser">Current user instance</param>
        /// <param name="searchKeyword">search keyword for free text search</param>
        /// <returns>List of Courses on filter</returns>
        public List<Course> FilterCourses(User currentUser, string searchKeyword)
        {
            return currentUser.IsTrainee ? LearningPathDataAccessor.FilterCourses(searchKeyword , currentUser.UserId)
                                         : LearningPathDataAccessor.FilterCourses(searchKeyword);
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


        public bool UpdateAssignmentProgress(Assignment data, User currentUser, out int feedbackId)
        {
            feedbackId = 0;
            bool isAssignmentFeedback = false ;

            if (data != null && data.TraineeId > 0)
            {
                // return false if trainee will not allowed to approve the completion of assignment or trainer cannot mark assignment as completed or trainer cannot approve/reassign assignment without feedback.
                if ((currentUser.IsTrainee && data.TraineeId != currentUser.UserId && data.IsApproved) || (!currentUser.IsTrainee && data.IsCompleted && !data.IsApproved) || (!currentUser.IsTrainee && data.Feedback.Count < 1))
                {
                    return false;
                }

                if (!currentUser.IsTrainee)
                {
                    var feedback = data.Feedback.FirstOrDefault(x => x.FeedbackId == 0);
                    feedback.AddedBy = currentUser;
                    feedback.AddedFor = new UserBl().GetUserByUserId(data.TraineeId);
                    feedback.AddedOn = DateTime.Now;
                    
                    if (feedback.FeedbackType.FeedbackTypeId == (int)Common.Enumeration.FeedbackType.Comment)
                    {
                        feedback.Rating = 0;
                    }
                    feedback.Skill = new Skill();
                    feedback.Project = new Project();

                    feedbackId = FeedbackDataAccesor.AddFeedback(feedback);    
                    if(feedbackId == 0 || !FeedbackDataAccesor.AddFeedbackAssignmentMapping(feedbackId, data.Id))
                        return false;

                    feedback.FeedbackId = feedbackId;

                    new NotificationBl().AddFeedbackNotification(feedback);
                    data.ApprovedBy = currentUser.UserId;

                    if (feedback.FeedbackType.FeedbackTypeId == (int) Common.Enumeration.FeedbackType.Assignment)
                    {
                        isAssignmentFeedback = true;
                        
                    }
                }
                
                var status = LearningPathDataAccessor.UpdateAssignmentProgress(data);

                if (!status || !isAssignmentFeedback || currentUser.IsTrainee) return status;

                CourseTrackerDetails courseDetails = LearningPathDataAccessor.GetCourseDetailBasedOnParameters(data.TraineeId, data.Id);
                  
                if( courseDetails != null && courseDetails.PercentageCompleted.CompareTo(100) == 0)
                {
                    return LearningPathDataAccessor.CompleteCourseForTrainee(courseDetails.Id, data.TraineeId)
                          && new FeedbackBl().GenerateCourseFeedback(courseDetails.Id, data.TraineeId);
                }
                return true;
            }
            return false;
        }

        public bool DeleteAssignment(int id)
        {
            return LearningPathDataAccessor.DeleteAssignment(id);
        }


        public List<Course> GetAllCourses(User currentUser)
        {
            return currentUser.IsTrainee ? LearningPathDataAccessor.GetAllCourses(currentUser.UserId) 
                                         : LearningPathDataAccessor.GetAllCourses();
        }

        public Course GetCourseWithSubtopics(int courseId)
        {
            var courseDetails = LearningPathDataAccessor.GetCourseWithSubtopics(courseId);
            if (courseDetails != null)
            {
                courseDetails.AuthorName = UserDataAccesor.GetUserById(courseDetails.AddedBy).FirstName;
            }
            return courseDetails;
        }
        public Course GetCourseWithAllData(int courseId,User currentUser, int userId = 0)
        {
            Course courseDetails;

            if (!currentUser.IsTrainee)
            {
                if (userId > 0)
                {
                    courseDetails = LearningPathDataAccessor.GetCourseWithAllData(courseId , userId);
                    courseDetails.TrackerDetails = LearningPathDataAccessor.GetAllCoursesForTrainee(userId).FirstOrDefault(x => x.Id == courseId);
                }
                else
                {
                    courseDetails = LearningPathDataAccessor.GetCourseWithAllData(courseId);
                }
            }
            else
            {
                courseDetails = LearningPathDataAccessor.GetCourseWithAllData(courseId , userId);
            }

            if (courseDetails != null)
            {
                User userData = UserDataAccesor.GetUserById(courseDetails.AddedBy);
                courseDetails.AuthorName = userData.FirstName;
                courseDetails.AuthorMailId = userData.Email;

                if (currentUser.IsTrainee && !courseDetails.IsStarted)
                {
                    courseDetails.LoadAlert = true;
                    foreach (CourseSubtopic  subtopic in courseDetails.CourseSubtopics)
                    {
                        subtopic.Assignments = new Collection<Assignment>();
                        subtopic.SubtopicContents = new Collection<SubtopicContent>();
                    }
                }
            }           
            return courseDetails;
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

       
        public bool SaveSubtopicContentProgress(int subtopicContentId, int userId)
        {
            if ((subtopicContentId > 0 && userId > 0) && LearningPathDataAccessor.SaveSubtopicContentProgress(subtopicContentId, userId))
            {
                CourseTrackerDetails courseDetails = LearningPathDataAccessor.GetCourseDetailBasedOnParameters(userId, 0,subtopicContentId);
                if (courseDetails != null && courseDetails.PercentageCompleted.CompareTo(100) == 0)
                {
                  return   LearningPathDataAccessor.CompleteCourseForTrainee(courseDetails.Id, userId) 
                          && new FeedbackBl().GenerateCourseFeedback(courseDetails.Id, userId);
                }
                return true;
            }
            return false;
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

        /// <summary>
        /// Update the Current User's and course Mapping
        /// </summary>
        /// <param name="currentUser">Session instance of current user</param>
        /// <param name="courseId">course id to be mapped</param>
        /// <returns>Status if mapping added or not.</returns>
        /// <exception >on exception return false</exception>
        public bool StartCourseForTrainee( User currentUser , int courseId )
        {
            if (currentUser != null && currentUser.IsTrainee && currentUser.UserId > 0 && courseId > 0)
            {
                return LearningPathDataAccessor.StartCourseForTrainee(currentUser, courseId);
            }
            return false;
        }       
    }
}
