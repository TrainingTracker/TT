using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;


namespace TrainingTracker.BLL
{
    /// <summary>
    /// Business class for feedback, Implemets Business base
    /// </summary>
    public class FeedbackBl : BusinessBase
    {
        /// <summary>
        /// Add New feedback for user
        /// </summary>
        /// <param name="feedback">instance of feedback object</param>
        /// <returns>success flag wether feedback was added or not</returns>
        public bool AddFeedback(Feedback feedback)
        {
            feedback.Project = feedback.Project ?? new Project();

            if (feedback.FeedbackType.FeedbackTypeId == (int)Common.Enumeration.FeedbackType.Skill)
            {
                if (!string.IsNullOrEmpty(feedback.Title))
                {
                    Skill newSkill = new Skill
                    {
                        Name = feedback.Title,
                        AddedBy = feedback.AddedBy.UserId,
                        AddedOn = DateTime.Now
                    };

                    newSkill.SkillId = SkillDataAccesor.AddNewSkillForId(newSkill);

                    //Since new skill update the skill data
                    feedback.Skill.SkillId = newSkill.SkillId;
                    feedback.Skill.Name = feedback.Title;
                }

                feedback.Title = feedback.Skill.Name;
                feedback.Skill = new Skill
                {
                    SkillId = feedback.Skill.SkillId
                };
            }
            else
            {
                feedback.Skill = new Skill();
            }


            // no way comment can have feedback rating
            if (feedback.FeedbackType.FeedbackTypeId == (int)Common.Enumeration.FeedbackType.Comment || feedback.FeedbackType.FeedbackTypeId == (int)Common.Enumeration.FeedbackType.Course)
            {
                feedback.Rating = 0;
            }

            feedback.Title = string.IsNullOrEmpty(feedback.Title) ? feedback.FeedbackType.Description : feedback.Title;

            int feedbackId = FeedbackDataAccesor.AddFeedback(feedback);

            if (!(feedbackId > 0)) return false;

            feedback.FeedbackId = feedbackId;

            return new NotificationBl().AddFeedbackNotification(feedback);
        }


        /// <summary>
        /// Fetches all feedback Threads
        /// </summary>
        /// <param name="feedbackId">feedbackId</param>
        /// <param name="currentUser">current user</param>
        public List<Threads> GetFeedbackThreads(int feedbackId, User currentUser)
        {
            int feedbackForUserId = FeedbackDataAccesor.GetTraineebyFeedbackId(feedbackId);

            if (!(currentUser.IsAdministrator || currentUser.IsManager || currentUser.IsTrainer || currentUser.UserId == feedbackForUserId)) return null;

            return FeedbackDataAccesor.GetFeedbackThreads(feedbackId);
        }

        /// <summary>
        /// Fetches all feedback Threads
        /// </summary>
        /// <param name="feedbackId">feedbackId</param>
        /// <param name="currentUser">current user </param>
        public Feedback GetFeedbackWithThreads(int feedbackId, User currentUser)
        {
            int feedbackForUserId = FeedbackDataAccesor.GetTraineebyFeedbackId(feedbackId);

            if (!(currentUser.IsAdministrator || currentUser.IsManager || currentUser.IsTrainer || currentUser.UserId == feedbackForUserId)) return null;

            return FeedbackDataAccesor.GetFeedbackWithThreads(feedbackId);
        }

        /// <summary>
        /// Add New Thread to Feedback
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        public bool AddNewThread(Threads thread)
        {
            return FeedbackDataAccesor.AddNewThread(thread) && new NotificationBl().AddNewThreadNotification(thread);
        }

        /// <summary>
        /// authorize Current user for team's Feedback
        /// </summary>
        /// <param name="feedbackId">feedback Id</param>
        /// <param name="currentUser">Current User</param>
        /// <returns>Success flag</returns>
        public bool AuthorizeCurrentUserForFeedback(int feedbackId, User currentUser)
        {
            return (currentUser.IsAdministrator && !currentUser.TeamId.HasValue)
                    || FeedbackDataAccesor.GetFeedbackWithThreads(feedbackId).AddedBy.UserId == Constants.AppBotUserId
                    || FeedbackDataAccesor.GetFeedbackWithThreads(feedbackId).AddedBy.TeamId == currentUser.TeamId;
        }

