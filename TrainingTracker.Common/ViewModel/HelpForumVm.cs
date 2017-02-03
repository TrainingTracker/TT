using TrainingTracker.Common.Entity;

namespace TrainingTracker.Common.ViewModel
{
    public class HelpForumVm
    {
        public PagedResult<ForumPost> Posts { get; set; }
        public ForumPost DefaultPost { get; set; }
    }
}