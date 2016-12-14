using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.DAL.Interface;
using Course = TrainingTracker.Common.Entity.Course;
using LearningMap = TrainingTracker.Common.Entity.LearningMap;
using User = TrainingTracker.Common.Entity.User;
using TrainingTracker.Common.Utility;
using TrainingTracker.Common.Constants;
using TrainingTracker.DAL.EntityFramework;


namespace TrainingTracker.DAL.DataAccess
{
    public class LearningMapDal
    {
        //List<Course> GetCoursesOfLearningMap (int id)
        //{
        //     try
        //    {
        //        using(var context = new TrainingTrackerEntities())
        //        {
        //            return context.LearningMapCourseMapppings.Where(l => !l.IsDeleted && l.LearningMapId == id)
        //                                        .Select(l => l.Course)
        //                                        .Select(c => new Course
        //                                                     {
        //                                                         Id = c.Id,
        //                                                         Name = c.Name,
        //                                                         Icon = c.Icon,
        //                                                         Description = c.Description,
        //                                                         AddedBy = c.AddedBy,
        //                                                         CreatedOn = c.CreatedOn,
        //                                                         Duration = c.Duration

        //                                                     }).ToList();


        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        LogUtility.ErrorRoutine(ex);
        //        return null;
        //    }

        //}


        //List<Course> GetTraineesOfLearningMap()
        //{
        //    return null;
        //}

        public LearningMap GetLearningMapWithAllData(int id)
        {

            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var data = context.LearningMaps
                                   .Where(l => !l.IsDeleted && l.Id == id)
                                   .AsEnumerable()
                                   .Select(l => new LearningMap
                                   {
                                       Id = l.Id,
                                       Title = l.Title,
                                       Notes = l.Notes,
                                       Duration = l.Duration,
                                       IsCourseRestricted = l.IsCourseRestricted,
                                       TeamId = l.TeamId,
                                       IsDeleted = l.IsDeleted,
                                       CreatedBy = l.CreatedBy,
                                       DateCreated = l.DateCreated,
                                       Courses = l.LearningMapCourseMapppings
                                                 .Where(s => !s.IsDeleted && s.LearningMapId == id)
                                                 .Select(s => s.Course)
                                                 .Select(c => new Course
                                                              {
                                                                  Id = c.Id,
                                                                  Name = c.Name,
                                                                  Icon = c.Icon,
                                                                  Description = c.Description,
                                                                  AddedBy = c.AddedBy,
                                                                  CreatedOn = c.CreatedOn,
                                                                  Duration = c.Duration

                                                              })
                                                  .ToList(),
                                       Trainees = l.LearningMapUserMapppings
                                                   .Where(s => s.LearningMapId == id)
                                                   .Select(s => s.User).Where(s => s.IsActive == true && s.Designation.Equals(UserRoles.Trainee))
                                                   .Select(c => new User
                                                                {
                                                                    UserId = c.UserId,
                                                                    UserName = c.UserName,
                                                                    FullName = c.FirstName + c.LastName,

                                                                }).ToList()


                                   })
                                    .FirstOrDefault();

                    var dataToAdd = new LearningMap
                    {
                        Title = "Test",
                        Notes = "Tes",
                        Duration = 1,
                        DateCreated = DateTime.Now,
                        IsCourseRestricted = false,
                        TeamId = 1,
                        CreatedBy = 1,
                        IsDeleted = false,
                        Courses = new List<Course>
                                    {
                                        new Course
                                        {
                                            Id = 1,
                                            SortOrder = 0,
                                            IsActive = true,
                                            CreatedOn= DateTime.Now
                                        }
                                    },

                        Trainees = new List<User>()
                                    {
                                        new User
                                        {
                                            UserId = 1,
                                            DateAddedToSystem = DateTime.Now
                                        }
                                    }

                    };
                    var x = AddLearningMap(dataToAdd);
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }

        }


        List<LearningMap> GetAllLearningMaps(int teamId)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    return context.LearningMaps.Where(l => !l.IsDeleted && l.TeamId == teamId)
                        .Select(l => new LearningMap
                                {
                                    Id = l.Id,
                                    Title = l.Title,
                                    Notes = l.Notes,
                                    Duration = l.Duration,
                                    IsCourseRestricted = l.IsCourseRestricted,
                                    TeamId = l.TeamId,
                                    IsDeleted = l.IsDeleted,
                                    CreatedBy = l.CreatedBy,
                                    DateCreated = l.DateCreated,
                                })
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }


        //List<User> GetAllTrainees(int ?teamId) 
        //{
        //    try
        //    {
        //        using (var context = new TrainingTrackerEntities())
        //        {
        //            return context.Users.Where(l => l.IsActive && (l.TeamId == teamId))
        //                .Select(l => new LearningMap
        //                {
        //                    Id = l.Id,
        //                    Title = l.Title,
        //                    Notes = l.Notes,
        //                    Duration = l.Duration,
        //                    IsCourseRestricted = l.IsCourseRestricted,
        //                    TeamId = l.TeamId,
        //                    IsDeleted = l.IsDeleted,
        //                    CreatedBy = l.CreatedBy,
        //                    DateCreated = l.DateCreated,
        //                })
        //                .ToList();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogUtility.ErrorRoutine(ex);
        //        return null;
        //    }
        //}


        //List<Course> GetAllCourses() 
        //{

        //    return null;
        //}


        public int AddLearningMap(LearningMap data)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var newLearningMapEntity = new EntityFramework.LearningMap
                    {
                        Title = data.Title,
                        Notes = data.Notes,
                        Duration = data.Duration,
                        DateCreated = data.DateCreated,
                        IsCourseRestricted = data.IsCourseRestricted,
                        TeamId = data.TeamId,
                        CreatedBy = data.CreatedBy,
                        IsDeleted = data.IsDeleted,
                        LearningMapCourseMapppings = data.Courses.Select(x => new LearningMapCourseMappping
                                                    {
                                                        //LearningMapId = newLearningMapEntity.Id,
                                                        CourseId = x.Id,
                                                        SortOrder = x.SortOrder,
                                                        IsDeleted = !x.IsActive,
                                                        DateInserted = DateTime.Now
                                                    }).ToList(),

                        LearningMapUserMapppings = data.Trainees.Select(x => new LearningMapUserMappping
                                                    {
                                                        //LearningMapId = newLearningMapEntity.Id,
                                                        UserId = x.UserId,
                                                        DateInserted = DateTime.Now
                                                    }).ToList()

                    };
                    context.LearningMaps.Add(newLearningMapEntity);

                    context.SaveChanges();
                    return newLearningMapEntity.Id;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return 0;
            }

        }


        void AddTraineesInLearningMap(List<User> traineeList, TrainingTrackerEntities context)
        {
            traineeList.ForEach(x => context.LearningMapUserMapppings
                                            .Add(new LearningMapUserMappping
                                                    {
                                                        UserId = x.UserId,
                                                        DateInserted = DateTime.Now
                                                    }
                                                ));
        }


        //bool UpdateLearningMap(LearningMap data)
        //{
        //    return true;
        //}


        void UpdateCoursesOfLearningMap(List<Course> courseList, int learningMapId, TrainingTrackerEntities context)
        {

            courseList.ForEach(x => context.LearningMapCourseMapppings
                                            .Add(new LearningMapCourseMappping
                                            {
                                                CourseId = x.Id,
                                                SortOrder = x.SortOrder,
                                                IsDeleted = !x.IsActive,
                                            }
                                        ));
        }

    }
}
