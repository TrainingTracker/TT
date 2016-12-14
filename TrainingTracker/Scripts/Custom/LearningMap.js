$(document).ready(function () {

    my.learningMap = function () {

        var getLearningMapCallback = function (jsonData) {
            if (jsonData !== null) {
                ko.applyBindings(my.learningMap);
            }
        };

        var getLearningMap = function () {
            my.learningMapService.getLearningMapWithAllData(1, getLearningMapCallback);
        };

        return {
            getLearningMap: getLearningMap
        }
    }();
    my.learningMap.getLearningMap();
});