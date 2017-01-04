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
        void AddUserSkillMapping(int skillId, int userId, int addedByUser);

        /// <summary>
        /// Method to Add new skill to fetch Id
        /// </summary>
        /// <param name="skill">Instances data for new skill to be added</param>
        /// <exception >On exceptions rethrows the exception,need to handle in calling function</exception>
        /// <returns>Skill Id</returns>
        int AddNewSkillForId(Skill skill);
    }
}