        public CodeReview SubmitCodeReviewMetaData(CodeReview codeReview)
        {
            DAL.EntityFramework.CodeReviewMetaData crMetaData;
            // existing
            if (codeReview.Id > 0)
            {
                crMetaData = UnitOfWork.CodeReviewRepository.GetCodeReviewWithAllData(codeReview.Id);
                if (crMetaData == null) throw new Exception("No Record Found");

                crMetaData.Description = codeReview.Description;
                crMetaData.IsDiscarded = codeReview.IsDeleted;
                crMetaData.ProjectName = codeReview.Title;

                List<CodeReviewTag> existingTags = CodeReviewTagConverter.ConvertListFromCore(crMetaData.CodeReviewTags.Where(x=>!x.IsDeleted).ToList());

                foreach (var tag in codeReview.Tags)
                {
                    if(existingTags.Any(x => x.Skill.SkillId==tag.Skill.SkillId))
                    {
                        continue;
                    }

                    crMetaData.CodeReviewTags.Add(new DAL.EntityFramework.CodeReviewTag
                    {
                        CreatedOn = DateTime.Now,
                        SkillId = tag.Skill.SkillId
                    });
                }
            }
            else
            {
                crMetaData = new DAL.EntityFramework.CodeReviewMetaData
                                                                     {
                                                                         AddedBy = codeReview.AddedBy.UserId,
                                                                         AddedFor = codeReview.AddedFor.UserId,
                                                                         CreatedOn = DateTime.Now,
                                                                         Description = codeReview.Description,
                                                                         IsDiscarded = false,
                                                                         ProjectName = codeReview.Title,

                                                                     };
                foreach (var tag in codeReview.Tags)
                {
                    crMetaData.CodeReviewTags.Add(new DAL.EntityFramework.CodeReviewTag
                                                   {
                                                       CreatedOn = DateTime.Now,
                                                       SkillId = tag.Skill.SkillId
                                                   });
                }

                crMetaData.CodeReviewTags.Add(new DAL.EntityFramework.CodeReviewTag
                {
                    CreatedOn = DateTime.Now,
                    SkillId = null
                });

                UnitOfWork.CodeReviewRepository.Add(crMetaData);

            }

            UnitOfWork.Commit();

            //get new data in case of newly added tags
            codeReview = CodeReviewConverter.ConvertFromCore(UnitOfWork.CodeReviewRepository.GetCodeReviewWithAllData(crMetaData.CodeReviewMetaDataId));

            codeReview.CodeReviewPreviewHtml = FetchCodeReviewPreview(codeReview.Id,false);
            return codeReview;
        }

