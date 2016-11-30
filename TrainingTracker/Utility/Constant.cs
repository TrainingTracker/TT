namespace TrainingTracker.Utility
{
    /// <summary>
    /// Constant class for Stored procedure VALIDATE_USER.
    /// </summary>
    public static class SPValidateUser
    {
        /// <summary>
        /// Stored procedure name.
        /// </summary>
        public const string NAME = "VALIDATE_USER";
        /// <summary>
        /// Table name to get result.
        /// </summary>
        public const string TABLE_NAME = "ValidationResult";
        /// <summary>
        /// Parameter name of username.
        /// </summary>
        public const string PARAM_USER_NAME = "@Username";
        /// <summary>
        /// Parameter name of password.
        /// </summary>
        public const string PARAM_PASSWORD = "@Password";
    }

    /// <summary>
    /// Constant class for Stored procedure GET_DASHBOARD_DATA.
    /// </summary>
    public static class SPGetDashboardData
    {
        public const string NAME = "GET_DASHBOARD_DATA";
        public const string TABLE_NAME = "UsersTable";
    }

    /// <summary>
    /// Constant class for Stored procedure ADD_USER.
    /// </summary>
    public static class SPAddUser
    {
        public const string NAME = "ADD_USER";
        public const string PARAM_FIRST_NAME = "@FirstName";
        public const string PARAM_LAST_NAME = "@LastName";
        public const string PARAM_USER_NAME = "@Username";
        public const string PARAM_PASSWORD = "@Password";
        public const string PARAM_EMAIL = "@Email";
        public const string PARAM_DESIGNATION = "@Designation";
        public const string PARAM_PROFILE_PICTURE_NAME = "@ProfilePictureName";
        public const string PARAM_IS_FEMALE = "@IsFemale";
        public const string PARAM_IS_ADMINISTRATOR = "@IsAdministrator";
        public const string PARAM_IS_TRAINER = "@IsTrainer";
        public const string PARAM_IS_TRAINEE = "@IsTrainee";
        public const string PARAM_IS_MANAGER = "@IsManager";
    }

    /// <summary>
    /// Constant class for Stored procedure GET_ALL_USERS.
    /// </summary>
    public static class SPGetAllUsers
    {
        public const string NAME = "GET_ALL_USERS";
        public const string TABLE_NAME = "UsersTable";
    }

    /// <summary>
    /// Constant class for Stored procedure GET_PROJECTS_BY_USER_ID.
    /// </summary>
    public static class SPGetProjectsByUserID
    {
        public const string NAME = "GET_PROJECTS_BY_USER_ID";
        public const string PARAM_USER_ID = "@UserId";
        public const string TABLE_NAME = "Result";
    }

    /// <summary>
    /// Constant class for Stored procedure GET_SESSIONS_BY_USER_ID.
    /// </summary>
    public static class SPGetSessionsByUserID
    {
        public const string NAME = "GET_SESSIONS_BY_USER_ID";
        public const string PARAM_USER_ID = "@UserId";
        public const string TABLE_NAME = "Result";
    }

    /// <summary>
    /// Constant class for Stored procedure GET_SKILLS_BY_USER_ID.
    /// </summary>
    public static class SPGetSkillsByUserID
    {
        public const string NAME = "GET_SKILLS_BY_USER_ID";
        public const string PARAM_USER_ID = "@UserId";
        public const string TABLE_NAME = "Result";
    }

    /// <summary>
    /// Constant Class for Stored procedure GET_APPLICATION_SKILLS
    /// </summary>
    public static class SPGetApplicationSkills
    {
        public const string NAME = "GET_APPLICATION_SKILLS";
        public const string TABLE_NAME = "Result";
    }

    /// <summary>
    /// Constant class for Stored procedure GET_USER_BY_ID.
    /// </summary>
    public static class SPGetUser
    {
        public const string NAME = "GET_USER";
        public const string PARAM_USER_ID = "@UserId";
        public const string PARAM_USER_NAME = "@UserName";
        public const string PARAM_SCENARIO = "@Scenario";
        public const string TABLE_NAME = "Result";
    }

    /// <summary>
    /// Constant class for Stored procedure GET_USER_FEEDBACKS.
    /// </summary>
    public static class SPGetUserFeedbacks
    {
        public const string NAME = "GET_USER_FEEDBACKS";
        public const string PARAM_USER_ID = "@UserId";
        public const string PARAM_SCENARIO = "@Scenario";
        public const string PARAM_PAGE_SIZE = "@PageSize";
        public const string PARAM_OFFSET = "@Offset";
        public const string FEEDBACK_ID = "@FeedbackId";
        public const string TABLE_NAME = "Result";
    }

    /// <summary>
    /// Constant class for Stored procedure ADD_FEEDBACK.
    /// </summary>
    public static class SPAddFeedback
    {
        public const string NAME = "ADD_FEEDBACK";
        public const string PARAM_FEEDBACK_TEXT = "@FeedbackText";
        public const string PARAM_TITLE = "@Title";
        public const string PARAM_FEEDBACK_TYPE = "@FeedbackType";
        public const string PARAM_PROJECT_ID = "@ProjectId";
        public const string PARAM_SKILL_ID = "@SkillId";
        public const string PARAM_RATING = "@Rating";
        public const string PARAM_ADDED_BY = "@AddedBy";
        public const string PARAM_ADDED_FOR = "@AddedFor";
        public const string PARAM_START_DATE = "@StartDate";
        public const string PARAM_END_DATE = "@EndDate";
    }

    #region Obsolete

    ///// <summary>
    ///// Constant class for Stored procedure GET_FEEDBACKS_BY_USER_ID.
    ///// </summary>
    //public static class SPGetFeedbacksByUserID
    //{
    //    public const string NAME = "GET_FEEDBACKS_BY_USER_ID";
    //    public const string PARAM_USER_ID = "@UserId";
    //    public const string TABLE_NAME = "Result";
    //}

    ///// <summary>
    ///// Constant class for Stored procedure ADD_COMMENT.
    ///// </summary>
    //public static class SPAddComment
    //{
    //    public const string NAME = "ADD_COMMENT";
    //    public const string PARAM_COMMENT = "@Comment";
    //    public const string PARAM_ADDED_BY = "@AddedBy";
    //    public const string PARAM_ADDED_FOR = "@AddedFor";
    //}

    ///// <summary>
    ///// Constant class for Stored procedure ADD_SKILL_FEEDBACK.
    ///// </summary>
    //public static class SPAddSkillFeedback
    //{
    //    public const string NAME = "ADD_SKILL_FEEDBACK";
    //    public const string PARAM_FEEDBACK = "@Feedback";
    //    public const string PARAM_RATING = "@Rating";
    //    public const string PARAM_ADDED_BY = "@AddedBy";
    //    public const string PARAM_SKILL_ID = "@SkillId";
    //    public const string PARAM_USER_ID = "@UserId";
    //}

    ///// <summary>
    ///// Constant class for Stored procedure ADD_USER_PROJECT_FEEDBACK.
    ///// </summary>
    //public static class SPAddUserProjectFeedback
    //{
    //    public const string NAME = "ADD_USER_PROJECT_FEEDBACK";
    //    public const string PARAM_FEEDBACK = "@Feedback";
    //    public const string PARAM_RATING = "@Rating";
    //    public const string PARAM_ADDED_BY = "@AddedBy";
    //    public const string PARAM_USER_PROJECT_ID = "@UserProjectId";
    //}

    #endregion

}