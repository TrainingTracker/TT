using System.Web.Mvc;
using TrainingTracker.BLL;

namespace TrainingTracker.Controllers
{
    public  class BaseController : Controller
    {
        public Common.Entity.User CurrentUser
        {
            get
            {
                if (Session["currentUser"] == null) Session["currentUser"] = new UserBl().GetUserByUserName(User.Identity.Name);
                return (Common.Entity.User)Session["currentUser"];
            }
        }

        private UserHelpForumBl  _helpForumBl;
        public UserHelpForumBl HelpForumBl
        {
            get { return _helpForumBl??(_helpForumBl = new UserHelpForumBl()); }
        }

        private DiscussionForumBl _discussionForumBl;
        public DiscussionForumBl DiscussionForumBl
        {
            get { return _discussionForumBl ?? (_discussionForumBl = new DiscussionForumBl()); }
        }

        private UserBl _userBl;
        public UserBl UserBl
        {
            get { return _userBl ?? (_userBl = new UserBl()); }
        }
    }
}