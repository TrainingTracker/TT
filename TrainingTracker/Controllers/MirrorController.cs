#region assembly

using System;
using System.Web.Mvc;
using TrainingTracker.Authorize;
using TrainingTracker.BLL;
using TrainingTracker.Common.Constants;
using TrainingTracker.Common.Enumeration;

#endregion

namespace TrainingTracker.Controllers
{
    [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
    public class MirrorController : BaseController
    {
        public ActionResult Index()
        {
            return RedirectToAction("Mirror");
        }

        public ActionResult Mirror()
        {
            return View();
        }

        public JsonResult GetAssignmentFeedbackWithFilters(int userId, DateTime startDate, DateTime endDate)
        {
            return Json(new MirrorBl().GetFeedbacksWithFilters(userId, startDate, endDate, FeedbackType.Assignment),
                       JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCodeReviewFeedbackWithFilters(int userId, DateTime startDate, DateTime endDate)
        {
            return Json(new MirrorBl().GetFeedbacksWithFilters(userId, startDate, endDate, FeedbackType.CodeReview),
                       JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSkillsFeedbackWithFilters(int userId, DateTime startDate, DateTime endDate)
        {
            return Json(new MirrorBl().GetFeedbacksWithFilters(userId, startDate, endDate, FeedbackType.Skill),
                       JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetRandomReviewFeedbackWithFilters(int userId, DateTime startDate, DateTime endDate)
        {
            return Json(new MirrorBl().GetFeedbacksWithFilters(userId, startDate, endDate, FeedbackType.RandomReview),
                       JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWeeklyFeedbackWithFilters(int userId, DateTime startDate, DateTime endDate)
        {

            return Json(new MirrorBl().GetFeedbacksWithFilters(userId, startDate, endDate, FeedbackType.Weekly),
                       JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadWeeklyFeedbackLearningTimelines(int userId, DateTime startDate, DateTime endDate)
        {
            return Json(new MirrorBl().LoadWeeklyFeedbackLearningTimelines(userId, startDate, endDate),
                        JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadWeeklyFeedbackTipDetails(int userId, DateTime startDate, DateTime endDate)
        {

            return Json(new MirrorBl().LoadWeeklyFeedbackTipDetails(userId, startDate, endDate),
                        JsonRequestBehavior.AllowGet);
        }
    }
}