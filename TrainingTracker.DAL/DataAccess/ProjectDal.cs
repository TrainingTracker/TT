using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.DataAccess
{
    public class ProjectDal:IProjectDal
    {
        public List<Project> GetProjectsByUserId(int userId)
        {
            var projects = new List<Project>();

            var prms = new List<SqlParameter>
            {
                SqlUtility.CreateParameter(SPGetProjectsByUserID.PARAM_USER_ID, SqlDbType.Int, userId)
            };

            try
            {
                var dt = SqlUtility.ExecuteAndGetTable(SPGetProjectsByUserID.NAME,
                    CommandType.StoredProcedure, SPGetProjectsByUserID.TABLE_NAME, prms);

                projects.AddRange(from DataRow row in dt.Rows
                                  select new Project
                                  {
                                      Title = row["Title"].ToString(),
                                      ProjectId = Convert.ToInt32(row["ProjectId"]),
                                      UserProjectId = Convert.ToInt32(row["UserProjectId"])
                                  });
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }

            return projects;
        }

    }
}