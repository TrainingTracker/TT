$(document).ready(function () {

    my.learningMap = function () {
       

        var allCourses = ko.observableArray([]);
        var availableCourses = ko.observableArray([]);
        var allLearningMaps = ko.observableArray([]);
        var allTrainees = ko.observableArray([]);
        var availableTrainees = ko.observableArray([]);
        var editorContent = {
            Id : ko.observable(-1),
            Title: ko.observable(''),
            IsTitleValidated : ko.observable(true),
            Notes: ko.observable(''),
            IsNotesValidated : ko.observable(true),
            Duration : ko.observable(0),
            IsCourseRestricted: ko.observable(0),
            Courses : ko.observableArray([]),
            Trainees: ko.observableArray([]),
         }

        var getLearningMapWithAllDataCallback = function (jsonData) {

            editorContent.Id(jsonData.Id);
            editorContent.Title(jsonData.Title);
            editorContent.Notes(jsonData.Notes);
            editorContent.Duration(jsonData.Duration);
            editorContent.IsCourseRestricted(jsonData.IsCourseRestricted);

            availableCourses([]);
            ko.utils.arrayForEach(allCourses(), function (item) {
                availableCourses.push(item);
            });

            editorContent.Courses([]);
            ko.utils.arrayForEach(jsonData.Courses, function (item) {
                editorContent.Courses.push(item);
                var index = -1;
                ko.utils.arrayForEach(availableCourses(), function (v, i) {
                    if (item.Id == v.Id)
                    {
                        index = i;
                    }
                });

                if (index > -1) {
                    availableCourses.splice(index, 1);
                }
                
            });

            availableTrainees([]);
            ko.utils.arrayForEach(allTrainees(), function (item) {
                availableTrainees.push(item);
            });

            editorContent.Trainees([]);
            ko.utils.arrayForEach(jsonData.Trainees, function (item) {
                item.newlyAdded = ko.observable(false);
                editorContent.Trainees.push(item);
                var index = -1;
                ko.utils.arrayForEach(availableTrainees(), function (v, i) {
                    if (item.UserId == v.UserId) {
                        index = i;
                    }
                });
                if (index > -1) {
                    availableTrainees.splice(index, 1);
                }

            });
            
        };

        var getLearningMapWithAllData = function (id) {
            if (id == 0) {
                editorContent.Id(0);
                editorContent.Title('');
                editorContent.Notes('');
                editorContent.Duration(0);
                editorContent.IsCourseRestricted(0);
                editorContent.Courses([]);
                editorContent.Trainees([]);

                availableTrainees([]);
                ko.utils.arrayForEach(allTrainees(), function (item) {
                    availableTrainees.push(item);
                });

                availableCourses([]);
                ko.utils.arrayForEach(allCourses(), function (item) {
                    availableCourses.push(item);
                });
            }
            else{
                my.learningMapService.getLearningMapWithAllData(id, getLearningMapWithAllDataCallback);
            }
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
                    item.Title = ko.observable(item.Title);
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
                    item.newlyAdded = ko.observable(false);
                    allTrainees.push(item);
                });
            }
        };
        var getAllTrainees = function () {
            my.learningMapService.getAllTrainees(getAllTraineesCallback);
        };

        var addLearningMapCallback = function (jsonData) {
            if (jsonData > 0) {
                editorContent.Id(jsonData);
                var newLearningMap = ko.mapping.toJS(editorContent);
                newLearningMap.Title = ko.observable(newLearningMap.Title);
                allLearningMaps.push(newLearningMap);

                ko.utils.arrayForEach(editorContent.Trainees(), function (item) {
                    item.newlyAdded(false);
                });
            }
        };
        var updateLearningMapCallback = function (jsonData) {
            if (jsonData) {
                ko.utils.arrayForEach(allLearningMaps(), function (item) {
                    if (item.Id == editorContent.Id()) {
                        item.Title(editorContent.Title());
                    }
                });

                ko.utils.arrayForEach(editorContent.Trainees(), function (item) {
                    item.newlyAdded(false);
                });
            }
           
        };
        var updateLearningMap = function () {
            if (validateEditorContent()) {

                ko.utils.arrayForEach(editorContent.Courses(), function (item, index) {
                    item.SortOrder = index + 1;
                });

                if (editorContent.Id() == 0) {
                    my.learningMapService.addLearningMap(editorContent, addLearningMapCallback);
                }
                else {
                    var learningMapData = ko.mapping.toJS(editorContent);

                    learningMapData.Trainees = [];
                    ko.utils.arrayForEach(editorContent.Trainees(), function (item) {
                        if (item.newlyAdded()) {
                            learningMapData.Trainees.push(item);
                        }
                    });

                    my.learningMapService.updateLearningMap(learningMapData, updateLearningMapCallback);
                }
            }
        
        };

        var removeCourse = function (index) {
            editorContent.Duration(editorContent.Duration() - editorContent.Courses()[index].Duration);
            availableCourses.push(editorContent.Courses()[index]);
            editorContent.Courses.splice(index, 1);
        };
        var addCourse = function (index) {
            editorContent.Duration(editorContent.Duration() + availableCourses()[index].Duration);
            editorContent.Courses.push(availableCourses()[index]);
            availableCourses.splice(index, 1);
        };
        var removeTrainee = function (index) {
            availableTrainees.push(editorContent.Trainees()[index]);
            editorContent.Trainees.splice(index, 1);
        };
        var addTrainee = function (index) {
            availableTrainees()[index].newlyAdded(true);
            editorContent.Trainees.push(availableTrainees()[index]);
            availableTrainees.splice(index, 1);
        };

        var validateEditorContent = function () {
            
            if (my.isNullorEmpty(editorContent.Title()))
            {
                editorContent.IsTitleValidated(false);
                return false;
            }
            if (my.isNullorEmpty(editorContent.Notes())) {
                editorContent.IsNotesValidated(false);
                return false;
            }
            
            return true;
        }

        return {

            
            availableCourses : availableCourses,
            allLearningMaps : allLearningMaps,
            availableTrainees: availableTrainees,
            editorContent: editorContent,
            allTrainees: allTrainees,
            allCourses : allCourses ,
            
            removeCourse: removeCourse,
            addCourse: addCourse,
            removeTrainee : removeTrainee,
            addTrainee: addTrainee,

            getAllCourses: getAllCourses,
            getAllLearningMaps : getAllLearningMaps,
            getAllTrainees: getAllTrainees,
            getLearningMapWithAllData : getLearningMapWithAllData,

            updateLearningMap: updateLearningMap
        }
    }();
    my.learningMap.getAllLearningMaps();
    my.learningMap.getAllCourses();
    my.learningMap.getAllTrainees();
});