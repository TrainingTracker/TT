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
    public class LearningMapDal : ILearningMapDal
    {
        /// <summary>
        /// Get the Learning Map data for the given Learning Map Id along with the Course list and Trainees list associated with this Learning Map.
        /// </summary>
        /// <param name="id">Learning Map Id which is used to fetch the data.</param>
        /// <returns>Learning Map object if successfully fetch the data of given Id and returns null if no data is present or some exception occurs.</returns>
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
                                       Courses = l.LearningMapCourseMappings
                                                 .Where(s => !s.IsDeleted && s.LearningMapId == id)
                                                 .Select(s => s.Course)
                                                 //.Where(s => s.IsPublished)
                                                 .Select(c => new Course
                                                              {
                                                                  Id = c.Id,
                                                                  Name = c.Name,
                                                                  Icon = c.Icon,
                                                                  Description = c.Description,
                                                                  AddedBy = c.AddedBy,
                                                                  CreatedOn = c.CreatedOn,
                                                                  Duration = c.Duration,
                                                                  IsActive = c.IsActive,
                                                                  IsPublished = c.IsPublished,
                                                                  SortOrder = c.LearningMapCourseMappings.Where(s => !s.IsDeleted && s.LearningMapId == id).Select(x => x.SortOrder).FirstOrDefault()
                                                              })
                                                  .OrderBy(c => c.SortOrder).ToList(),
                                       Trainees = l.LearningMapUserMappings
                                                   .Where(s => s.LearningMapId == id)
                                                   .Select(s => s.User).Where(s => s.IsActive == true && (bool)s.IsTrainee)
                                                   .Select(x => new User
                                                                {
                                                                    UserId = x.UserId,
                                                                    FirstName = x.FirstName,
                                                                    LastName = x.LastName,
                                                                    FullName = x.FirstName + " " + x.LastName,
                                                                    UserName = x.UserName,
                                                                    Email = x.Email,
                                                                    Designation = x.Designation,
                                                                    ProfilePictureName = x.ProfilePictureName,
                                                                    IsFemale = x.IsFemale ?? false,
                                                                    IsTrainee = x.IsTrainee ?? false,
                                                                    IsActive = x.IsActive ?? false,
                                                                    DateAddedToSystem = x.DateAddedToSystem,
                                                                    TeamId = x.TeamId
                                                                    
                                                                })
                                                   .ToList()


                                   })
                                    .FirstOrDefault();
                    return data;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }

        }

        /// <summary>
        /// Get all the Learning Maps that belong to a specified team.
        /// </summary>
        /// <param name="teamId">teamId which is used to fetch the Learning map data of that team only.</param>
        /// <returns>List of Learning Map object if successfully fetches some data and returns null if no data is present or some exception occurs. </returns>
        public List<LearningMap> GetAllLearningMaps(int teamId)
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

        /// <summary>
        /// Get all the Trainees List belong to a specified team.
        /// </summary>
        /// <param name="teamId">teamId which is used to fetch the Trainees data of that team only.</param>
        /// <returns>List of User object if successfully fetches some data and returns null if no data is present or some exception occurs. </returns>
        public List<User> GetAllTrainees(int teamId) 
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    return context.Users.Where(l => l.IsActive == true && l.TeamId == teamId && l.IsTrainee == true)
                        .Select(x => new User
                        {
                            UserId = x.UserId,
                            FirstName = x.FirstName,
                            LastName = x.LastName,
                            FullName = x.FirstName + " " + x.LastName,
                            UserName = x.UserName,
                            Email = x.Email,
                            Designation = x.Designation,
                            ProfilePictureName = x.ProfilePictureName,
                            IsFemale = x.IsFemale ?? false,
                            IsAdministrator = x.IsAdministrator ?? false,
                            IsTrainer = x.IsTrainer ?? false,
                            IsTrainee = x.IsTrainee ?? false,
                            IsManager = x.IsManager ?? false,
                            IsActive = x.IsActive ?? false,
                            DateAddedToSystem = x.DateAddedToSystem,
                            TeamId = x.TeamId
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

        /// <summary>
        /// Add Learning Map data along with the mapping details of all courses and trainees included in the Learning Map(if any).
        /// </summary>
        /// <param name="data">Learning Map object which contain Learning map data along with the Course list(if any) and Trainee
        /// list(if any) included in the Learning map</param>
        /// <returns>Id of the Learning map if successfully added otherwise zero</returns>
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
                        DateCreated = DateTime.Now,
                        IsCourseRestricted = data.IsCourseRestricted,
                        TeamId = data.TeamId,
                        CreatedBy = data.CreatedBy,
                        IsDeleted = data.IsDeleted,
                        LearningMapCourseMappings = (data.Courses != null) ? data.Courses.Select(x => new LearningMapCourseMapping
                                                                            {
                                                                                //LearningMapId = newLearningMapEntity.Id,
                                                                                CourseId = x.Id,
                                                                                SortOrder = x.SortOrder,
                                                                                IsDeleted = false,
                                                                                DateInserted = DateTime.Now
                                                                            }).ToList()
                                                                            : null,

                        LearningMapUserMappings = (data.Trainees != null) ? data.Trainees.Select(x => new LearningMapUserMapping
                                                    {
                                                        //LearningMapId = newLearningMapEntity.Id,
                                                        UserId = x.UserId,
                                                        DateInserted = DateTime.Now
                                                    }).ToList()
                                                    : null

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


        //void AddTraineesInLearningMap(List<User> traineeList, TrainingTrackerEntities context)
        //{
        //    traineeList.ForEach(x => context.LearningMapUserMappings
        //                                    .Add(new LearningMapUserMapping
        //                                            {
        //                                                UserId = x.UserId,
        //                                                DateInserted = DateTime.Now
        //                                            }
        //                                        ));
        //}

        /// <summary>
        /// Updates Learning Map data along with the mapping details of all courses included in the Learning Map(if any) and 
        /// Adds mapping details of new Trainees added in the Learning Map(if any) 
        /// </summary>
        /// <param name="data">Learning Map object contains Learning map data with Course list(if any) and newly added Trainee list(if any).
        /// Trainee list should contains only those trainees which is newly added </param>
        /// <returns>True if Learning Map data along with the Course and User mapping data is updated successfully otherwise false</returns>
        public bool UpdateLearningMap(LearningMap data)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var learningMapEntity = context.LearningMaps.First(x => x.Id == data.Id);

                    if (learningMapEntity == null) return false;

                    learningMapEntity.Title = data.Title;
                    learningMapEntity.Notes = data.Notes;
                    learningMapEntity.Duration = data.Duration;
                    learningMapEntity.IsCourseRestricted = data.IsCourseRestricted;

                    // Removing All courses from current learning map
                    context.LearningMapCourseMappings.RemoveRange(learningMapEntity.LearningMapCourseMappings);

                    // Adding Updated list of courses in the current learning map
                    if (data.Courses != null) // checking if all the courses are removed
                    { 
                        var courseMappingEntity = data.Courses.Select(x => new LearningMapCourseMapping
                                                        {
                                                            LearningMapId = data.Id,
                                                            CourseId = x.Id,
                                                            SortOrder = x.SortOrder,
                                                            IsDeleted = false,
                                                            DateInserted = DateTime.Now
                                                        }).ToList();
                        context.LearningMapCourseMappings.AddRange(courseMappingEntity);
                    }

                    // Trainees can only be added
                    if(data.Trainees != null)
                    {
                        var newTraineesMapping = data.Trainees.Select(x => new LearningMapUserMapping
                                                   {
                                                       LearningMapId = data.Id,
                                                       UserId = x.UserId,
                                                       DateInserted = DateTime.Now
                                                   }).ToList();
                        context.LearningMapUserMappings.AddRange(newTraineesMapping);
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

        /// <summary>
        /// Delete Learning Map data by making its IsDeleted attribute to true.
        /// </summary>
        /// <param name="id">Learning Map Id whose data is to be deleted</param>
        /// <returns>true if Learning Map data is deleted successfully and false if data to be deleted is not found or some other exception occurs.</returns>
        public bool DeleteLearningMap(int id) 
        {

            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var learningMapEntity = context.LearningMaps.First(x => x.Id == id);

                    if (learningMapEntity == null) return false;

                    learningMapEntity.IsDeleted = true;
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

        //void UpdateCoursesOfLearningMap(List<Course> courseList, int learningMapId, TrainingTrackerEntities context)
        //{

        //    courseList.ForEach(x => context.LearningMapCourseMappings
        //                                    .Add(new LearningMapCourseMapping
        //                                    {
        //                                        CourseId = x.Id,
        //                                        SortOrder = x.SortOrder,
        //                                        IsDeleted = !x.IsActive,
        //                                    }
        //                                ));
        //}

    }
}
