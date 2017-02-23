using System;
using System.Collections.Generic;
using System.Linq;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.ViewModel;
using TrainingTracker.DAL.ModelMapper;

namespace TrainingTracker.BLL
{
    /// <summary>
    /// Bussiness class for session
    /// </summary>
    public class SessionBl : BussinessBase
    {
        /// <summary>
        /// method to Add or edit session details
        /// </summary>
        /// <param name="objSession">instance of session Object</param>
        /// <returns>boolean resutt of event's success</returns>
        public bool AddEditSessions(Session objSession)
        {
            objSession.IsNeW = objSession.Id == 0;

            try
            {
                objSession.Id = SessionDataAccesor.AddEditSessions(objSession);
                return (new NotificationBl().AddSessionNotification(objSession));
            }
            catch (Exception)
            {
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
           return  GetSessions(pageNumber, sessionType, sessionId, searchKeyword, currentUser);
        }

        public SessionVm GetSessionVm(int pageNumber, int sessionType, int sessionId, string searchKeyword, User currentUser)
        {
            SessionVm objSessionVm = GetSessions(pageNumber, sessionType, sessionId, searchKeyword, currentUser);

            objSessionVm.AllAttendees = UserConverter.ConvertListFromCore(UnitOfWork.UserRepository.GetAllTrainees(currentUser.TeamId ?? 0, true))
                                                     .OrderBy(x=>x.FirstName)
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
