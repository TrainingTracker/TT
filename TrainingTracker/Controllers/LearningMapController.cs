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
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            return Json(new LearningMapBL().GetAllLearningMaps((int)currentUser.TeamId), JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetAllTrainees()
        {
            User currentUser = new UserBl().GetUserByUserName(User.Identity.Name);
            return Json(new LearningMapBL().GetAllTrainees((int)currentUser.TeamId), JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddLearningMap(LearningMap data)
        {
            return Json(new LearningMapBL().AddLearningMap(data), JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateLearningMap(LearningMap data)
        {
            return Json(new LearningMapBL().UpdateLearningMap(data), JsonRequestBehavior.AllowGet);
        }
       
    }
}