﻿using System;
using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    public interface ILearningPathDal
    {
        int AddCourse(Course courseToAdd);
        int AddCourseSubtopic(CourseSubtopic subtopicToAdd);
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
        List<Course> GetAllCoursesWithSubtopics();
        List<SubtopicContent> GetSubtopicContents(int subtopicId);
        
        
        
    }
}
