$(document).ready(function (my) {
    "use strict";
    my.mirrorService = {
        getAssignmentFeedbackWithFiltersPromise: function(userId, startDate, endDate) {
            return my.ajaxService.ajaxGetDeffered(my.rootUrl + "/Mirror/GetAssignmentFeedbackWithFilters?userId=" + userId + "&startDate=" + startDate + "&endDate=" + endDate, null);
        },
        getCodeReviewWithFiltersPromise: function(userId, startDate, endDate) {
        },
        getRandomReviewWithFiltersPromise: function(userId, startDate, endDate) {
        },
        getSkillsWithFiltersPromise: function(userId, startDate, endDate) {
        },
        getWeeklyFeedbacksWithFiltersPromise: function(userId, startDate, endDate) {
        },
    };
