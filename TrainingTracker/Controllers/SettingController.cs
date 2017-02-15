using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;

namespace TrainingTracker.Controllers
{
    [CustomAuthorizeAttribute]
    public class SettingController : Controller
    {
        User CurrentUser
        {
            get 
            {
                if (Session["currentUser"] == null)
                {
                    Session["currentUser"] = new UserBl().GetUserByUserName(User.Identity.Name);
                }
                
                return (User)Session["currentUser"];
            }
        }

        
        [CustomAuthorize(Roles = UserRoles.Administrator+","+UserRoles.Manager+","+UserRoles.Trainer+","+UserRoles.Trainee)]
        public ActionResult UserSetting()
        {
            return View("UserSetting");
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult SetEmailPreferences(List<EmailAlertSubscription> emailSubscriptions)
        {
            return Json(new EmailPreferencesBl().SetEmailPreferences(emailSubscriptions, CurrentUser));
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public JsonResult GetCurrentUserSubscriptions()
        {
            return Json(new EmailPreferencesBl().GetUserSubscriptionsById(CurrentUser.UserId), JsonRequestBehavior.AllowGet);
        }
    }
}