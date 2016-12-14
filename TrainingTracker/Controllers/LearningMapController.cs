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

        public JsonResult GetLearningMapWithAllData()
        { 
            return Json(new LearningMapBL().GetLearningMapWithAllData(1), JsonRequestBehavior.AllowGet);
        }
       
    }
}