$(document).ready(function () {

    my.learningMap = function () {
       

        var allCourses = ko.observableArray([]);
        var allLearningMaps = ko.observableArray([]);
        var allTrainees = ko.observableArray([]);

        var getLearningMapWithAllDataCallback = function (jsonData) {
           
        };
        var getLearningMapWithAllData = function (id) {
            my.learningMapService.getLearningMapWithAllData(id, getLearningMapWithAllDataCallback);
        };

        var getAllCoursesCallback = function (jsonData) {
            if (jsonData !== null) {
                ko.utils.arrayForEach(jsonData, function (item) {
                    allCourses.push(item);
                });

                ko.applyBindings(my.learningMap);
            }
        };
        var getAllCourses = function () {
            my.learningMapService.getAllCourses(getAllCoursesCallback);
        };

        var getAllLearningMapsCallback = function (jsonData) {
            if (jsonData !== null) {
                ko.utils.arrayForEach(jsonData, function (item) {
                    allLearningMaps.push(item);
                });

            }
        };
        var getAllLearningMaps = function () {
            my.learningMapService.getAllLearningMaps(getAllLearningMapsCallback);
        };

        var getAllTraineesCallback = function (jsonData) {
            if (jsonData !== null) {
                ko.utils.arrayForEach(jsonData, function (item) {
                    allTrainees.push(item);
                });
            }
        };
        var getAllTrainees = function () {
            my.learningMapService.getAllTrainees(getAllTraineesCallback);
        };

        var addLearningMapCallback = function (jsonData) {

        };
        var addLearningMap = function (data) {
            my.learningMapService.addLearningMap(data, addLearningMapCallback);
        };

        var updateLearningMapCallback = function (jsonData) {

        };
        var updateLearningMap = function () {
            my.learningMapService.updateLearningMap(updateLearningMapCallback);
        };


        return {

            allCourses : allCourses,
            allLearningMaps : allLearningMaps,
            allTrainees : allTrainees,

            
            getAllCourses: getAllCourses,
            getAllLearningMaps : getAllLearningMaps,
            getAllTrainees: getAllTrainees,

            addLearningMap : addLearningMap,
            updateLearningMap: updateLearningMap
        }
    }();
    my.learningMap.getAllLearningMaps();
    my.learningMap.getAllCourses();
    my.learningMap.getAllTrainees();
});