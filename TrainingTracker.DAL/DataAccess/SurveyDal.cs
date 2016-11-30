using System;
using System.Data.Entity;
using System.Linq;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using SurveySection = TrainingTracker.Common.Entity.SurveySection;

namespace TrainingTracker.DAL.DataAccess
{
    /// <summary>
    /// Data access class for Survey operation
    /// </summary>
    public class SurveyDal : ISurveyDal
    {
        /// <summary>
        /// Data Access method to get Survey Questions
        /// </summary>
        /// <param name="teamId">team id</param>
        /// <returns>Survey instance</returns>
        public Common.Entity.Survey GetWeeklySurveySetForTeam( int teamId )
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    Common.Entity.Survey survey = context.Teams.Where(x => x.TeamId == teamId)
                                               .Include(x=>x.Survey)
                                               .Include(x=>x.Survey.SurveySections)
                                               .Include(x=>x.Survey.SurveySections
                                                                   .Select(y=>y.SurveyQuestions
                                                                               .Select(z=>z.SurveyAnswers)))
                                               .AsEnumerable()
                                               .Select(x=> new Common.Entity.Survey
                                               {
                                                   SurveyId = x.Survey.SurveyId,
                                                   Description = x.Survey.Description,
                                                   
                                                   SurveySubSections = x.Survey.SurveySections
                                                                               .Where(yS=>yS.IsDeleted==false)
                                                                               .Select(y=>new SurveySection
                                                                                           {
                                                                                               Id = y.SurveySectionId,
                                                                                               Header = y.SectionHeader,
                                                                                               SortOrder = y.SortOrder,
                                                                                               Questions = y.SurveyQuestions
                                                                                                            .Where(yQ=>yQ.IsDeleted==false)
                                                                                                            .Select(q => new Common.Entity.SurveyQuestion
                                                                                                            {
                                                                                                                SurveyQuestionId = q.SurveyQuestionId,
                                                                                                                HelpText = q.HelpText,
                                                                                                                AdditionalNoteRequired = q.AdditionalNoteRequired,
                                                                                                                IsMandatory = q.IsMandatory,
                                                                                                                QuestionText = q.QuestionText,
                                                                                                                ResponseTypeId = q.ResponseTypeId,
                                                                                                                SortOrder = q.SortOrder,
                                                                                                                Answers = q.SurveyAnswers
                                                                                                                           .Where(yA => yA.IsDeleted == false)
                                                                                                                           .Select(a=>new Common.Entity.SurveyAnswer
                                                                                                                           {
                                                                                                                               Id = a.SurveyAnswerId,
                                                                                                                               OptionText = a.OptionText,
                                                                                                                               SortOrder = a.SortOrder
                                                                                                                           }).OrderBy(so=>so.SortOrder)
                                                                                                                             .ToList()
                                                                                                            }).OrderBy(so=>so.SortOrder)
                                                                                                              .ToList()
                                                                                           }).OrderBy(so=>so.SortOrder)
                                                                                             .ToList(),

                                               }).FirstOrDefault();
                    return survey;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// Data Access method to Save response for the survey
        /// </summary>
        /// <param name="response">instance of response</param>
        /// <param name="survey">instance of survey</param>
        /// <returns>id of saved feedback</returns>
        public int SaveWeeklySurveyResponseForTrainee( Common.Entity.SurveyResponse response,Common.Entity.Survey survey )
        {
              try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                   SurveyCompletedMetaData instanceMetaData= new SurveyCompletedMetaData
                                                             {
                                                                 SurveyId = survey.SurveyId,
                                                                 SurveyTakenBy = response.AddedBy.UserId,
                                                                 DateCompleted = DateTime.Now
                                                             };

                    Feedback feedback =new Feedback
                                          {
                                            AddedBy = response.Feedback.AddedBy.UserId,
                                            AddedFor = response.Feedback.AddedFor.UserId ,
                                            AddedOn = DateTime.Now,
                                            SkillId = 0,
                                            FeedbackText = response.Feedback.FeedbackText,
                                            FeedbackType =  response.Feedback.FeedbackType.FeedbackTypeId,
                                            Title=response.Feedback.FeedbackType.Description ,
                                            StartDate = response.Feedback.StartDate,
                                            EndDate = response.Feedback.EndDate,
                                            ProjectId = 0,
                                            Rating = (short?)response.Feedback.Rating                                           
                                          };

                    foreach (var ans in response.Response)
                    {
                        instanceMetaData.SurveyResponses.Add(new SurveyResponse
                        {
                            SurveyQuestionId = ans.QuestionId,
                            SurveyAnswerId = ans.AnswerId,
                            AdditionalNote = ans.AdditionalNotes,
                            DateCreated = DateTime.Now
                        });
                    }
                    context.SurveyCompletedMetaDatas.Add(instanceMetaData);
                    context.Feedbacks.Add(feedback);
                    context.WeeklyFeedbackSurveyMappings.Add(new WeeklyFeedbackSurveyMapping
                                                             {
                                                                 FeedbackId = feedback.FeedbackId,
                                                                 SurveyCompletedMetaDataId = instanceMetaData.SurveyCompletedMetaDataId
                                                             });
                     context.SaveChanges() ;
                    return feedback.FeedbackId;
                }
            }
              catch (Exception ex)
              {
                  LogUtility.ErrorRoutine(ex);
                  return -1;
              }
        }
    }
}
