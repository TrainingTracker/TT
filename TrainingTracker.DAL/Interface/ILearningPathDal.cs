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

        bool DeleteCourse(int id);
        bool DeleteCourseSubtopic(int id);
        bool DeleteSubtopicContent(int id);
        bool DeleteAssignment(int id);

        List<Assignment> GetAssignments(int subtopicContentId);
        List<Course> GetAllCourses();
        List<SubtopicContent> GetSubtopicContents(int subtopicId);
        Course GetCourseWithSubtopics(int courseId);
        Course GetCourseWithAllData(int courseId, int userId = 0);

        bool SaveSubtopicOrder(List<CourseSubtopic> data);
        bool SaveSubtopicContentProgress(int subtopicContentId, int userId);
        bool SaveSubtopicContentOrder(List<SubtopicContent> data);
        bool PublishCourse(int id);
        
    }
}
