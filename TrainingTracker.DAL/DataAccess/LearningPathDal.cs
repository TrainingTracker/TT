﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using TrainingTracker.DAL.Interface;
using Course = TrainingTracker.Common.Entity.Course;
using CourseSubtopic = TrainingTracker.Common.Entity.CourseSubtopic;
using SubtopicContent = TrainingTracker.Common.Entity.SubtopicContent;
using Assignment = TrainingTracker.Common.Entity.Assignment;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.Common.Entity;


namespace TrainingTracker.DAL.DataAccess
{
    /// <summary>
    /// Class for Learning path Dal Implementation which implements ILearningPathDal interface.
    /// </summary>
    public class LearningPathDal : ILearningPathDal
    {
        ModelMapper.ModelMapper _modelMapperObject;
        ModelMapper.ModelMapper ModelMapper 
        {
            get 
            {
                return _modelMapperObject ?? (_modelMapperObject = new ModelMapper.ModelMapper());
            }
        }

        /// <summary>
        /// Add Course
        /// </summary>
        /// <param name="courseToAdd">Course data object</param>
        /// <returns>Course Id of the added course if successful otherwise 0 </returns>
        public int AddCourse(Course courseToAdd)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {

                    EntityFramework.Course newCourseEntity = ModelMapper.MapToCourseModel(courseToAdd);
                    EntityFramework.Course newCourseEntity1 = new EntityFramework.Course 
                                                             { 
                                                                 Name = courseToAdd.Name,
                                                                 Description = courseToAdd.Description,
                                                                 Icon = courseToAdd.Icon,
                                                                 AddedBy = courseToAdd.AddedBy,
                                                                 IsActive = courseToAdd.IsActive,
                                                                 CreatedOn = courseToAdd.CreatedOn,
                                                                 Duration = courseToAdd.Duration
                                                             };

                    context.Courses.Add(newCourseEntity);
                    context.SaveChanges();
                    return newCourseEntity.Id;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return 0;
            }
        }

        public bool UpdateCourse(Course courseToUpdate)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var courseEntity = context.Courses.FirstOrDefault(x => x.Id == courseToUpdate.Id);

                    if (courseEntity == null) return false;

