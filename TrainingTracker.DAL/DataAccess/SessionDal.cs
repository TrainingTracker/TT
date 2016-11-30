using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.DAL.DataAccess
{
    /// <summary>
    /// Dataaccess class for session, Implementing IDal
    /// </summary>
    public class SessionDal : ISessionDal
    {
        /// <summary>
        /// fetch Session based authored by User Id
        /// </summary>
        /// <param name="userId">userid of presenter</param>
        /// <returns>List of session</returns>
        public List<Common.Entity.Session> GetSessionsByUserId(int userId)
        {
            var sessions = new List<Common.Entity.Session>();

            var prms = new List<SqlParameter>
            {
                SqlUtility.CreateParameter(SPGetSessionsByUserID.PARAM_USER_ID, SqlDbType.Int, userId)
            };

            try
            {
                var dt = SqlUtility.ExecuteAndGetTable(SPGetSessionsByUserID.NAME,
                    CommandType.StoredProcedure, SPGetSessionsByUserID.TABLE_NAME, prms);

                sessions.AddRange(from DataRow row in dt.Rows
                                  select new Common.Entity.Session
                                  {
                                      Title = row["Title"].ToString()
                                  });
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }

            return sessions;
        }

        /// <summary>
        /// Inteface method to fetch sessions by User id
        /// </summary>
        /// <param name="objSession">User Id</param>
        /// <returns>List Of session</returns>
        public int AddEditSessions(Common.Entity.Session objSession)
        {
            var prms = new List<SqlParameter>
            {
                SqlUtility.CreateParameter(SPAddEditSession.PARAM_PRESENTER, SqlDbType.NVarChar, objSession.Presenter),
                SqlUtility.CreateParameter(SPAddEditSession.PARAM_ID, SqlDbType.Int, objSession.Id),
                SqlUtility.CreateParameter(SPAddEditSession.PARAM_TITLE, SqlDbType.NVarChar, objSession.Title),
                SqlUtility.CreateParameter(SPAddEditSession.PARAM_DESCRIPTION, SqlDbType.NVarChar, objSession.Description),
                SqlUtility.CreateParameter(SPAddEditSession.PARAM_DATE, SqlDbType.DateTime, objSession.Date),
                SqlUtility.CreateParameter(SPAddEditSession.PARAM_ATTENDEES, SqlDbType.NVarChar,  string.Join(",",objSession.Attendee))

            };

            try
            {
                var status = AddUpdateSessionFile(objSession);
                return SqlUtility.ExecuteScalar(SPAddEditSession.NAME,
                    CommandType.StoredProcedure,
                    prms);

            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Get Session based on filter
        /// </summary>
        /// <param name="pageSize">record count</param>
        /// <param name="sessionType">type of session</param>
        /// <param name="searchKeyword">search keyword</param>
        /// <param name="teamId">team Id</param>
        /// <returns>List of session</returns>
        public List<Common.Entity.Session> GetSessionOnFilter(int pageSize, int sessionType, string searchKeyword,int teamId)
        {
            List<Common.Entity.Session> sessions = new List<Common.Entity.Session>();
            var prms = new List<SqlParameter>
            {
                SqlUtility.CreateParameter(SPGetSessionsOnFilter.PARAM_KEYWORD, SqlDbType.NVarChar, searchKeyword),
                SqlUtility.CreateParameter(SPGetSessionsOnFilter.PARAM_PAGESIZE, SqlDbType.Int, pageSize),
                SqlUtility.CreateParameter(SPGetSessionsOnFilter.PARAM_SESSIONTYPE, SqlDbType.Int, sessionType),
                SqlUtility.CreateParameter(SPGetSessionsOnFilter.PARAM_TEAM, SqlDbType.Int, teamId),

            };
            try
            {
                var dt = SqlUtility.ExecuteAndGetTable(SPGetSessionsOnFilter.NAME,
                    CommandType.StoredProcedure, SPGetSessionsOnFilter.TABLE_NAME, prms);


                foreach (DataRow row in dt.Rows)
                {
                    if (sessions.All(x => x.Id != Convert.ToInt32(row["SessionId"])))
                    {
                        sessions.Add(new Common.Entity.Session
                        {
                            Title = row["Title"].ToString(),
                            Id = Convert.ToInt32(row["SessionId"]),
                            Date = Convert.ToDateTime(row["SessionDate"]),
                            Description = row["Description"].ToString(),
                            PresenterFullName = row["PresenterFullName"].ToString(),
                            Presenter = Convert.ToInt32(row["Presenter"]),
                            VideoFileName = row["VideoFileName"].ToString(),
                            SlideName = row["SlideName"].ToString(),
                            SessionAttendees = new List<Common.Entity.User>()
                        });
                    }

                    int userId;
                    var objSession = sessions.FirstOrDefault(x => x.Id == Convert.ToInt32(row["SessionId"]));

                    // if session doesn't exist or failed to convert user id
                    if (objSession == null || !Int32.TryParse(row["UserId"].ToString(), out userId)) continue;

                    Common.Entity.User objUser = new Common.Entity.User
                    {
                        UserId = Convert.ToInt32(row["UserId"]),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString()
                    };

                    objSession.SessionAttendees.Add(objUser);
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }
            return sessions;
        }

        /// <summary>
        /// Function for update the video filename to associate session.
        /// </summary>
        /// <param name="session">Contain parameter as session object</param>
        /// <returns>Returns true if session file is updated successfully</returns>
        internal bool AddUpdateSessionFile(Common.Entity.Session session)
        {
            try
            {

                using (TrainingTrackerEntities context = new TrainingTrackerEntities())
                {
                    var sessionContext = context.Sessions.FirstOrDefault(s => s.SessionId == session.Id);

                    if (sessionContext == null) return false;

                    if (session.VideoFileName != null)
                    {
                        sessionContext.VideoFileName = session.VideoFileName;
                    }
                    if (session.SlideName != null)
                    {
                        sessionContext.SlideName = session.SlideName;
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