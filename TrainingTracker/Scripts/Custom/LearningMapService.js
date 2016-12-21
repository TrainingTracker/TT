$(document).ready(function (my) {
    "use strict";
    my.learningMapService = {

        getLearningMapWithAllData: function (learningMapId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningMap/GetLearningMapWithAllData?id=" + learningMapId, null, callback);
        },
        getAllCourses: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningMap/GetAllCourses", null, callback);
        },
        getAllLearningMaps: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningMap/getAllLearningMaps", null, callback);
        },
        getAllTrainees: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningMap/GetAllTrainees", null, callback);
        },
        addLearningMap : function(data, callback){
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/AddLearningMap", data, callback);
        },
        updateLearningMap : function(data, callback){
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/UpdateLearningMap", data, callback);
        }
    };
    
}(my));