using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using Skill = TrainingTracker.Common.Entity.Skill;
using User = TrainingTracker.Common.Entity.User;
using Question = TrainingTracker.Common.Entity.Question;
using QuestionLevelMapping = TrainingTracker.Common.Entity.QuestionLevelMapping;

namespace TrainingTracker.DAL.DataAccess
{
    public class QuestionDal : IQuestionDal
    {
        public List<Question> GetQuestionsBySkillAndExperience(int skillId, int startExperience, int endExperience)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    return context.Questions.Join(context.QuestionLevelMappings, x => x.Id, m => m.QuestionId,
                        (x, m) => new { q = x, mp = m }).
                        Where(a => a.q.SkillId == skillId && a.mp.ExperienceStartRange >= startExperience
                            && a.mp.ExperienceEndRange <= endExperience).
                            Select(x => new Question
                            {
                                Id = x.q.Id,
                                QuestionText = x.q.QuestionText,
                                Description = x.q.Description,
                                SkillId = x.q.SkillId,
                                Level = x.mp.Level
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        public List<Question> GetQuestionsBySkillAndUserId(int skillId, int userId)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    return context.Questions.
                        Where(x => x.SkillId == skillId && x.AddedBy == userId).
                            Select(x => new Question
                            {
                                Id = x.Id,
                                QuestionText = x.QuestionText,
                                Description = x.Description,
                                SkillId = x.SkillId,
                                AddedBy = x.AddedBy,
                                AddedDate = x.AddedDate
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        public List<Question> GetQuestionsByUserId(int userId)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    return context.Questions.
                        Where(x => x.AddedBy == userId).
                            Select(x => new Question
                            {
                                Id = x.Id,
                                QuestionText = x.QuestionText,
                                Description = x.Description,
                                SkillId = x.SkillId,
                                AddedBy = x.AddedBy,
                                AddedDate = x.AddedDate
                            }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        public bool AddQuestion(Question question)
        {
            try
            {
                var newQuestion = new EntityFramework.Question
                {
                    QuestionText = question.QuestionText,
                    Description = question.Description,
                    SkillId = question.SkillId,
                    AddedBy = question.AddedBy,
                    AddedDate = question.AddedDate
                };
                using (var context = new TrainingTrackerEntities())
                {
                    context.Questions.Add(newQuestion);
                    context.SaveChanges();
                }
                return AddQuestionMapping(question.LevelMapping, newQuestion.Id);
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public bool AddQuestionMapping(List<QuestionLevelMapping> mappings, int questionId)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    foreach (var questionLevelMapping in mappings)
                    {
                        context.QuestionLevelMappings.Add(new EntityFramework.QuestionLevelMapping
                        {
                            QuestionId =questionId,
                            Level = questionLevelMapping.Level,
                            ExperienceStartRange = questionLevelMapping.ExperienceStartRange,
                            ExperienceEndRange = questionLevelMapping.ExperienceEndRange
                        });
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
    }
}
