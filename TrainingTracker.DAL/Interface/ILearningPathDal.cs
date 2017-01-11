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
        /// <param name="searchKeyword">search keyword for free text search</param>
        /// <returns>List of courses matching search keyword</returns>
        List<Course> FilterCourses(string searchKeyword);

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
        List<Course> GetAllCourses();
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

    }
}
