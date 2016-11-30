/**************************************************************************************************
*   Created By : Satyabrata                                                                                                                                                           
*   Created On : 11 Sept 2016
*   Modified By :
*   Modified Date:
*   Description: Notification controller file containing Action methods for Notification.
**************************************************************************************************/
using System.Web.Mvc;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Controllers
{
    [CustomAuthorize]
    public class NotificationController : Controller
    {
        /// <summary>
        /// Action method for GetNotification.
        /// </summary>
        /// <returns>Return list of notification as JSON object.</returns>
        public ActionResult GetNotification()
        {
            return Json(new NotificationBl().GetNotification(new UserBl().GetUserByUserName(User.Identity.Name).UserId), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        ///Action method for  Update Notification
        /// </summary>
        /// <param name="notification">Notification object</param>
        /// <returns> Return a boolean value as a JSON object.</returns>
        [HttpPost]
        public ActionResult UpdateNotification(Notification notification)
        {
            return Json(new NotificationBl().UpdateNotification(new UserBl().GetUserByUserName(User.Identity.Name).UserId, notification));
        }

        /// <summary>
        ///Action method for  Update All Notification for user
        /// </summary>
        /// <param name="notification">Notification object</param>
        /// <returns> Return a boolean value as a JSON object.</returns>
        [HttpPost]
        public ActionResult MarkAllNotificationAsRead()
        {
            return Json(new NotificationBl().MarkAllNotificationAsRead(new UserBl().GetUserByUserName(User.Identity.Name).UserId));
        }
    }
}