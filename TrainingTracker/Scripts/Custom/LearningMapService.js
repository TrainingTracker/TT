$(document).ready(function (my) {
    "use strict";
    my.learningMapService = {

        getLearningMapWithAllData: function (learningMapId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningMap/GetLearningMapWithAllData?id=" + learningMapId, null, callback);
        },
        
    };
}(my));