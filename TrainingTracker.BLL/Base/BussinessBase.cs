using TrainingTracker.DAL.DataAccess;
using TrainingTracker.DAL.Interface;

namespace TrainingTracker.BLL.Base
{
    /// <summary>
    /// Bussiness base class
    /// </summary>
    public class BussinessBase
    {
        /// <summary>
        /// Private session dal accessor
        /// </summary>
        private ISessionDal _sessionDalAccessor;

        /// <summary>
        /// public with only getter Sesion dal accessor
        /// </summary>
        public ISessionDal SessionDataAccesor
        {
            get { return _sessionDalAccessor ?? (_sessionDalAccessor = new SessionDal()); }
        }

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
            get { return  _userDalAccessor ?? ( _userDalAccessor =new UserDal()); }    
        }

        /// <summary>
        /// Private project dal accessor
        /// </summary>
        private IProjectDal _projectDalAccessor;

        /// <summary>
        /// public with only getter Project dal accessor
        /// </summary>
        public IProjectDal ProjectDataAccesor
        {
            get { return  _projectDalAccessor ?? ( _projectDalAccessor =new ProjectDal()); }  
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
            get { return  _feedbackDalAccessor ?? ( _feedbackDalAccessor = new FeedbackDal()); } 
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
            get { return  _questionDalAccessor ?? ( _questionDalAccessor = new QuestionDal()); } 
        }
		
		
        /// <summary>
        /// Private release dal accessor
        /// </summary>
        private IReleaseDal _releaseDataAccesor;
		
		/// <summary>
        /// public with only getter Release dal accessor
        /// </summary>
		public IReleaseDal ReleaseDataAccesor
        {
			get { return  _releaseDataAccesor ?? ( _releaseDataAccesor = new ReleaseDal()); } 
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
          get { return  _notificationDataAccesor ?? ( _notificationDataAccesor = new NotificationDal()); } 
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
    }
}