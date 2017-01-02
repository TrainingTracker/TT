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

        var searchKeyword = {
            Course: ko.observable(''),
            Trainee: ko.observable(''),
            LearningMap : ko.observable('')
        }

        var resetEditorContent = function () {
            editorContent.Id(-1);
            editorContent.Title('');
            editorContent.Notes('');
            editorContent.Duration(0);
            editorContent.IsCourseRestricted(0);
            editorContent.Courses([]);
            editorContent.Trainees([]);
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
                item.IsVisible = ko.observable(true);
                editorContent.Courses.push(item);
                ko.utils.arrayFirst(availableCourses(), function (v, i) {
                    if (item.Id == v.Id)
                    {
                        availableCourses.splice(i, 1);
                        return true;
                    }
                });

                
            });

            availableTrainees([]);
            ko.utils.arrayForEach(allTrainees(), function (item) {
                availableTrainees.push(item);
            });

            editorContent.Trainees([]);
            ko.utils.arrayForEach(jsonData.Trainees, function (item) {
                item.newlyAdded = ko.observable(false);
                item.IsVisible = ko.observable(true);

                editorContent.Trainees.push(item);

                ko.utils.arrayFirst(availableTrainees(), function (v, i) {
                    if (item.UserId == v.UserId) {
                        availableTrainees.splice(i, 1);
                        return true;
                    }
                });
            });
            
        };

        var getLearningMapWithAllData = function (id) {
            if (id == 0) {
                resetEditorContent();
                editorContent.Id(0);

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
                    item.IsVisible = ko.observable(true);
                    allCourses.push(item);
                });
                notifyStyle();
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
                    item.IsVisible = ko.observable(true);
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
                    item.IsVisible = ko.observable(true);
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
                newLearningMap.IsVisible = ko.observable(true);
                allLearningMaps.push(newLearningMap);

                ko.utils.arrayForEach(editorContent.Trainees(), function (item) {
                    item.newlyAdded(false);
                });

                $.notify(editorContent.Title() + ' is Added', { style: 'customAlert' });
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

                $.notify(editorContent.Title() + ' is Updated', { style: 'customAlert' });
            }
           
        };
        var updateLearningMap = function () {
            if (validateEditorContent()) {

                ko.utils.arrayForEach(editorContent.Courses(), function (item, index) {
                    item.SortOrder = index + 1;
                });

                var newTraineesNames = "";

                // If new Learning Map is added
                if (editorContent.Id() == 0) {
                   
                    // creating trainees name string those are included
                    ko.utils.arrayForEach(editorContent.Trainees(), function (item) {
                        newTraineesNames = newTraineesNames + item.FullName + ", ";
                        
                    });

                    // checkingif any
                    if (editorContent.Trainees().length > 0) {
                        $.confirm({
                            title: 'Confirm!',
                            content: 'Once you assign new Trainees to the Learning Map you would not be able to remove them later.Please confirm that you have assigned <b>' + newTraineesNames.slice(0,-2) + "</b> into <b>" + editorContent.Title() + "</b>",
                            columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                            useBootstrap: true,
                            buttons: {
                                confirm: function () {
                                    my.learningMapService.addLearningMap(editorContent, addLearningMapCallback);
                                },
                                cancel: function () {
                                    $.alert({
                                        title:'Alert!!!',
                                        columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                                        useBootstrap: true,
                                        content: 'Save action for <b>' + editorContent.Title() + '</b> is <em>cancelled</em>'
                                    });
                                }
                            }
                        });
                    }
                    else {
                        my.learningMapService.addLearningMap(editorContent, addLearningMapCallback);
                    }

                }
                else {

                    var learningMapData = ko.mapping.toJS(editorContent);

                    learningMapData.Trainees = [];
                    ko.utils.arrayForEach(editorContent.Trainees(), function (item) {
                        if (item.newlyAdded()) {
                            learningMapData.Trainees.push(item);
                            newTraineesNames = newTraineesNames + item.FullName + ", ";
                        }
                    });

                    if (learningMapData.Trainees.length > 0) {
                        $.confirm({
                            title: 'Confirm!',
                            columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                            useBootstrap: true,
                            content: 'Once you assign new Trainees to the Learning Map you would not be able to remove them later.Please confirm that you have assigned <b>'+ newTraineesNames.slice(0,-2) + "</b> into <b>" + learningMapData.Title + "</b>",
                            buttons: {
                                confirm: function () {
                                    my.learningMapService.updateLearningMap(learningMapData, updateLearningMapCallback);
                                },
                                cancel: function () {
                                    $.alert(
                                    {
                                        title:'Alert!!!',
                                        columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                                        useBootstrap: true,
                                        content: 'Save action for <b>' + editorContent.Title() + '</b> is <em>cancelled</em>'
                                });
                                }
                            }
                        });
                    }
                    else {
                        my.learningMapService.updateLearningMap(learningMapData, updateLearningMapCallback);
                    }
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

        var validateEditorContent = function() {

            if (my.isNullorEmpty(editorContent.Title())) {
                editorContent.IsTitleValidated(false);
                return false;
            }
            if (my.isNullorEmpty(editorContent.Notes())) {
                editorContent.IsNotesValidated(false);
                return false;
            }

            return true;
        };

        var deleteLearningMapCallback = function(jsonData) {
            if (jsonData) {

                ko.utils.arrayFirst(allLearningMaps(), function(v, i) {
                    if (editorContent.Id() == v.Id) {
                        allLearningMaps.splice(i, 1);
                        availableCourses([]);
                        availableTrainees([]);
                        return true;
                    }
                });
                $.notify(editorContent.Title() + ' is Deleted', { style: 'customAlert' });
                resetEditorContent();
            }
        };
        var deleteLearningMap = function () {
            $.confirm({
                title: 'Confirm!',
                columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                useBootstrap: true,
                content: 'Do you really want to <b>delete</b> Learning Map <b>' + editorContent.Title() + '</b>',
                buttons: {
                    confirm: function () {
                        my.learningMapService.deleteLearningMap(editorContent.Id(), deleteLearningMapCallback);
                    },
                    cancel: function () {
                        $.alert({
                            title: 'Alert!!!',
                            columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                            useBootstrap: true,
                            content: 'Delete action for <b>' + editorContent.Title() + '</b> is <em>cancelled</em>'
                    });
                    }
                }
            });
            
        }

        var filterCourse = function () {
            
            if (my.isNullorEmpty(searchKeyword.Course())) {
                ko.utils.arrayForEach(availableCourses(), function (item) {
                    item.IsVisible(true);
                    
                });
            }
            else {
                ko.utils.arrayForEach(availableCourses(), function (item) {
                    if (item.Name.toUpperCase().includes(searchKeyword.Course().trim().toUpperCase())) {
                        item.IsVisible(true);
                    }
                    else {
                        item.IsVisible(false);
                    }
                });
            }

        };

        var filterTrainee = function () {

            if (my.isNullorEmpty(searchKeyword.Trainee())) {
                ko.utils.arrayForEach(availableTrainees(), function (item) {
                    item.IsVisible(true);

                });
            }
            else {
                ko.utils.arrayForEach(availableTrainees(), function (item) {
                    if (item.FullName.toUpperCase().includes(searchKeyword.Trainee().trim().toUpperCase())) {
                        item.IsVisible(true);
                    }
                    else {
                        item.IsVisible(false);
                    }
                });
            }

        };

        var filterLearningMap = function () {

            if (my.isNullorEmpty(searchKeyword.LearningMap())) {
                ko.utils.arrayForEach(allLearningMaps(), function (item) {
                    item.IsVisible(true);

                });
            }
            else {
                ko.utils.arrayForEach(allLearningMaps(), function (item) {
                    if (item.Title().toUpperCase().includes(searchKeyword.LearningMap().trim().toUpperCase())) {
                        item.IsVisible(true);
                    }
                    else {
                        item.IsVisible(false);
                    }
                });
            }

        };


        var notifyStyle = function () {
            $.notify.addStyle('customAlert', {
                html: "<div data-notify-text /div>",
                classes: {
                    base: {
                        "white-space": "nowrap",
                        "color": "white",
                        "font-size": "18px",
                        "background-color": "#194a71",
                        "padding": "5px 15px",
                        "position": "fixed",
                        "top": "9%",
                        "left": "60%",
                        "text-align": "center",
                        "min-width": "20%"

                    },
                    blue: {
                        "background-color": "#14588f"
                    }
                }
            });
        }

        return {

            availableCourses : availableCourses,
            allLearningMaps : allLearningMaps,
            availableTrainees: availableTrainees,
            editorContent: editorContent,
            searchKeyword : searchKeyword,
            allCourses: allCourses,

            removeCourse: removeCourse,
            addCourse: addCourse,
            removeTrainee : removeTrainee,
            addTrainee: addTrainee,

            filterCourse: filterCourse,
            filterTrainee: filterTrainee,
            filterLearningMap: filterLearningMap,

            getAllCourses: getAllCourses,
            getAllLearningMaps : getAllLearningMaps,
            getAllTrainees: getAllTrainees,
            getLearningMapWithAllData : getLearningMapWithAllData,

            updateLearningMap: updateLearningMap,

            deleteLearningMap : deleteLearningMap
        }
    }();

    my.learningMap.searchKeyword.Course.subscribe(function () {
        my.learningMap.filterCourse();
    });
    my.learningMap.searchKeyword.Trainee.subscribe(function () {
        my.learningMap.filterTrainee();
    });
    my.learningMap.searchKeyword.LearningMap.subscribe(function () {
        my.learningMap.filterLearningMap();
    });

    my.learningMap.getAllLearningMaps();
    my.learningMap.getAllCourses();
    my.learningMap.getAllTrainees();
});