        public CodeReview SubmitCodeReviewPoint(CodeReviewPoint codeReviewPoint)
        {
            DAL.EntityFramework.CodeReviewPoint codeReviewPointCore;

            codeReviewPoint.CodeReviewTagId = codeReviewPoint.CodeReviewTagId == 0 ? null : codeReviewPoint.CodeReviewTagId;

            // existing
            if (codeReviewPoint.PointId > 0)
            {
                codeReviewPointCore = UnitOfWork.CodeReviewRepository
                                                .GetCodeReviewWithAllData(codeReviewPoint.CodeReviewMetadataId)
                                                .CodeReviewTags
                                                .First(i => i.SkillId == (codeReviewPoint.CodeReviewTagId==0?null:codeReviewPoint.CodeReviewTagId) && !i.IsDeleted)
                                                .CodeReviewPoints
                                                .First(x => x.CodeReviewPointId == codeReviewPoint.PointId);

                codeReviewPointCore.PointTitle = codeReviewPoint.Title;
                codeReviewPointCore.Description = codeReviewPoint.Description;
                codeReviewPointCore.CodeReviewPointType = codeReviewPoint.Rating;
                codeReviewPointCore.IsDeleted = codeReviewPoint.IsDeleted;
                codeReviewPointCore.ModifiedOn = DateTime.Now;
            }
            else
            {
                codeReviewPointCore = new DAL.EntityFramework.CodeReviewPoint
                                           {
                                               PointTitle = codeReviewPoint.Title,
                                               Description = codeReviewPoint.Description,
                                               CodeReviewPointType = codeReviewPoint.Rating,
                                               IsDeleted = codeReviewPoint.IsDeleted,
                                               CreatedOn = DateTime.Now,
                                               ModifiedOn = DateTime.Now
                                           };

                UnitOfWork.CodeReviewRepository.GetCodeReviewWithAllData(codeReviewPoint.CodeReviewMetadataId)
                                                 .CodeReviewTags
                                                 .First(i => i.SkillId == codeReviewPoint.CodeReviewTagId && !i.IsDeleted)
                                                 .CodeReviewPoints.Add(codeReviewPointCore);
            }

            UnitOfWork.Commit();

            //return FetchCodeReviewPreview(codeReviewPoint.CodeReviewMetadataId,false);

            CodeReview updatedCodeReviewPoint = CodeReviewConverter.ConvertFromCore(UnitOfWork.CodeReviewRepository
                                                                                              .GetCodeReviewWithAllData(codeReviewPoint.CodeReviewMetadataId));

            updatedCodeReviewPoint.CodeReviewPreviewHtml = UtilityFunctions.GenerateCodeReviewPreview(updatedCodeReviewPoint, false);

            return updatedCodeReviewPoint;

        }

        public string FetchCodeReviewPreview(int codeReviewMetaId, bool isFeedback)
        {
            return UtilityFunctions.GenerateCodeReviewPreview(CodeReviewConverter.ConvertFromCore(UnitOfWork.CodeReviewRepository
                                                                                                             .GetCodeReviewWithAllData(codeReviewMetaId)),
                                                               isFeedback);
        }

        public bool SubmitCodeReviewFeedback(CodeReview codeReview)
        {

            DAL.EntityFramework.CodeReviewMetaData codeReviewMetaData = UnitOfWork.CodeReviewRepository.GetCodeReviewWithAllData(codeReview.Id);
            CodeReview codeReviewDetailsFromCore = CodeReviewConverter.ConvertFromCore(codeReviewMetaData);

            Feedback feedback = new Feedback
            {
                AddedBy = new User { UserId = codeReview.AddedBy.UserId },
                AddedFor = new User { UserId = codeReview.AddedFor.UserId },
                FeedbackType = new FeedbackType { FeedbackTypeId = (int)Common.Enumeration.FeedbackType.CodeReview },
                FeedbackText = UtilityFunctions.GenerateCodeReviewPreview(codeReviewDetailsFromCore, true),
                Skill =new Skill(),
                Project = new Project(),
                Title = codeReviewDetailsFromCore.Title,
                Rating = codeReview.Rating
            };

            int feedbackId = FeedbackDataAccesor.AddFeedback(feedback);

            if (!(feedbackId > 0)) return false;

            codeReviewMetaData.FeedbackId = feedback.FeedbackId = feedbackId;

            UnitOfWork.Commit();

            return new NotificationBl().AddFeedbackNotification(feedback);

            //  return AddFeedback(feedback);
        }

        public bool DiscardCodeReviewFeedback(int codeReviewId)
        {
            if (codeReviewId <= 0) return false;

             DAL.EntityFramework.CodeReviewMetaData crMetaData =  UnitOfWork.CodeReviewRepository.Get(codeReviewId);

            if(crMetaData.FeedbackId.HasValue) return false;

            crMetaData.IsDiscarded = true;
            return UnitOfWork.Commit() > 0;
            
        }