                    courseEntity.Name = courseToUpdate.Name;
                    courseEntity.Description = courseToUpdate.Description;
                    courseEntity.Icon = courseToUpdate.Icon;
                    courseEntity.Duration = courseToUpdate.Duration;
                    context.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        /// <summary>
        /// Data access method for filtering courses on search keyword
        /// </summary>
        /// <param name="searchKeyword">search keyword for free text search</param>
        /// <returns>List of courses matching search keyword</returns>
        public List<Course> FilterCourses(string searchKeyword)
        {
             try
             {
                 // To search on multiple words
                string[] splittedKeywords = !string.IsNullOrEmpty(searchKeyword) ? searchKeyword.Split(new char[] {' ', ',', '+'}, StringSplitOptions.RemoveEmptyEntries)
                                                                            : new string[] {} ;
                 int length = splittedKeywords.Length;

                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Courses
                                  .Where(x => ( length == 0 || splittedKeywords.Any(y => x.Name.Contains(y) || x.Description.Contains(y))) 
                                                && x.IsActive )
                                  .Select(x=> new Course
                                  {
                                      Id = x.Id,
                                      Name = x.Name,
                                      Description = x.Description,
                                      Icon = x.Icon                                     
                                  }).ToList();                                           
                }
            }
             catch (Exception ex)
             {
                 LogUtility.ErrorRoutine(ex);
                 return new List<Course>();
             }
        }

        public bool DeleteCourse(int id)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var courseEntityToDelete = context.Courses.Find(id);
                    if (courseEntityToDelete == null)
                        return false;

                    courseEntityToDelete.IsActive = false;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }

        }


        public Course GetCourseWithSubtopics(int courseId) 
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var courseWithSubtopics = context.Courses
                                   .Where(c => c.IsActive && c.Id == courseId)
                                   .AsEnumerable()
                                   .Select(c =>
                                   {
                                       var course = ModelMapper.MapFromCourseModel(c);
                                       course.CourseSubtopics = c.CourseSubtopics
                                                           .Where(s => s.IsActive)
                                                           .Select(s => ModelMapper.MapFromCourseSubtopic(s))
                                                           .OrderBy(x => x.SortOrder)
                                                           .ToList();
                                       return course;
                                   })
                                    .FirstOrDefault();

                     var courseWithSubtopics1 = context.Courses
                                   .Where(c => c.IsActive && c.Id == courseId)
                                   .AsEnumerable()
                                   .Select(c => new Course
                                                {
                                                    Id = c.Id,
                                                    Name = c.Name,
                                                    Icon = c.Icon,
                                                    Description = c.Description,
                                                    AddedBy = c.AddedBy,
                                                    CreatedOn = c.CreatedOn,
                                                    IsPublished = c.IsPublished,
                                                    Duration = c.Duration,
                                                    CourseSubtopics = c.CourseSubtopics
                                                                        .Where(s => s.IsActive)
                                                                        .Select(s => new CourseSubtopic
                                                                                      {
                                                                                          Id = s.Id,
                                                                                          Name = s.Name,
                                                                                          CourseId = s.CourseId,
                                                                                          Description = s.Description,
                                                                                          AddedBy = s.AddedBy,
                                                                                          SortOrder = s.SortOrder,
                                                                                          CreatedOn = s.CreatedOn

                                                                                      })
                                                                        .OrderBy(x => x.SortOrder)
                                                                        .ToList()

                                                })
                                    .FirstOrDefault();

                     return courseWithSubtopics;
                                   
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }   
        }

        public Course GetCourseWithAllData(int courseId, int userId = 0)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var coursewithalldata = context.Courses
                                  .Where(c => c.IsActive && c.Id == courseId)
                                  .AsEnumerable()
                                  .Select(c => new Course
                                  {
                                      Id = c.Id,
                                      Name = c.Name,
                                      Icon = c.Icon,
                                      Description = c.Description,
                                      AddedBy = c.AddedBy,
                                      CreatedOn = c.CreatedOn,
                                      IsPublished = c.IsPublished,
                                      Duration = c.Duration,
                                      CourseSubtopics = c.CourseSubtopics
                                                                       .Where(s => s.IsActive)
                                                                       .Select(s => new CourseSubtopic
                                                                       {
                                                                           Id = s.Id,
                                                                           Name = s.Name,
                                                                           CourseId = s.CourseId,
                                                                           Description = s.Description,
                                                                           AddedBy = s.AddedBy,
                                                                           SortOrder = s.SortOrder,
                                                                           CreatedOn = s.CreatedOn,
                                                                           SubtopicContents = s.SubtopicContents.Where(d=>d.IsActive).Select(d => new SubtopicContent
                                                                           {
                                                                               Id = d.Id,
                                                                               CourseSubtopicId = d.CourseSubtopicId,
                                                                               Name = d.Name,
                                                                               Description = d.Description,
                                                                               Url = d.Url,
                                                                               CreatedOn = d.CreatedOn,
                                                                               IsActive = d.IsActive,
                                                                               AddedBy = d.AddedBy,
                                                                               SortOrder = d.SortOrder,
                                                                               IsCompleted = (userId == 0) ?
                                                                                                null : (bool?)(d.SubtopicContentUserMaps
                                                                                                           .Where(u => u.SubtopicContentId == d.Id && u.UserId == userId)
                                                                                                           .FirstOrDefault() != null ) 
                                                                           }).OrderBy(x => x.SortOrder)
                                                                             .ToList(),
                                                                           Assignments = GetAssignments(s.Id, userId)
                                                                       })
                                                                       .OrderBy(x => x.SortOrder)
                                                                       .ToList()

                                  })
                                   .FirstOrDefault();

                    return coursewithalldata;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        //ToDo: This function can be refactored and reused
        public List<Course> GetAllCourses()
        {
            try
            {
                using(var context = new TrainingTrackerEntities())
                {

                    var userDal = new UserDal();
                    var course = context.Courses.Where(c => c.IsActive)
                                         .AsEnumerable()
                                         .Select(c =>
                                            {
                                                var courseModel = ModelMapper.MapFromCourseModel(c);
                                                courseModel.AuthorName = userDal.GetUserById(c.AddedBy).FirstName;
                                                courseModel.AuthorMailId = userDal.GetUserById(c.AddedBy).Email;
                                                return courseModel;
                                            }
                                         ).ToList();
                     return course;
                     
                }
            }
            catch(Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }

        }

        public int AddCourseSubtopic(CourseSubtopic subtopicToAdd)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    EntityFramework.CourseSubtopic newSubtopicEntity = new EntityFramework.CourseSubtopic
                    {
                        Name = subtopicToAdd.Name,
                        CourseId = subtopicToAdd.CourseId,
                        Description = subtopicToAdd.Description,
                        SortOrder = context.CourseSubtopics.Where(c => c.CourseId == subtopicToAdd.CourseId).Count() + 1,
                        AddedBy = subtopicToAdd.AddedBy,
                        IsActive = subtopicToAdd.IsActive,
                        CreatedOn = subtopicToAdd.CreatedOn
                    };
                    context.CourseSubtopics.Add(newSubtopicEntity);
                    context.SaveChanges();
                    return newSubtopicEntity.Id;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return 0;
            }
        }

        public bool UpdateCourseSubtopic(CourseSubtopic subtopicToUpdate)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var subtopicEntityToUpdate = context.CourseSubtopics.Find(subtopicToUpdate.Id);
                    if (subtopicEntityToUpdate == null)
                        return false;

                    subtopicEntityToUpdate.Name = subtopicToUpdate.Name;
                    subtopicEntityToUpdate.Description = subtopicToUpdate.Description;
                    subtopicEntityToUpdate.SortOrder = subtopicToUpdate.SortOrder;
                    subtopicEntityToUpdate.CourseId = subtopicToUpdate.CourseId;
                    
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public bool DeleteCourseSubtopic(int id)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var subtopicEntityToDelete = context.CourseSubtopics.Find(id);
                    if (subtopicEntityToDelete == null)
                        return false;

                    subtopicEntityToDelete.IsActive = false;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public List<SubtopicContent> GetSubtopicContents(int subtopicId)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    return context.SubtopicContents
                        .Where(s => s.CourseSubtopicId == subtopicId && s.IsActive)
                            .Select(s => new SubtopicContent { 
                                        Id = s.Id,
                                        CourseSubtopicId = s.CourseSubtopicId,
                                        Name = s.Name,
                                        Description = s.Description,
                                        Url = s.Url,
                                        CreatedOn = s.CreatedOn,
                                        IsActive = s.IsActive,
                                        AddedBy = s.AddedBy,
                                        SortOrder = s.SortOrder

                                    }).OrderBy(x => x.SortOrder)
                                    .ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        public bool AddSubtopicContent(SubtopicContent dataToAdd, out int id)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    EntityFramework.SubtopicContent entityToAdd = new EntityFramework.SubtopicContent
                                                    {
                                                        Name = dataToAdd.Name,
                                                        Description = dataToAdd.Description,
                                                        CourseSubtopicId = dataToAdd.CourseSubtopicId,
                                                        Url = dataToAdd.Url,
                                                        SortOrder = context.SubtopicContents.Where(s => s.CourseSubtopicId == dataToAdd.CourseSubtopicId).Count() + 1,
                                                        AddedBy = dataToAdd.AddedBy,
                                                        CreatedOn = dataToAdd.CreatedOn,
                                                        IsActive = dataToAdd.IsActive
                                                    };
                    context.SubtopicContents.Add(entityToAdd);
                    context.SaveChanges();

                    id = entityToAdd.Id;
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                id = 0;
                return false;
            }
        }

        public bool UpdateSubtopicContent(SubtopicContent dataToUpdate)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var entityToUpdate = context.SubtopicContents.Find(dataToUpdate.Id);
                    if (entityToUpdate == null)
                        return false;

                    entityToUpdate.Name = dataToUpdate.Name;
                    entityToUpdate.Description = dataToUpdate.Description;
                    entityToUpdate.SortOrder = dataToUpdate.SortOrder;
                    entityToUpdate.Url = dataToUpdate.Url;
                    entityToUpdate.CourseSubtopicId = dataToUpdate.CourseSubtopicId;
                    
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public bool DeleteSubtopicContent(int id)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var entityToDelete = context.SubtopicContents.Find(id);
                    if (entityToDelete == null)
                        return false;

                    entityToDelete.IsActive = false;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public bool AddAssignment(Assignment dataToAdd, out int id)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    EntityFramework.Assignment newEntity = new EntityFramework.Assignment
                    {
                        Name = dataToAdd.Name,
                        Description = dataToAdd.Description,
                        AddedBy = dataToAdd.AddedBy,
                        IsActive = dataToAdd.IsActive,
                        CreatedOn = dataToAdd.CreatedOn,
                        AssignmentAsset = dataToAdd.AssignmentAsset
                    };

                    context.Assignments.Add(newEntity);
                    context.SaveChanges();
                    id = newEntity.Id;
                    AddAssignmentSubtopicMapping(id, dataToAdd.CourseSubtopicId);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                id = 0;
                return false;
            }
        }


        public bool AddAssignmentSubtopicMapping(int assignmentId, int subtopicId)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    EntityFramework.AssignmentSubtopicMap newEntity = new EntityFramework.AssignmentSubtopicMap
                    {
                        AssignmentId = assignmentId,
                        SubtopicId = subtopicId
                    };

                    context.AssignmentSubtopicMaps.Add(newEntity);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }


        public bool DeleteAssignment(int id)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var entity = context.Assignments.Find(id);
                    entity.IsActive = false;
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }


        public bool UpdateAssignment(Assignment dataToUpdate)
        {
             try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var entity = context.Assignments.Where( a => a.IsActive && a.Id == dataToUpdate.Id).FirstOrDefault();
                    entity.AssignmentAsset = dataToUpdate.AssignmentAsset;
                    entity.Description = dataToUpdate.Description;
                    entity.Name = dataToUpdate.Name;
                    
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public List<Assignment> GetAssignments(int subtopicId, int traineeId = 0)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    //var entity =
                    //     context.Assignments
                    //    .Join(context.AssignmentSubtopicMaps
                    //    .Where(x => x.SubtopicId == subtopicId), a => a.Id, s => s.AssignmentId, (a, s) =>
                    //        new Assignment
                    //        {
                    //            Name = a.Name,
                    //            Description = a.Description,
                    //            Id = a.Id,
                    //            AddedBy = a.AddedBy,
                    //            CreatedOn = a.CreatedOn,
                    //            IsActive = a.IsActive,
                    //            CourseSubtopicId = s.SubtopicId,
                    //            AssignmentAsset = a.AssignmentAsset
                    //        }).ToList().Where(x => x.IsActive).ToList();

                    return context.AssignmentSubtopicMaps
                             .Where(s => s.SubtopicId == subtopicId)
                             .Select(a => new { assignmentData = a.Assignment, traineeData = a.Assignment.AssignmentUserMaps.Where(s => s.TraineeId == traineeId && s.AssignmentId == a.Assignment.Id) })
                             .Where(a => a.assignmentData.IsActive)
                             .Select(a => new Assignment
                             {
                                 Name = a.assignmentData.Name,
                                 Description = a.assignmentData.Description,
                                 Id = a.assignmentData.Id,
                                 AddedBy = a.assignmentData.AddedBy,
                                 CreatedOn = a.assignmentData.CreatedOn,
                                 IsActive = a.assignmentData.IsActive,
                                 CourseSubtopicId = subtopicId,
                                 AssignmentAsset = a.assignmentData.AssignmentAsset,
                                 TraineeId = (int)traineeId,
                                 IsCompleted = a.traineeData.Select(b => b.IsCompleted).FirstOrDefault(),
                                 IsApproved = a.traineeData.Select(b => b.IsApproved).FirstOrDefault(),
                                 ApprovedBy = (int)a.traineeData.Select(b => b.ApprovedBy).FirstOrDefault()

                             })
                             .ToList();
                    
                   // return entity;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        
        public bool UpdateAssignmentProgress(Assignment data)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var entity = context.AssignmentUserMaps.Where(s => s.AssignmentId == data.Id && s.TraineeId == data.TraineeId).FirstOrDefault();
                   
                    if (entity == null)
                    {
                        var newEntity = new EntityFramework.AssignmentUserMap
                        {
                            IsApproved = false,
                            IsCompleted = true,
                            AssignmentId = data.Id,
                            TraineeId = data.TraineeId
                        };

                        context.AssignmentUserMaps.Add(newEntity);


                    }
                    else if (!entity.IsApproved) // do not update the data if the assignment is already updated.
                    {
                        entity.IsCompleted = data.IsCompleted;
                        entity.IsApproved = data.IsApproved;
                        entity.ApprovedBy = data.ApprovedBy;
                    }

                    context.SaveChanges();
                    return true;

                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public bool SaveSubtopicContentProgress(int subtopicContentId, int userId)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var newEntity = new EntityFramework.SubtopicContentUserMap
                    {
                        SubtopicContentId = subtopicContentId,
                        UserId = userId,
                        Seen = true,
                        CreatedOn = DateTime.Now
                    };

                    context.SubtopicContentUserMaps.Add(newEntity);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public bool SaveSubtopicOrder(List<CourseSubtopic> data)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    data.ForEach(x =>
                    {
                        var entity = context.CourseSubtopics.Find(x.Id);
                        entity.SortOrder = x.SortOrder;
                        context.SaveChanges();
                    }
                    );

                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public bool SaveSubtopicContentOrder(List<SubtopicContent> data)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    data.ForEach(x =>
                    {
                        var entity = context.SubtopicContents.Find(x.Id);
                        entity.SortOrder = x.SortOrder;
                        context.SaveChanges();
                    }
                    );

                    return true;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public bool PublishCourse(int id)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var course = context.Courses.Find(id);
                    if (course != null) {
                        course.IsPublished = true;
                        context.SaveChanges();
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

       

        /// <summary>
        /// DataAccess method to fetch the list of Curces assigned to the User
        /// ** should be modified to fetch the cource details like percentage completed,status etc..
        /// </summary>
        /// <param name="traineeId">user id of the trainee</param>
        /// <exception>All exceptions are handled to return empty List</exception>
        /// <returns>The implementing method should return the List of Courses for the trainee,or empty list.</returns>
        public List<CourseTrackerDetails> GetAllCoursesForTrainee( int traineeId )
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    //return context.LearningMaps
                    //    .Include(x => x.LearningMapCourseMappings)
                    //    .Include(x => x.LearningMapCourseMappings.Select(y => y.Course))
                    //    .Include(x => x.LearningMapUserMappings.Select(y => y.UserId == traineeId))
                    //    .Select(x => x.LearningMapCourseMappings.All(y => new Course
                    //    {
                    //        Id = y.Course.Id,
                    //        Name = y.Course.Name
                    //    }));

                    //var courseId 

                    //return context.LearningMapUserMappings
                    //                      .Where(y => y.UserId == traineeId)
                    //                      .AsEnumerable()
                    //                      .Select(y=>y.LearningMap
                    //                                  .LearningMapCourseMappings
                    //                                  .Select(z=>new Course
                    //                                           {
                    //                                               Id = z.Course.Id,
                    //                                               Name = z.Course.Name
                    //                                           }))
                    //                      .ToList();
                    //return context.Courses
                    //              .Include(x => x.LearningMapCourseMappings)
                    //              .Include(x => x.LearningMapCourseMappings.Select(y => y.LearningMap))
                    //              .Include(x => x.LearningMapCourseMappings.Select(y => y.LearningMap.LearningMapUserMappings))
                                
                    //              .Select(x => new Course
                    //              {
                    //                  Id = x.Id ,
                    //                  Name = x.Name
                    //              }).ToList();         

                    //return context.LearningMapUserMappings
                    //               .Where(x=>x.UserId==traineeId)
                    //               .Include(x=>x.LearningMap.LearningMapCourseMappings.Select(y=>y.Course))
                    //               .Select(x=> x.LearningMap.LearningMapCourseMappings.Select(y=> new Course
                    //               {
                    //                   Id=y.Course.Id,
                    //                   Name = y.Course.Name
                    //               }));   

                    //.Join(context.BranchUsers , p => p.LastUpdatedBy , b => b.UserID , ( p , b ) => new { p , b })

                    return context.Courses
                                  .Join(context.LearningMapCourseMappings,c=>c.Id,lmcm=>lmcm.CourseId,(c,lmcm)=>new {c,lmcm})
                                  .Join(context.LearningMaps,p=>p.lmcm.LearningMapId,lm=>lm.Id,(p,lm)=>new {p,lm})
                                  .Join(context.LearningMapUserMappings,q=>q.lm.Id,lmum=>lmum.LearningMapId,(q,lmum)=>new {q,lmum})
                                  .Join(context.Users,r=>r.lmum.UserId,u=>u.UserId,(r,u)=>new {r,u})
                                  .GroupJoin(context.CourseSubtopics , s => s.r.q.p.c.Id , cs => cs.CourseId , ( s , cs) => new { s , cs =  cs.FirstOrDefault() })
                                  .GroupJoin(context.SubtopicContents , t => t.cs.Id , sc => sc.CourseSubtopicId , ( t , sc ) => new { t , sc = sc.FirstOrDefault() })
                                  .GroupJoin(context.SubtopicContentUserMaps , u => u.sc.Id , scum => scum.SubtopicContentId , ( u , scum ) => new { u , scum = scum.FirstOrDefault() })
                                  .Where(x=>x.u.t.s.r.lmum.UserId == traineeId)
                                  .Select(x => new CourseTrackerDetails
                                  {
                                      Id = x.u.t.s.r.q.p.c.Id ,
                                      Name = x.u.t.s.r.q.p.c.Name ,
                                      TotalSubTopicCount = context.SubtopicContents.Count(y => y.CourseSubtopic.CourseId == x.u.t.s.r.q.p.c.Id) ,
                                      CoveredSubTopicCount = context.SubtopicContentUserMaps.Count(y => y.Seen && y.SubtopicContent.CourseSubtopic.Course.Id == x.u.t.s.r.q.p.c.Id && y.UserId == traineeId)
                                      
                                  })
                                  .ToList()
                                  .GroupBy(x=>x.Id)
                                  .Select(x=>x.First())
                                  .ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return new List<CourseTrackerDetails>();
            }
        }

        /// <summary>
        /// Signature for method validates whether is allowed to access the course or not
        /// </summary>
        /// <param name="requestedCourseId">course id to validated to allow access</param>
        /// <param name="currentUser">requested user instance</param>
        /// <returns>success flag for user permission to acces the page</returns>
        public bool AuthorizeUserForCourse( int requestedCourseId , Common.Entity.User currentUser )
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.LearningMapCourseMappings
                                  .Any(x => x.CourseId == requestedCourseId &&
                                       x.LearningMap.LearningMapUserMappings.Any(y => y.UserId == currentUser.UserId));

                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }
        
    }
}
