using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using Session = TrainingTracker.Common.Entity.Session;

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
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    sessions = context.UserSessionMappings.Where(m => m.UserId == userId).Select(s => new Common.Entity.Session
                    {
                        Id = s.Session.SessionId,
                        Title = s.Session.Title
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
            }

            return sessions;
        }

        /// <summary>
        /// Method to save session data.
        /// </summary>
        /// <param name="session">Session data.</param>
        /// <returns>Session id.</returns>
        public int AddEditSessions(Common.Entity.Session session)
        {
            EntityFramework.Session sessionData = null;

            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    if (session.Id > 0) sessionData = context.Sessions.SingleOrDefault(x => x.SessionId == session.Id);
                    if (sessionData == null) sessionData = new EntityFramework.Session { SessionId = session.Id };
                    sessionData.Presenter = session.Presenter;
                    sessionData.Title = session.Title;
                    sessionData.Description = session.Description;
                    sessionData.SessionDate = session.Date;
                    if (session.Id == 0) context.Sessions.Add(sessionData);
                    if (session.Attendee.Any())
                    {
                        foreach (var user in session.Attendee)
                        {
                            context.UserSessionMappings.Add(new UserSessionMapping
                            {
                                AddedBy = session.Presenter,
                                UserId = Convert.ToInt32(user),
                                AddedOn = DateTime.Now,
                                SessionId = sessionData.SessionId
                            });
                        }
                    }
                    context.SaveChanges();
                    return sessionData.SessionId;
                }
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return 0;
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
        public List<Common.Entity.Session> GetSessionOnFilter(int pageSize, int sessionType, string searchKeyword, int teamId)
        {
            var sessions = new List<Common.Entity.Session>();
            var today = DateTime.Now.Date;
            try
            {
                using (var context = new TrainingTrackerEntities())
                {
                    var query = context.Sessions.Where(x => (teamId == 0 || x.User.TeamId == teamId));
                    if (sessionType == 1) query = query.Where(x => x.SessionDate >= today);
                    if (sessionType == 2) query = query.Where(x => x.SessionDate < today);
                 
                    sessions = query.OrderByDescending(x => x.SessionDate).Take(pageSize).Select(s => new Session
                    {
                        Title = s.Title,
                        Id = s.SessionId,
                        Date = s.SessionDate ?? DateTime.MinValue,
                        Description = s.Description,
                        PresenterFullName = s.User.FirstName + " " + s.User.LastName,
                        Presenter = s.Presenter ?? 0,
                        VideoFileName = s.VideoFileName,
                        SlideName = s.SlideName,
                        }).ToList();

                    foreach (var session in sessions)
                    {
                        session.SessionAttendees = context.UserSessionMappings.Where(x => x.SessionId == session.Id)
                            .Select(m => new Common.Entity.User
                            {
                                UserId = m.User1.UserId,
                                FirstName = m.User1.FirstName,
                                LastName = m.User1.LastName
                            }).ToList();
                    }
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

                using (var context = new TrainingTrackerEntities())
                {
                    var sessionContext = context.Sessions.FirstOrDefault(s => s.SessionId == session.Id);

                    if (sessionContext == null) return false;
                    if (session.VideoFileName != null) sessionContext.VideoFileName = session.VideoFileName;
                    if (session.SlideName != null) sessionContext.SlideName = session.SlideName;

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