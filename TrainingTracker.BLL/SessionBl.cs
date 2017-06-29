using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;
using TrainingTracker.Common.ViewModel;
using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.ModelMapper;
using Session = TrainingTracker.Common.Entity.Session;
using User = TrainingTracker.Common.Entity.User;

namespace TrainingTracker.BLL
{
    /// <summary>
    /// Bussiness class for session
    /// </summary>
    public class SessionBl : BusinessBase
    {
        /// <summary>
        /// method to Add or edit session details
        /// </summary>
        /// <param name="objSession">instance of session Object</param>
        /// <param name="currentUser">instance of Current User</param>
        /// <returns>boolean resutt of event's success</returns>
        public SessionVm AddNewSession(Session objSession, User currentUser)
        {
            objSession.IsNeW = (objSession.Id == 0);
            objSession.Presenter = currentUser;

            DAL.EntityFramework.Session coreSession = SessionConverter.ConvertToCore(objSession);

            foreach (var trainee in objSession.SessionAttendees)
            {
                coreSession.UserSessionMappings.Add(new UserSessionMapping
                {
                    AddedBy = currentUser.UserId,
                    AddedOn = DateTime.Now,
                    UserId = trainee.UserId
                });
            }
            UnitOfWork.SessionRepository.Add(coreSession);

            UnitOfWork.Commit();
            objSession.Id = coreSession.SessionId;
         
            new NotificationBl().AddSessionNotification(objSession);
            return GetSessionOnFilter(1, (int)Common.Enumeration.SessionType.All, coreSession.SessionId, "", currentUser);

        }

        /// <summary>
        /// method to Add or edit session details
        /// </summary>
        /// <param name="objSession">instance of session Object</param>
        /// <returns>boolean resutt of event's success</returns>
        public bool UpdateSessionsDetails(Session objSession, User currentUser)
        {
            if (objSession.Id <= 0) return false;
            try
            {
                DAL.EntityFramework.Session coreSession = UnitOfWork.SessionRepository
                                                                    .GetSessionWithAttendeesTrackable(objSession.Id);

                coreSession.SessionDate = objSession.Date;
                coreSession.Description = objSession.Description;
                coreSession.Title = objSession.Title;
                coreSession.SlideName = objSession.SlideName;
                coreSession.VideoFileName = objSession.VideoFileName;

                coreSession.UserSessionMappings.Where(x => objSession.SessionAttendees.All(y => y.UserId != x.UserId))
                                               .ToList()
                                               .ForEach( trainee => coreSession.UserSessionMappings.Remove(trainee));
               

                objSession.SessionAttendees.Where(x => coreSession.UserSessionMappings.All(y => y.UserId != x.UserId))
                                           .ToList()
                                           .ForEach( trainee => coreSession.UserSessionMappings.Add(new UserSessionMapping
                                                                                                     {
                                                                                                         AddedBy = currentUser.UserId,
                                                                                                         AddedOn = DateTime.Now,
                                                                                                         UserId = trainee.UserId
                                                                                                     }));

                objSession.Presenter = currentUser;

              return UnitOfWork.Commit() > 0 && (new NotificationBl().AddSessionNotification(objSession));
            }
            catch (Exception ex)
            {
                LogUtility.ErrorRoutine(ex);
                return false;
            }

        }

        public Session GetSessionWithAttendees(int sessionId)
        {
            return SessionConverter.ConvertFromCoreWithAttendees(UnitOfWork.SessionRepository.GetSessionWithAttendees(sessionId));
        }

        /// <summary>
        /// Get session based on filter conditions
        /// </summary>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="sessionType">type of session All/Presented/Scheduled</param>
        ///  <param name="sessionId">any session id</param>
        /// <param name="searchKeyword">any keyword term</param>
        /// <param name="currentUser">CurrentUser</param>
        /// <returns>instance of session method</returns>
        public SessionVm GetSessionOnFilter(int pageNumber, int sessionType, int sessionId, string searchKeyword, User currentUser)
        {
            return GetSessions(pageNumber, sessionType, sessionId, searchKeyword, currentUser);
        }

        public SessionVm GetSessionVm(int pageNumber, int sessionType, int sessionId, string searchKeyword, User currentUser)
        {
            SessionVm objSessionVm = GetSessions(pageNumber, sessionType, sessionId, searchKeyword, currentUser);

            objSessionVm.AllAttendees = UserConverter.ConvertListFromCore(UnitOfWork.UserRepository.GetAllTrainees(currentUser.TeamId ?? 0, true))
                                                     .OrderBy(x => x.FirstName)
                                                     .ToList();

            return objSessionVm;
        }

        public SessionVm GetSessions(int pageNumber, int sessionType, int sessionId, string searchKeyword, User currentUser)
        {
            SessionVm objSessionVm = new SessionVm
            {
                SessionList = GetPagedFilteredSessions(searchKeyword, sessionType, sessionId, currentUser.TeamId ?? 0, pageNumber, 5),
            };

            if (objSessionVm.SessionList.Results != null && objSessionVm.SessionList.Results.Count > 0)
            {
                objSessionVm.DefaultSession = GetSessionWithAttendees(objSessionVm.SessionList.Results[0].Id);
            }

            return objSessionVm;
        }

        public bool UpdateSessionAssets(Session session,bool isVideo)
        {
            if (session.Id <= 0) return false;

            DAL.EntityFramework.Session coreSession = UnitOfWork.SessionRepository
                                                                   .GetSessionWithAttendeesTrackable(session.Id);
            if (!isVideo) coreSession.SlideName = session.SlideName;
            else coreSession.VideoFileName = session.VideoFileName;

            return UnitOfWork.Commit() > 0;

        }

        private PagedResult<Session> GetPagedFilteredSessions(string wildcard, int statusId, int searchSessionId
                                                             , int teamId, int pageNumber, int pageSize)
        {
            var session = UnitOfWork.SessionRepository.GetPagedFilteredSessions(wildcard, (Common.Enumeration.SessionType)statusId, searchSessionId,
                                                                                 teamId, pageNumber, pageSize);

            if (session == null) return new PagedResult<Session>();

            return new PagedResult<Session>
            {
                CurrentPage = session.CurrentPage,
                PageCount = session.PageCount,
                PageSize = session.PageSize,
                RowCount = session.RowCount,
                Results = session.Results == null
                           ? new List<Session>()
                           : SessionConverter.ConvertListFromCore(session.Results.ToList())
            };
        }
    }
}
