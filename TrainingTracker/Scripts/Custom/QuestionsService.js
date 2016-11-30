$(document).ready(function (my) {
    "use strict";
    my.questionsService = {
        getQuestionsVm: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Questions/GetQuestionsVm", null, callback);
        },
        getQuestionsBySkillAndExperience: function (skillId, startExperience, endExperience, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Questions/GetQuestionsBySkillAndExperience?skillId="
                + skillId + "&startExperience=" + startExperience + "&endExperience=" + endExperience, null, callback);
        },
        getQuestionsBySkillAndUserId: function (skillId, userId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Questions/GetQuestionsBySkillAndUserId?skillId=" + skillId + "&userId=" + userId, null, callback);
        },
        getQuestionsByUserId: function (userId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Questions/GetQuestionsByUserId?userId=" + userId, null, callback);
        },
        addQuestion: function (question, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Questions/AddQuestion", question, callback);
        },
        addCategory: function (category, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Questions/AddCategory", category, callback);
        }
    };
}(my));