        public CodeReview DiscardTagFromCodeReviewFeedback(int codeReviewId,int codeReviewTagId)
        {

            DAL.EntityFramework.CodeReviewMetaData crMetaData = UnitOfWork.CodeReviewRepository.GetCodeReviewWithAllData(codeReviewId);
           
            crMetaData.CodeReviewTags
                      .First(x => x.CodeReviewTagId == codeReviewTagId)
                      .IsDeleted = true;

            UnitOfWork.Commit();

            CodeReview updatedCodeReviewPoint = CodeReviewConverter.ConvertFromCore(UnitOfWork.CodeReviewRepository
                                                                                              .GetCodeReviewWithAllData(codeReviewId));

            updatedCodeReviewPoint.CodeReviewPreviewHtml = UtilityFunctions.GenerateCodeReviewPreview(updatedCodeReviewPoint, false);

            return updatedCodeReviewPoint;
          
        }

        /// <summary>
        ///  Private to class method to generate course 
        /// </summary>
        /// <param name="courseId">Course Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>success flag for the event</returns>
        internal bool GenerateCourseFeedback(int courseId, int userId)
        {
            Course course = LearningPathDataAccessor.GetCourseWithAllData(courseId, userId);

            Feedback feedback = new Feedback
            {
                AddedBy = new User { UserId = Constants.AppBotUserId },
                AddedFor = new User { UserId = userId },
                FeedbackType = new FeedbackType { FeedbackTypeId = (int)Common.Enumeration.FeedbackType.Course },
                FeedbackText = UtilityFunctions.GenerateHtmlForCourseFeedback(course),
                Title = course.Name
            };

            return AddFeedback(feedback);

        }
        
        public List<CodeReview> GetPrevCodeReviewDataForTrainee(int traineeId, int count)
        {
            return
                UnitOfWork.CodeReviewRepository
                          .GetPrevCodeReviewForTrainee(traineeId, count)
                          .Select(cr => new
                                        {
                                            cr.CodeReviewMetaDataId,
                                            cr.Description,
                                            cr.ProjectName,
                                            cr.IsDiscarded,
                                            cr.CreatedOn,
                                            Feedback = new
                                                       {
                                                           cr.Feedback.FeedbackId,
                                                           cr.Feedback.AddedOn,
                                                           cr.Feedback.FeedbackType,
                                                           cr.Feedback.User,
                                                           cr.Feedback.Rating
                                                       },
                                            CodeReviewTags = cr.CodeReviewTags
                                                               .Where(t => !t.IsDeleted)
                                                               .OrderBy(t => t.SkillId)
                                        })
                          .AsEnumerable()
                          .Select(cr => new CodeReview
                                        {
                                            Id = cr.CodeReviewMetaDataId,
                                            Description = cr.Description,
                                            Title= cr.ProjectName,
                                            IsDeleted = cr.IsDiscarded.GetValueOrDefault(),
                                            CreatedOn = cr.CreatedOn,
                                            Feedback = new Feedback
                                                       {
                                                           FeedbackId = cr.Feedback.FeedbackId,
                                                           AddedOn = cr.Feedback.AddedOn.GetValueOrDefault(),
                                                           FeedbackText = string.Empty,
                                                           FeedbackType =new FeedbackType{ FeedbackTypeId = cr.Feedback.FeedbackType.GetValueOrDefault()},
                                                           AddedBy= UserConverter.ConvertFromCore(cr.Feedback.User),
                                                           Rating = cr.Feedback.Rating.GetValueOrDefault()
                                                       },
                                            Tags = CodeReviewTagConverter.ConvertListFromCore( cr.CodeReviewTags
                                                               .Select(tag =>
                                                                       {
                                                                           tag.CodeReviewPoints = tag.CodeReviewPoints
                                                                                                     .Where(point => point.CodeReviewPointType != 3).ToList();
                                                                           return tag;
                                                                       })
                                                               .ToList())
                                        })
                          .ToList();

        }

    }
}