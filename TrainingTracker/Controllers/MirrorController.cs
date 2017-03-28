#region assembly
using System.Web.Mvc;
using TrainingTracker.Authorize;
using TrainingTracker.Common.Constants;

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
	}
}