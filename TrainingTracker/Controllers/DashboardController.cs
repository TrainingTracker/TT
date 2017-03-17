using System.Web;
using System.Web.Mvc;
using TrainingTracker.BLL;
using TrainingTracker.Common.Constants;
using TrainingTracker.Authorize;
using TrainingTracker.Common.Entity;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace TrainingTracker.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Dashboard
        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer + "," + UserRoles.Trainee)]
        public ActionResult Index()
        {
            try
            {
                if (HttpContext.User.IsInRole(UserRoles.Administrator) || HttpContext.User.IsInRole(UserRoles.Manager) || HttpContext.User.IsInRole(UserRoles.Trainer))
                {
                    return View("Dashboard");
                }
                return RedirectToAction("UserProfile", "Profile", new { userId = CurrentUser.UserId });
            }
            catch
            {
                return RedirectToAction("SignOut" , "Login");
            }
        }

        [CustomAuthorize(Roles = UserRoles.Administrator + "," + UserRoles.Manager + "," + UserRoles.Trainer)]
        public ActionResult GetDashboardData()
        {
            return Json(new DashboardBl().GetDashboardData(CurrentUser) , JsonRequestBehavior.AllowGet);
        }
    }
}