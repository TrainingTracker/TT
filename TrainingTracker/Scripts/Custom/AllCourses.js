$(document).ready(function () {
    my.allCoursesVm = function () {
        var searchKeyword = ko.observable("").extend({ throttle: 200, notify: 'always' });
        var serverReturnedCourse = [];
        var allCourses = ko.observableArray();

        var loadCourses = function() {
			
            my.courseService.filterCourses(searchKeyword(), loadCoursesCallback);
        };

        var loadCoursesCallback = function (courses)
        {
            serverReturnedCourse = courses;
            allCourses([]);
            ko.utils.arrayForEach(courses, function (sub)
            {
                allCourses.push(sub);
            });
           
        };

        var navigateToCourse = function(courseId) {
            window.location.href = my.rootUrl + '/LearningPath/CourseEditorNew?courseId=' + courseId;
        };

        var filterCourse = function ()
        {
            var courses = [];
            
            if (my.isNullorEmpty(my.allCoursesVm.searchKeyword()))
            {
                courses = serverReturnedCourse;
            }
            else
            {
                courses = ko.utils.arrayFilter(serverReturnedCourse, function (item)
                {
                    return item.Name.toUpperCase().includes(searchKeyword().trim().toUpperCase()) || item.Description.toUpperCase().includes(searchKeyword().trim().toUpperCase());
                });
            }
                     
            allCourses([]);
            ko.utils.arrayForEach(courses, function (sub)
            {
                allCourses.push(sub);
            });
           
        };
        
        return {
            searchKeyword: searchKeyword,
            allCourses:allCourses,
            loadCourses: loadCourses,
            navigateToCourse: navigateToCourse,
            filterCourse: filterCourse
           
        };
    }();
    ko.applyBindings(my.allCoursesVm);
    my.allCoursesVm.loadCourses();

    my.allCoursesVm.searchKeyword.subscribe(function ()
    {        
        my.allCoursesVm.filterCourse();
    });

});