using System.Web.Mvc;
using TrainingTracker.BLL;

namespace TrainingTracker.Controllers
{
    public  class BaseController : Controller
    {
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

    }
}