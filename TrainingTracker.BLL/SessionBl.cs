using System;
using TrainingTracker.BLL.Base;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.ViewModel;

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

        /// <summary>
        /// Get session based on filter conditions
        /// </summary>
        /// <param name="pageSize">number of results to be fetched</param>
        /// <param name="sessionType">type of session All/Presented/Scheduled</param>
        /// <param name="searchKeyword">any keyword term</param>
        /// <param name="currentUser">CurrentUser</param>
        /// <returns>instance of session method</returns>
        public SessionVm GetSessionOnFilter(int pageSize, int sessionType, string searchKeyword,User currentUser)
        {
            SessionVm objSessionVm = new SessionVm
            {
                SessionList = SessionDataAccesor.GetSessionOnFilter(pageSize , sessionType , searchKeyword , currentUser.TeamId??0) ,
                AllAttendees = new UserBl().GetAllUsersByTeam(currentUser)
            };
            return objSessionVm;
        }
    }
}
