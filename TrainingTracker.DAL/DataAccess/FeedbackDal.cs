using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using Feedback = TrainingTracker.Common.Entity.Feedback;
using FeedbackType = TrainingTracker.Common.Entity.FeedbackType;
using User = TrainingTracker.Common.Entity.User;

namespace TrainingTracker.DAL.DataAccess
{
    /// <summary>
    /// Data access classs for Feedback , Implements IFeedbackDal
    /// </summary>
    public class FeedbackDal : IFeedbackDal
    {
        private ISkillDal _skillDal;

        private ISkillDal SkillDal
        {
            get
            {
                return _skillDal ?? (_skillDal = new SkillDal());
            }
        }

        /// <summary>
        /// Dal method to Add feedback
        /// </summary>
        /// <param name="feedbackData">feedback instance</param>
        /// <returns>int of feedback</returns>
        public int AddFeedback(Feedback feedbackData)
        {
            try
            {
                var feedback = new EntityFramework.Feedback
                {
                    FeedbackText = feedbackData.FeedbackText.Trim() ,
                    Title = feedbackData.Title ,
                    FeedbackType = feedbackData.FeedbackType.FeedbackTypeId ,
                    ProjectId = feedbackData.Project.ProjectId ,
                    SkillId = feedbackData.Skill.SkillId ,
                    Rating = (short?) feedbackData.Rating ,
                    AddedBy = feedbackData.AddedBy.UserId ,
                    AddedFor = feedbackData.AddedFor.UserId ,
                    StartDate = feedbackData.StartDate ,
                    EndDate = feedbackData.EndDate ,
                    AddedOn = feedbackData.AddedOn == DateTime.MinValue ? DateTime.Now : feedbackData.AddedOn
                };
              
                using (var context = new TrainingTrackerEntities())
                {
                    context.Feedbacks.Add(feedback);
                    context.SaveChanges();

                    // Add new mapping for user and skill 
                    if (feedbackData.FeedbackType.FeedbackTypeId == (int) Common.Enumeration.FeedbackType.Skill)
                    {
                        SkillDal.AddUserSkillMapping(feedbackData.Skill.SkillId , feedbackData.AddedFor.UserId , feedbackData.AddedBy.UserId);
                    }
                        
                    return feedback.FeedbackId;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return 0;
            }
        }

        /// <summary>
        /// Gets all feedback for a users.
        /// </summary>
        /// <returns>List of all feedback for a users.</returns>
        public List<Feedback> GetUserFeedback(int userId, int count, int? feedbackId = null, DateTime? startAddedOn = null, DateTime? endAddedOn = null)
        {
            var feedbacks = new List<Feedback>();

            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var query = context.Feedbacks.Where(x => x.AddedFor == userId);
                    if (feedbackId != null && feedbackId > 0) query = query.Where(x => x.FeedbackType == feedbackId);
                    if (startAddedOn != null && endAddedOn != null) query = query.Where(x => ((x.StartDate > DateTime.MinValue && x.StartDate >= startAddedOn) && (x.EndDate > DateTime.MinValue && x.EndDate <= endAddedOn))
                                                                                              || ( x.AddedOn >= startAddedOn && x.AddedOn <= endAddedOn));

                    feedbacks = query.Include(x => x.User).Include(x => x.FeedbackThreads).Include(x => x.FeedbackType1)
                        .OrderByDescending(x => x.AddedOn).Take(count)
                        .Select(f => new Feedback
                        {
                            FeedbackId = f.FeedbackId,
                            FeedbackText = f.FeedbackText,
                            Title = f.Title,
                            FeedbackType = new FeedbackType
                            {
                                FeedbackTypeId = f.FeedbackType1.FeedbackTypeId,
                                Description = f.FeedbackType1.Description,
                            },
                            Rating = f.Rating == null ? 0 : (int)f.Rating,
                            AddedOn = (DateTime)f.AddedOn,
                            AddedBy = new User
                            {
                                UserId = f.User.UserId,
                                FullName = f.User.FirstName + " " + f.User.LastName,
                                ProfilePictureName = f.User.ProfilePictureName,
                            },
                            StartDate = f.StartDate ?? new DateTime(),
                            EndDate = f.EndDate ?? new DateTime(),
                            ThreadCount = f.FeedbackThreads.Count
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }
            return feedbacks;
        }

        /// <summary>
        /// Fetch feedback's threads
        /// </summary>
        /// <param name="feedbackId">feedback Id</param>
        /// <returns>List Of Threads</returns>
        public List<Threads> GetFeedbackThreads(int feedbackId)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.FeedbackThreads
                                  .Where(x => x.FeedbackId == feedbackId)
                                  .Select(x => new Threads
                                  {
                                      ThreadId = x.ThreadId,
                                      Comments = x.Comments,
                                      AddedBy = new User
                                      {
                                          UserId = x.User.UserId,
                                          FullName = x.User.FirstName + " " + x.User.LastName,
                                          ProfilePictureName = x.User.ProfilePictureName
                                      },
                                      DateInserted = x.DateTimeInserted
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
        /// Fetch feedback with threads
        /// </summary>
        /// <param name="feedbackId">feedback Id</param>
        /// <returns>Instance of Feedback</returns>
        public Feedback GetFeedbackWithThreads(int feedbackId)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    var feedback = context.Feedbacks
                        .Where(x => x.FeedbackId == feedbackId)
                        .Select(x => new Feedback
                        {
                            FeedbackId = x.FeedbackId,
                            FeedbackText = x.FeedbackText,
                            Title = x.Title,
                            FeedbackType = new FeedbackType
                            {
                                FeedbackTypeId = x.FeedbackType1.FeedbackTypeId,
                                Description = x.FeedbackType1.Description,
                            },

                            Rating = x.Rating ?? 0,
                            AddedOn = x.AddedOn ?? new DateTime(),
                            AddedBy = new User
                            {
                                UserId = x.User.UserId,
                                FullName = x.User.FirstName + " " + x.User.LastName,
                                ProfilePictureName = x.User.ProfilePictureName,
                                TeamId = x.User.TeamId
                            },
                            StartDate = x.StartDate ?? new DateTime(),
                            EndDate = x.EndDate ?? new DateTime(),
                        }).FirstOrDefault();

                    if (feedback == null) return null;

                    feedback.Threads = context.FeedbackThreads.Where(x => x.FeedbackId == feedbackId)
                                                             .Select(x => new Threads
                                                             {
                                                                 ThreadId = x.ThreadId,
                                                                 Comments = x.Comments,
                                                                 AddedBy = new User
                                                                 {
                                                                     UserId = x.User.UserId,
                                                                     FullName = x.User.FirstName + " " + x.User.LastName,
                                                                     ProfilePictureName = x.User.ProfilePictureName
                                                                 },
                                                                 DateInserted = x.DateTimeInserted
                                                             }).ToList();
                    return feedback;

                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// Add New Thread to Feedback
        /// </summary>
        /// <param name="thread">instance of thread</param>
        /// <returns>Succes event of flag</returns>
        public bool AddNewThread(Threads thread)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    context.FeedbackThreads.Add(new FeedbackThread
                    {
                        FeedbackId = thread.FeedbackId,
                        Comments = thread.Comments,
                        DateTimeInserted = DateTime.Now,
                        AddedBy = thread.AddedBy.UserId,
                    });
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
        /// Get Trainee User Id by Feedback Id
        /// </summary>
        /// <param name="feedbackId">feedbackId</param>
        /// <returns>User Id</returns>
        public int GetTraineebyFeedbackId(int feedbackId)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Feedbacks.Where(x => x.FeedbackId == feedbackId)
                            .Select(x => x.AddedFor)
                            .FirstOrDefault() ?? 0;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return 0;
            }
        }

        /// <summary>
        /// Get Feedback AddedBy Trainers
        /// </summary>
        /// <param name="userId">Trainer Id</param>
        /// <param name="count">page size</param>
        /// <param name="skip">to skip</param>
        /// <param name="feedbackId">any specefic id</param>
        /// <param name="startAddedOn">Date range start</param>
        /// <param name="endAddedOn">Date Range End</param>
        /// <returns>List Of feedback</returns>
        public List<Feedback> GetFeedbackAddedByUser(int userId, int? count = 5, int? skip = 0, int? feedbackId = null,
                                                     DateTime? startAddedOn = null, DateTime? endAddedOn = null)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    int[] allowedFeedback = {
                                                 (int) Common.Enumeration.FeedbackType.Assignment,
                                                 (int) Common.Enumeration.FeedbackType.CodeReview,
                                                 (int) Common.Enumeration.FeedbackType.Weekly
                                            };

                    return context.Feedbacks.Where(x => x.AddedBy == userId && allowedFeedback.Contains(x.FeedbackType.Value))
                                            .Select(x => new Feedback
                                            {
                                                FeedbackId = x.FeedbackId,
                                                FeedbackText = x.FeedbackText,
                                                Title = x.Title,
                                                FeedbackType = new FeedbackType
                                                {
                                                    FeedbackTypeId = x.FeedbackType1.FeedbackTypeId,
                                                    Description = x.FeedbackType1.Description,
                                                },

                                                Rating = 0,
                                                AddedOn = x.AddedOn ?? new DateTime(),
                                                AddedBy = new User
                                                {
                                                    UserId = x.User1.UserId,
                                                    FullName = x.User1.FirstName + " " + x.User1.LastName,
                                                    ProfilePictureName = x.User1.ProfilePictureName,
                                                    TeamId = x.User1.TeamId
                                                },
                                                StartDate = x.StartDate ?? new DateTime(),
                                                EndDate = x.EndDate ?? new DateTime(),
                                            }).OrderByDescending(x => x.AddedOn).Skip(skip.Value).Take(count.Value).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// fetches Trainor synopsis
        /// </summary>
        /// <param name="trainerId">trainer Id</param>
        /// <returns>instances of Trainor synopsis</returns>
        public TrainerFeedbackSynopsis GetTrainorFeedbackSynopsis(int trainerId)
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    IEnumerable<EntityFramework.Feedback> feedbacks = context.Feedbacks.Where(x => x.AddedBy == trainerId);

                    return new TrainerFeedbackSynopsis
                    {
                        AssignmentFeedbackCount = feedbacks.Count(x => x.FeedbackType == (int)Common.Enumeration.FeedbackType.Assignment),
                        CodeReviewFeedbackCount = feedbacks.Count(x => x.FeedbackType == (int)Common.Enumeration.FeedbackType.CodeReview),
                        WeeklyFeedbackCount = feedbacks.Count(x => x.FeedbackType == (int)Common.Enumeration.FeedbackType.Weekly),
                        SessionFeedbackCount = context.Sessions.Count(x => x.Presenter == trainerId),
                        SlowFeedbackCount = feedbacks.Count(x => x.Rating == (int)Common.Enumeration.FeedbackRating.Slow),
                        AverageFeedbackCount = feedbacks.Count(x => x.Rating == (int)Common.Enumeration.FeedbackRating.Average),
                        FastFeedbackCount = feedbacks.Count(x => x.Rating == (int)Common.Enumeration.FeedbackRating.Fast),
                        ExceptionalFeedbackCount = feedbacks.Count(x => x.Rating == (int)Common.Enumeration.FeedbackRating.Exceptional),
                    };
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }
    }
}