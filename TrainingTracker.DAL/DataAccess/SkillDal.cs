using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using Skill = TrainingTracker.Common.Entity.Skill;

namespace TrainingTracker.DAL.DataAccess
{
    public class SkillDal : ISkillDal
    {
        public List<Skill> GetSkillsByUserId(int userId)
        {
            var skills = new List<Skill>();
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    skills = context.Feedbacks.Join(context.Skills, f => f.SkillId, s => s.SkillId, (f, s) => new { f, s })
                        .Where(x => x.f.AddedFor == userId)
                        .GroupBy(a => a.f.SkillId).Select(g => new { g.Key, Item = g.FirstOrDefault(), AverageRating = g.Average(x => x.f.Rating) })
                            .Select(x => new Skill
                            {
                                Name = x.Item.s.Name,
                                SkillId = x.Item.f.SkillId ?? 0,
                                Rating = (int)x.AverageRating
                            }
                        ).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }

            return skills;
        }

        public List<Skill> GetAllSkillsForApp()
        {
            var skills = new List<Skill>();
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    skills = context.Skills.OrderBy(x => x.Name).Select(x => new Skill
                    {
                        Name = x.Name,
                        SkillId = x.SkillId
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }
            return skills;
        }

        public List<Skill> GetSkillsWithQuestionCount()
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    return context.Skills.OrderBy(c => c.Name).Select(x => new Skill
                    {
                        SkillId = x.SkillId,
                        Name = x.Name,
                        Count = context.Questions.Count(q => q.SkillId == x.SkillId)
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        public bool AddSkill(Skill skill)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    context.Skills.Add(new EntityFramework.Skill
                    {
                        Name = skill.Name,
                        AddedBy = skill.AddedBy,
                        AddedOn = DateTime.Now
                    });
                    return context.SaveChanges() > 0;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }
        }

        public void AddUserSkillMapping(int skillId, int userId, int addedByUser)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var skillMapping =
                        context.UserSkillMappings.SingleOrDefault(x => x.SkillId == skillId && x.UserId == userId);
                    if (skillMapping == null) return;
                    context.UserSkillMappings.Add(new UserSkillMapping
                    {
                        SkillId = skillId,
                        AddedBy = addedByUser,
                        UserId = userId,
                        AddedOn = DateTime.Now
                    });
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }
        }

        /// <summary>
        /// Method to Add new skill to fetch Id
        /// </summary>
        /// <param name="skill">Instances data for new skill to be added</param>
        /// <exception >On exceptions rethrows the exception,need to handle in calling function</exception>
        /// <returns>Skill Id</returns>
        public int AddNewSkillForId(Skill skill)
        {
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    EntityFramework.Skill newSkill = new EntityFramework.Skill
                    {
                        Name = skill.Name ,
                        AddedBy = skill.AddedBy ,
                        AddedOn = DateTime.Now
                    };
                    context.Skills.Add(newSkill);
                    context.SaveChanges();
                    return newSkill.SkillId;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                throw ex;
            }
        }

    }
}