﻿$(document).ready(function(my) {
    "use strict";
    my.userService = {
        createUser: function(user, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/CreateUser", user, callback);
        },
        updateUser: function(user, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/UpdateUser", user, callback);
        },
        getAllUsers: function(callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetManageProfileVm", null, callback);
        },
        getActiveUsers: function(callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetActiveUsers", null, callback);
        },
        getUserProfileVm: function(userId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetUserProfileVm?userId=" + userId, null, callback);
        },
        addUserFeedback: function(feedbackPost, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/AddFeedback", feedbackPost, callback);
        },
        authenticateUser: function(userData, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Login/AuthenticateLogin?userName=" + userData.UserName
                + "&password=" + userData.Password, null, callback);
        },
        getCurrentUser: function(callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Login/GetCurrentUser", null, callback);
        },
        getFeedbackonAppliedFilter: function(pageSize, feedbackId, userId, startDate, endDate, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetUserFeedbackOnFilter?pageSize=" + pageSize + "&feedbackType=" + feedbackId + "&userId=" + userId + "&startDate=" + startDate + "&endDate=" + endDate, null, callback);
        },
        getDashboardVm: function(callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Dashboard/GetDashboardData", null, callback);
        },
        addEditSession: function(sessionDetails, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Session/AddEditSession", sessionDetails, callback);
        },
        getSessionsOnFilter: function(pageSize, seminarType, searchKeyword, getSessionsOnFilterCallback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Session/GetUserFeedbackOnFilter?pageSize=" + pageSize + "&seminarType=" + seminarType + "&searchKeyword=" + '', null, getSessionsOnFilterCallback);
        },
        uploadImage: function(imagefile, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/Profile/UploadImage", imagefile, callback);
        },
        getUserFeedbackForPlot: function(traineeId, startDate, endDate, arrayFeedbackType, trainer, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetUserFeedbackOnFilterForPlot?traineeId=" + traineeId + "&startDate=" + startDate + "&endDate=" + endDate + "&trainerId=" + trainer + "&arrayFeedbackType=" + arrayFeedbackType, null, callback);
        },
        getNotification: function(callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Notification/GetNotification", null, callback);
        },
        updateNotification: function(notificationInfo, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Notification/UpdateNotification", notificationInfo, callback);
        },
        getFeedbackWithThreads: function(feedbackId, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/GetFeedbackWithThreads?FeedbackId=" + feedbackId, null, callback);
        },
        getFeedbackThreads: function(feedbackId, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/GetFeedbackThreads?FeedbackId=" + feedbackId, null, callback);
        },
        addNewThread: function(thread, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/AddNewThread", thread, callback);
        },
        markAllNotificationAsRead: function(callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Notification/markAllNotificationAsRead", null, callback);
        },
        fetchSurveyQuestionForTeam: function(traineeId, startDate, endDate, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/FetchWeeklySurveyQuestionForTeam?traineeId=" + traineeId + "&startDate=" + startDate + "&endDate=" + endDate, null, callback);
        },

        fetchWeeklyFeedbackPreview: function(surveyResponse) {
            return my.ajaxService.ajaxPostDeffered(my.rootUrl + "/Profile/FetchWeeklyFeedbackPreview", surveyResponse);
        },

        saveWeeklySurveyResponse: function(surveyResponse, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/SaveWeeklySurveyResponseForTrainee", surveyResponse, callback);
        },
        updateSubscribedTraineee: function(subscribedTraineeList, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Setting/SetEmailPreferences", subscribedTraineeList, callback);
        },
        getSubscribedTraineee: function(callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Setting/GetCurrentUserSubscriptions", null, callback);
        },
        getCurrentUserPromise: function() {
            return my.ajaxService.ajaxGetDeffered(my.rootUrl + "/Login/GetCurrentUser", null);
        },
        getAllActiveUsersPromise: function() {
            return my.ajaxService.ajaxGetDeffered(my.rootUrl + "/Profile/GetActiveUsers", null);
        },
        getAllSkills: function() {
            return my.ajaxService.ajaxGetDeffered(my.rootUrl + "/Profile/GetAllSkills", null);
        },
        getMembersUnderLead: function(callback) {
            return my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetMembersUnderLead", null, callback);
        },
        importGPSUser: function(callback) {
            return my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/ImportGPSUser", null, callback);
        },
        syncGPSUsers: function(callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/SyncGPSUsers", null, callback);
        },
        getAllDesignation: function(callback) {
            return my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/GetAllDesignation", null, callback);
        },
        getUserDetailsWithFiltersPromise: function(userId) {
            return my.ajaxService.ajaxGetDefferedCustomLoader(my.rootUrl + "/Profile/GetUserByUserId?userId=" + userId, null);
        },
        addUpdateCodeReviewDetailsWithPromise: function(codeReview) {
            return my.ajaxService.ajaxPostDefferedCustomLoader(my.rootUrl + "/Profile/SubmitCodeReviewMetaData", codeReview);
        },
        addUpdateTagPointsWithPromise: function(codeReview) {
            return my.ajaxService.ajaxPostDefferedCustomLoader(my.rootUrl + "/Profile/SubmitCodeReviewPoint", codeReview);
        },
        getCodeReviewPreview: function(codeReviewId, isFeedback, callback) {
            return my.ajaxService.ajaxGetJson(my.rootUrl + "/Profile/FetchCodeReviewPreview?codeReviewId=" + codeReviewId + "&isFeedback=" + isFeedback, null, callback);
        },
        getPrevCrPointData: function(traineeId, ratingFilter, callback) {
            return my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/FetchPrevCodeReviewData", { traineeId: traineeId, ratingFilter: ratingFilter }, callback);
        },
        calculateCrRating: function(codeReview, callback) {
            return my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/CalculateCodeReviewRating", codeReview, callback);
        },
        submitCodeReviewFeedback: function(codeReview, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/SubmitCodeReviewFeedback", codeReview, callback);
        },

        discardCodeReviewFeedback: function(codeReviewId, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/DiscardCodeReviewFeedback?codeReviewId=" + codeReviewId, null, callback);
        },

        discardTagFromCodeReviewFeedback: function(codeReviewId, codeReviewTagId, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/DiscardTagFromCodeReviewFeedback?codeReviewId=" + codeReviewId + "&codeReviewTagId=" + codeReviewTagId, null, callback);
        },

        addCategory: function(category, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Profile/AddCategory", category, callback);
        },
        getAllTeams: function(callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Setting/GetTeams", null, callback);
        },
        getCrRatingConfig: function(team, callback) {
            var data = { teamId: team ? team.TeamId : null };
            my.ajaxService.ajaxPostJson(my.rootUrl + '/Setting/GetCrRatingConfig', data, callback);
        },
        updateCrRatingConfig: function (data,callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + '/Setting/UpdateCrRatingConfig', data, callback);
        }
    };
}(my));