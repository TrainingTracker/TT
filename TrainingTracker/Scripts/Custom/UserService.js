$(document).ready(function (my) {
    "use strict";
    my.userService = {
        createUser: function (user, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/CreateUser", user, callback);
        },
        updateUser: function (user, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/UpdateUser", user, callback);
        },
        getAllUsers: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetManageProfileVm", null, callback);
        },
         getActiveUsers: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetActiveUsers", null, callback);
        },
        getUserProfileVm: function (userId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetUserProfileVm?userId=" + userId, null, callback);
        },
        addUserFeedback: function (feedbackPost, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/AddFeedback", feedbackPost, callback);
        },
        authenticateUser: function (userData, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Login/AuthenticateLogin?userName=" + userData.UserName
                + "&password=" + userData.Password, null, callback);
        },
        getCurrentUser: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Login/GetCurrentUser", null, callback);
        },
        getFeedbackonAppliedFilter: function (pageSize, feedbackId, userId,startDate,endDate, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetUserFeedbackOnFilter?pageSize=" + pageSize + "&feedbackType=" + feedbackId + "&userId=" + userId + "&startDate=" + startDate + "&endDate=" + endDate, null, callback);
        },
        getDashboardVm: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Dashboard/GetDashboardData", null, callback);
        },
        addEditSession:function(sessionDetails, callback)
        {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Session/AddEditSession", sessionDetails, callback);
        },
        getSessionsOnFilter:function(pageSize,seminarType,searchKeyword,getSessionsOnFilterCallback) 
        {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Session/GetUserFeedbackOnFilter?pageSize=" + pageSize + "&seminarType=" + seminarType + "&searchKeyword=" + '', null, getSessionsOnFilterCallback);
        },
        uploadImage: function (imagefile, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/Profile/UploadImage", imagefile, callback);
        },
        getUserFeedbackForPlot :function(traineeId, startDate, endDate, arrayFeedbackType, trainer, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetUserFeedbackOnFilterForPlot?traineeId=" + traineeId + "&startDate=" + startDate + "&endDate=" + endDate + "&trainerId=" + trainer + "&arrayFeedbackType=" + arrayFeedbackType, null, callback);
        },
        getNotification: function (callback)
        {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Notification/GetNotification", null, callback);
        },
        updateNotification: function (notificationInfo, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Notification/UpdateNotification", notificationInfo, callback);
        },
        getFeedbackWithThreads: function (feedbackId,callback)
        {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/GetFeedbackWithThreads?FeedbackId=" + feedbackId, null, callback);
        },
        getFeedbackThreads: function (feedbackId,callback)
        {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/GetFeedbackThreads?FeedbackId=" + feedbackId, null, callback);
        },
        addNewThread : function(thread, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/AddNewThread" , thread, callback);
        },
        markAllNotificationAsRead :function(callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Notification/markAllNotificationAsRead",null,  callback);
        },
        fetchSurveyQuestionForTeam: function (traineeId, startDate,endDate, callback)
        {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/FetchWeeklySurveyQuestionForTeam?traineeId=" + traineeId + "&startDate=" + startDate + "&endDate=" + endDate, null, callback);
        },
        
        fetchWeeklyFeedbackPreview : function(surveyResponse) {
            return my.ajaxService.ajaxPostDeffered(my.rootUrl + "/Profile/FetchWeeklyFeedbackPreview", surveyResponse);
        },
        
        saveWeeklySurveyResponse: function (surveyResponse,callback)
        {
             my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/SaveWeeklySurveyResponseForTrainee", surveyResponse, callback);
        }
    };
}(my));