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

        // GET: UserProfile?userId=
        [CustomAuthorize(Roles = UserRoles.Administrator+","+UserRoles.Manager+","+UserRoles.Trainer+","+UserRoles.Trainee)]
        public ActionResult UserSetting()
        {
            return View("UserSetting");
        }

        public JsonResult UpdateSubscribedTraineee(List<SubscribedTrainee> updatedList)
        {
            return Json(new UserBl().UpdateSubscribedTraineee(updatedList, CurrentUser));
        }

        public JsonResult GetSubscribedTraineee()
        {
            return Json(new UserBl().GetSubscribedTraineee(CurrentUser.UserId), JsonRequestBehavior.AllowGet);
        }
    }
}