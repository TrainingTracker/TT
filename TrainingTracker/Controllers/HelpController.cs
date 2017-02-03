using System.Web.Mvc;
using TrainingTracker.Authorize;
using TrainingTracker.Common.Entity;

namespace TrainingTracker.Controllers
{
    [CustomAuthorize]
    public class HelpController : BaseController
    {
        // GET: Help
        public ActionResult Index()
        {
            return View("Forum");
        }

        public ActionResult GetPosts(string wildcard, int categoryId, int statusId, int searchPostId, int pageNumber)
        {
            return Json(HelpForumBl.GetHelpForumVm(wildcard, categoryId, statusId, searchPostId, pageNumber), JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetPostById(int postId)
        {
            return Json(HelpForumBl.GetPostWithThreads(postId), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddPost(ForumPost post)
        {
            return Json(HelpForumBl.AddPost(post).ToString());
        }

        [HttpPost]
        public ActionResult UpdatePostStatus(int postId, int statusId, string message, int userId)
        {
            return Json(HelpForumBl.UpdatePostStatus(postId, statusId, message, userId).ToString());
        }

        [HttpPost]
        public ActionResult AddPostThread(ForumThread postThread)
        {
            return Json(HelpForumBl.AddPostThread(postThread).ToString());
        }
    }
}