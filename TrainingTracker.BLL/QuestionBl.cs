using System;
using System.Collections.Generic;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.ViewModel;

namespace TrainingTracker.BLL
{
    /// <summary>
    /// Business class for Question, Inherits business base 
    /// </summary>
    public class QuestionBl : BussinessBase
    {
        /// <summary>
        /// Fetches Question By skill and Experience
        /// </summary>
        /// <param name="skillId">skill id</param>
        /// <param name="startExperience">Start Experience</param>
        /// <param name="endExperience">End Experience</param>
        /// <returns>List Of Questions</returns>
        public List<Question> GetQuestionsBySkillAndExperience(int skillId, int startExperience, int endExperience)
        {
            return QuestionDataAccesor.GetQuestionsBySkillAndExperience(skillId, startExperience, endExperience);
        }

        /// <summary>
        /// Fetches Questions by Skill and User Id
        /// </summary>
        /// <param name="skillId">skill Id</param>
        /// <param name="userId">User Id</param>
        /// <returns>List Of Questions</returns>
        public List<Question> GetQuestionsBySkillAndUserId(int skillId, int userId)
        {
            return QuestionDataAccesor.GetQuestionsBySkillAndUserId(skillId, userId);
        }

        /// <summary>
        /// Fetches Questions By User Id
        /// </summary>
        /// <param name="userId">User Id</param>
        /// <returns>List Of Questions</returns>
        public List<Question> GetQuestionsByUserId(int userId)
        {
            return QuestionDataAccesor.GetQuestionsByUserId(userId);
        }

        /// <summary>
        /// Insert New Questions
        /// </summary>
        /// <param name="question">Instacne of Question</param>
        /// <returns>Success flag for Insertion success</returns>
        public bool AddQuestion(Question question)
        {
            question.AddedDate = DateTime.Now;
            question.Description = question.Description ?? string.Empty;
            return QuestionDataAccesor.AddQuestion(question);
        }


        /// <summary>
        /// Insert New Skills
        /// </summary>
        /// <param name="skill">Instance of Skill Entity</param>
        /// <returns>Success flag for Insertion</returns>
        public bool AddSkill(Skill skill)
        {
            return SkillDataAccesor.AddSkill(skill);
        }

        /// <summary>
        /// Get Question Vm 
        /// </summary>
        /// <returns>Instance of QuestionVm</returns>
        public QuestionVm GetQuestionVm()
        {
            return new QuestionVm
            {
                Categories = SkillDataAccesor.GetSkillsWithQuestionCount(),
                Questions = new List<Question>()
            };
        }
    }
}
