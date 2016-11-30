using System.Web.Mvc;
using TrainingTracker.Authorize;

namespace TrainingTracker.Controllers
{
    public class UnauthorizedController : Controller
    {
        // GET: Unauthorized
        [CustomAuthorize]
        public ActionResult UnauthorizedAccess()
        {
            return View("UnauthorizedAccess");
        }
    }
}