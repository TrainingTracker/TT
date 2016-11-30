
using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using Team = TrainingTracker.Common.Entity.Team;

namespace TrainingTracker.DAL.DataAccess
{
    public class TeamDal:ITeamDal
    {
        /// <summary>
        /// interfaceimplementation signature for Fetching All teams
        /// </summary>
        /// <returns>List Of All Teams</returns>
        public List<Team> GetAllTeam()
        {
            try
            {
                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    return context.Teams.Select(x => new Team
                                                     {
                                                         TeamId = x.TeamId,
                                                         TeamName = x.TeamName
                                                     }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return null;
            }
        }

        /// <summary>
        /// interface signature for Adding New Team
        /// </summary>
        /// <returns>success event of all Team</returns>
       public bool AddNewTeam()
        {
            return false;
        }
    }
}
