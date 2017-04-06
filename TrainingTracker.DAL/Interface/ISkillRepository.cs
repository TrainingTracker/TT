using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    public interface ISkillRepository : IRepository<EntityFramework.Skill>
    {

        IEnumerable<Skill> GetAllSkillsForTrainee(int userId);
    }
}
