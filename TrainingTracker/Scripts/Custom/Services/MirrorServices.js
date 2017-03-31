$(document).ready(function (my) {
    "use strict";
    my.mirrorService = {
        getAssignmentFeedbackWithFiltersPromise: function(userId, startDate, endDate) {
            return my.ajaxService.ajaxGetDefferedCustomLoader(my.rootUrl + "/Mirror/GetAssignmentFeedbackWithFilters?userId=" + userId + "&startDate=" + startDate + "&endDate=" + endDate, null);
        },
        getCodeReviewWithFiltersPromise: function(userId, startDate, endDate) {
            return my.ajaxService.ajaxGetDefferedCustomLoader(my.rootUrl + "/Mirror/GetCodeReviewFeedbackWithFilters?userId=" + userId + "&startDate=" + startDate + "&endDate=" + endDate, null);
        },
        getRandomReviewWithFiltersPromise: function(userId, startDate, endDate) {
            return my.ajaxService.ajaxGetDefferedCustomLoader(my.rootUrl + "/Mirror/GetRandomReviewFeedbackWithFilters?userId=" + userId + "&startDate=" + startDate + "&endDate=" + endDate, null);
        },
        getSkillsWithFiltersPromise: function(userId, startDate, endDate) {
            return my.ajaxService.ajaxGetDefferedCustomLoader(my.rootUrl + "/Mirror/GetSkillsFeedbackWithFilters?userId=" + userId + "&startDate=" + startDate + "&endDate=" + endDate, null);
        },
        getWeeklyFeedbacksWithFiltersPromise: function(userId, startDate, endDate) {
            return my.ajaxService.ajaxGetDefferedCustomLoader(my.rootUrl + "/Mirror/GetWeeklyFeedbackWithFilters?userId=" + userId + "&startDate=" + startDate + "&endDate=" + endDate, null);
        },
    };
}(my));