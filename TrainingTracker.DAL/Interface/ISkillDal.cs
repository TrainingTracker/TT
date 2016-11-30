using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    /// <summary>
    /// Inteface for Skill Dal
    /// </summary>
    public interface ISkillDal
    {
        /// <summary>
        /// Interface method for fetching skills by user id.
        /// </summary>
        /// <param name="userId">user id</param>
        /// <returns>returns users skills</returns>
        List<Skill> GetSkillsByUserId(int userId);

        /// <summary>
        /// Interface method for fetching
        /// </summary>
        /// <returns></returns>
        List<Skill> GetAllSkillsForApp();

        List<Skill> GetSkillsWithQuestionCount();

        bool AddSkill(Skill skill);
    }
}
