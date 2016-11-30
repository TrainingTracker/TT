
using System.Collections.Generic;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.DAL.Interface
{
    /// <summary>
    /// Interace class for Team and user Team notification Mapping
    /// </summary>
     public  interface ITeamDal
    {
         /// <summary>
         /// interface signature for Fetching All teams
         /// </summary>
         /// <returns>List Of All Teams</returns>
        List<Team> GetAllTeam();

         /// <summary>
         /// interface signature for Adding New Team
         /// </summary>
         /// <returns>success event of all Team</returns>
        bool AddNewTeam();
    }
}
