
﻿using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
﻿using TrainingTracker.Common.Entity;
using TrainingTracker.DAL.Repositories;
using Course = TrainingTracker.DAL.EntityFramework.Course;

namespace TrainingTracker.DAL.RepoInterface
{
    public interface ICourseRepository : IRepository<Course>
    {
        PagedResult<Course> GetCourses(int pageNumber, int pageSize);
        bool Update(Course updatedData);
    }
}