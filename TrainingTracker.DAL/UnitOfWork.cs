using TrainingTracker.DAL.EntityFramework;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL.Repositories;

namespace TrainingTracker.DAL
{
    public class UnitOfWork
    {
        private readonly TrainingTrackerEntities _context;

        #region Constructor

        public UnitOfWork()
        {
            _context = new TrainingTrackerEntities();
        }

        #endregion

        #region FactorySettings

        private IUserRepository _userRepository;
        public IUserRepository UserRepository
        {
            get
            {
                return _userRepository ?? (_userRepository = new UserRepository(_context));
            }
        }

        private ICourseRepository _courseRepository;
        public ICourseRepository CourseRepository
        {
            get
            {
                return _courseRepository ?? (_courseRepository = new CourseRepository(_context));
            }
        }

        private IEmailAlertSubscriptionRepository _emailAlertSubscriptionRepository;
        public IEmailAlertSubscriptionRepository EmailAlertSubscriptionRepository
        {
            get
            {
                return _emailAlertSubscriptionRepository ?? (_emailAlertSubscriptionRepository = new EmailAlertSubscriptionRepository(_context));
            }
        }

        private IRepository<ForumUserHelpCategory> _forumUserHelpCategoryRepository;
        public IRepository<ForumUserHelpCategory> ForumUserHelpCategoryRepository
        {
            get
            {
                return _forumUserHelpCategoryRepository ?? (_forumUserHelpCategoryRepository = new Repository<ForumUserHelpCategory>(_context));
            }
        }

        private IRepository<ForumUserHelpThread> _forumUserHelpThreadRepository;
        public IRepository<ForumUserHelpThread> ForumUserHelpThreadRepository
        {
            get
            {
                return _forumUserHelpThreadRepository ?? (_forumUserHelpThreadRepository = new Repository<ForumUserHelpThread>(_context));
            }
        }

        private IForumUserHelpPostRepository _forumUserHelpPostRepository;
        public IForumUserHelpPostRepository ForumUserHelpPostRepository
        {
            get
            {
                return _forumUserHelpPostRepository ?? (_forumUserHelpPostRepository = new ForumUserHelpPostRepository(_context));
            }

        }

        private IEmailContentRepository _emailRepository;

        public IEmailContentRepository EmailRepository
        {
            get
            {
                return _emailRepository ?? (_emailRepository = new EmailContentRepository(_context));
            }

        }

        private INotificationRepository _notificationRepository;
        public INotificationRepository NotificationRepository
        {
            get
            {
                return _notificationRepository ?? (_notificationRepository = new NotificationRepository(_context));
            }
        }

        private IFeedbackRepository _feedbackRepository;
        public IFeedbackRepository FeedbackRepository
        {
            get { return _feedbackRepository ?? (_feedbackRepository = new FeedbackRepository(_context)); }

        }

        private IRepository<ForumDiscussionCategory> _forumDiscussionCategoryRepository;
        public IRepository<ForumDiscussionCategory> ForumDiscussionCategoryRepository
        {
            get
            {
                return _forumDiscussionCategoryRepository ?? (_forumDiscussionCategoryRepository = new Repository<ForumDiscussionCategory>(_context));
            }
        }

        private IRepository<ForumDiscussionThread> _forumDiscussionThreadRepository;
        public IRepository<ForumDiscussionThread> ForumDiscussionThreadRepository
        {
            get
            {
                return _forumDiscussionThreadRepository ?? (_forumDiscussionThreadRepository = new Repository<ForumDiscussionThread>(_context));
            }
        }

        private ISessionRepository _sessionRepositoryRepository;
        public ISessionRepository SessionRepository
        {
            get
            {
                return _sessionRepositoryRepository ?? (_sessionRepositoryRepository = new SessionRepository(_context));
            }
        }

        private IReleaseRepository _releaseRepository;
        public IReleaseRepository ReleaseRepository
        {
            get
            {
                return _releaseRepository ?? (_releaseRepository = new ReleaseRepository(_context));
            }
        }

        private IForumDiscussionPostRepository _forumDiscussionPostRepository;
        public IForumDiscussionPostRepository ForumDiscussionPostRepository
        {
            get
            {
                return _forumDiscussionPostRepository ?? (_forumDiscussionPostRepository = new ForumDiscussionPostRepository(_context));
            }
        }

        private ISkillRepository _skillRepository;
        public ISkillRepository SkillRepository
        {
            get
            {
                return _skillRepository ?? (_skillRepository = new SkillRepository(_context));
            }
        }
        #endregion

        #region CommonMethods

        public int Commit()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        #endregion
    }
}