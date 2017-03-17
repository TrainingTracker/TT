using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.Interface;
using TrainingTracker.DAL;
using TrainingTracker.DAL.ModelMapper;

namespace TrainingTracker.BLL.Base
{
    /// <summary>
    /// Bussiness base class
    /// </summary>
    public class BusinessBase
    {

        #region OldDataAccesors

        /// <summary>
        /// Private skill dal accessor
        /// </summary>
        private ISkillDal _skillDalAccessor;

        /// <summary>
        /// public with only getter Skill dal accessor
        /// </summary>
        public ISkillDal SkillDataAccesor
        {
            get { return _skillDalAccessor ?? (_skillDalAccessor = new SkillDal()); }
        }

        /// <summary>
        /// Private user dal accessor
        /// </summary>
        private IUserDal _userDalAccessor;

        /// <summary>
        /// public with only getter User dal accessor
        /// </summary>
        public IUserDal UserDataAccesor
        {
            get { return _userDalAccessor ?? (_userDalAccessor = new UserDal()); }
        }

       
        /// <summary>
        /// Private feedback dal accessor
        /// </summary>
        private IFeedbackDal _feedbackDalAccessor;

        /// <summary>
        /// public with only getter Feedback dal accessor
        /// </summary>
        public IFeedbackDal FeedbackDataAccesor
        {
            get { return _feedbackDalAccessor ?? (_feedbackDalAccessor = new FeedbackDal()); }
        }


        /// <summary>
        /// Private question dal accessor
        /// </summary>
        private IQuestionDal _questionDalAccessor;

        /// <summary>
        /// public with only getter Question dal accessor
        /// </summary>
        public IQuestionDal QuestionDataAccesor
        {
            get { return _questionDalAccessor ?? (_questionDalAccessor = new QuestionDal()); }
        }


        /// <summary>
        /// Private notification dal accessor
        /// </summary>
        private INotificationDal _notificationDataAccesor;

        /// <summary>
        /// public with only getter Notification dal accessor
        /// </summary>
        public INotificationDal NotificationDataAccesor
        {
            get { return _notificationDataAccesor ?? (_notificationDataAccesor = new NotificationDal()); }
        }


        /// <summary>
        /// Private notification dal accessor
        /// </summary>
        private ITeamDal _teamDataAccesor;

        /// <summary>
        /// public with only getter Notification dal accessor
        /// </summary>
        public ITeamDal TeamDataAccesor
        {
            get { return _teamDataAccesor ?? (_teamDataAccesor = new TeamDal()); }
        }


        /// <summary>
        /// Private Survey dal accessor
        /// </summary>
        private ISurveyDal _surveyDataAccesor;

        /// <summary>
        /// public with only getter Survey dal accessor
        /// </summary>
        public ISurveyDal SurveyDataAccesor
        {
            get { return _surveyDataAccesor ?? (_surveyDataAccesor = new SurveyDal()); }
        }


        /// <summary>
        /// Private variable for LearningPathDal accessor.
        /// </summary>
        private ILearningPathDal _learningPathDataAccessor;

        /// <summary>
        /// Public property to get the LearningPathDal object.
        /// </summary>
        public ILearningPathDal LearningPathDataAccessor
        {
            get { return _learningPathDataAccessor ?? (_learningPathDataAccessor = new LearningPathDal()); }
        }


        /// <summary>
        /// Private variable for LearningMapDal accessor.
        /// </summary>
        private ILearningMapDal _learningMapDataAccessor;

        /// <summary>
        /// Public property to get the LearningPathDal object.
        /// </summary>
        public ILearningMapDal LearningMapDataAccessor
        {
            get { return _learningMapDataAccessor ?? (_learningMapDataAccessor = new LearningMapDal()); }
        }

        #endregion

       
        private UnitOfWork _unitOfWork;
        public UnitOfWork UnitOfWork
        {
            get { return _unitOfWork ?? (_unitOfWork = new UnitOfWork()); }
        }


        private ModelMapper.ModelMapper _modelMapperObject;
        public ModelMapper.ModelMapper ModelMapper
        {
            get
            {
                return _modelMapperObject ?? (_modelMapperObject = new ModelMapper.ModelMapper());
            }
        }


        #region "Converters"

        private ForumUserHelpPostConverter _forumUserHelpPostConverter;
        public ForumUserHelpPostConverter ForumUserHelpPostConverter
        {
            get
            {
                return _forumUserHelpPostConverter ?? (_forumUserHelpPostConverter = new ForumUserHelpPostConverter());
            }
        }


        private ForumUserHelpThreadConverter _forumUserHelpThreadConverter;
        public ForumUserHelpThreadConverter ForumUserHelpThreadConverter
        {
            get
            {
                return _forumUserHelpThreadConverter ?? (_forumUserHelpThreadConverter = new ForumUserHelpThreadConverter());
            }
        }

        private ForumDiscussionPostConverter _forumDiscussionPostConverter;
        public ForumDiscussionPostConverter ForumDiscussionPostConverter
        {
            get
            {
                return _forumDiscussionPostConverter ?? (_forumDiscussionPostConverter = new ForumDiscussionPostConverter());
            }
        }

        private ForumDiscussionThreadConverter _forumDiscussionThreadConverter;
        public ForumDiscussionThreadConverter ForumDiscussionThreadConverter
        {
            get
            {
                return _forumDiscussionThreadConverter ?? (_forumDiscussionThreadConverter = new ForumDiscussionThreadConverter());
            }
		}

        private SessionConverter _sessionConverter;
        public SessionConverter SessionConverter
        {
            get
            {
                return _sessionConverter ?? (_sessionConverter = new SessionConverter());
            }
        }

        private ReleaseConverter _releaseConverter;
        public ReleaseConverter ReleaseConverter
        {
            get
            {
                return _releaseConverter ?? (_releaseConverter = new ReleaseConverter());
            }
        }

        private UserConverter _userConverter;
        public UserConverter UserConverter
        {
            get
            {
                return _userConverter ?? (_userConverter = new UserConverter());
            }
        }
		
        private EmailAlertSubscriptionConverter _emailAlertSubscriptionConverter;
        public EmailAlertSubscriptionConverter EmailAlertSubscriptionConverter
        {
            get
            {
                return _emailAlertSubscriptionConverter ?? (_emailAlertSubscriptionConverter = new EmailAlertSubscriptionConverter());
            }
        }

        private NotificationConverter _notificationConverter ;
        public NotificationConverter NotificationConverter
        {
            get
            {
                return _notificationConverter ?? (_notificationConverter = new NotificationConverter());
            }
        }


        #endregion

    }
}