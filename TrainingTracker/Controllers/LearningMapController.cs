using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Collections.Generic;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Entity;
using TrainingTracker.Common.Utility;

namespace TrainingTracker.Controllers
{
    [CustomAuthorizeAttribute]
    [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
    public class LearningMapController : Controller
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

        public ViewResult LearningMap()
        {
            return View();
        }


        public JsonResult GetLearningMapWithAllData(int id)
        { 
            return Json(new LearningMapBL().GetLearningMapWithAllData(id), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllCourses()
        {
            return Json(new LearningMapBL().GetAllCourses(), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllLearningMaps()
        {
            return Json(new LearningMapBL().GetAllLearningMaps((int)CurrentUser.TeamId), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllTrainees()
        {
            return Json(new UserBl().GetAllTrainees((int)CurrentUser.TeamId), JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddLearningMap(LearningMap data)
        {
            data.TeamId = (int)CurrentUser.TeamId;
            data.CreatedBy = CurrentUser.UserId;

            return Json(new LearningMapBL().AddLearningMap(data), JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateLearningMap(LearningMap data)
        {
            return Json(new LearningMapBL().UpdateLearningMap(data), JsonRequestBehavior.AllowGet);
        }
       

        public JsonResult DeleteLearningMap(int id)
        {
            return Json(new LearningMapBL().DeleteLearningMap(id), JsonRequestBehavior.AllowGet);
        }
    }
}