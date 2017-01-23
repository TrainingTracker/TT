using System;
using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    public interface ILearningPathDal
    {
        int AddCourse(Course courseToAdd);
        int AddCourseSubtopic(CourseSubtopic subtopicToAdd);

        /// <summary>
        /// Interace signature for filtering courses on search keyword
        /// </summary>
        /// <param name="traineeId">Filter Trainee assigned Course</param>
        /// <param name="searchKeyword">search keyword for free text search</param>
        /// <returns>List of courses matching search keyword</returns>
        List<Course> FilterCourses(string searchKeyword,int traineeId=0);

        bool AddSubtopicContent(SubtopicContent dataToAdd, out int id);
        bool AddAssignment(Assignment dataToAdd, out int id);
        bool AddAssignmentSubtopicMapping(int assignmentId, int subtopicId);

        bool UpdateCourse(Course courseToUpdate);
        bool UpdateCourseSubtopic(CourseSubtopic subtopicToUpdate);
        bool UpdateSubtopicContent(SubtopicContent dataToUpdate);
        bool UpdateAssignment(Assignment dataToUpdate);
        bool UpdateAssignmentProgress(Assignment data);

        bool DeleteCourse(int id);
        bool DeleteCourseSubtopic(int id);
        bool DeleteSubtopicContent(int id);
        bool DeleteAssignment(int id);

        List<Assignment> GetAssignments(int subtopicContentId, int traineeId = 0);
        List<Course> GetAllCourses(int traineeId=0);
        List<SubtopicContent> GetSubtopicContents(int subtopicId);
        Course GetCourseWithSubtopics(int courseId);
        Course GetCourseWithAllData(int courseId, int userId = 0);

        bool SaveSubtopicOrder(List<CourseSubtopic> data);
        bool SaveSubtopicContentProgress(int subtopicContentId, int userId);
        bool SaveSubtopicContentOrder(List<SubtopicContent> data);
        bool PublishCourse(int id);

        /// <summary>
        /// Interface Signature to fetch all courses for Trainee,
        /// </summary>
        /// <param name="traineeId">user id of the trainee</param>
        /// <returns>The implementing method should return the List of Courses for the trainee,or empty list.</returns>
        List<CourseTrackerDetails> GetAllCoursesForTrainee( int traineeId );


        /// <summary>
        /// Signature for method validates whether is allowed to access the course or not
        /// </summary>
        /// <param name="requestedCourseId">course id to validated to allow access</param>
        /// <param name="currentUser">requested user instance</param>
        /// <returns>success flag for user permission to acces the page</returns>
        bool AuthorizeUserForCourse(int requestedCourseId, User currentUser);

        /// <summary>
        /// Signature for method to update the Current User's and course Mapping
        /// </summary>
        /// <param name="currentUser">Session instance of current user</param>
        /// <param name="courseId">course id to be mapped</param>
        /// <returns>Status if mapping added or not.</returns>
        /// <exception >on exception return false</exception>
        bool StartCourseForTrainee(User currentUser, int courseId);

        /// <summary>
        /// Data access method to update the course status to completed for trainee
        /// </summary>
        /// <param name="courseId">course Id to be updated</param>
        /// <param name="traineeId">trainee Id to be updated</param>
        /// <returns>success flag for updation of the context</returns>
        bool CompleteCourseForTrainee(int courseId, int traineeId);

        /// <summary>
        /// Interface signature for fetching course Detail
        /// </summary>
        /// <param name="userId">user id</param>
        /// <param name="assignmentId">assignment id</param>
        /// <param name="subtopicId">subtopic id</param>
        /// <returns>courseid</returns>
        CourseTrackerDetails GetCourseDetailBasedOnParameters(int userId, int assignmentId=0, int subtopicId=0);

    }
}
