$(document).ready(function () {
    my.allCoursesVm = function () {
        var searchKeyword = ko.observable("");
        var allCourses = ko.observableArray();

        var loadCourses = function() {
			
            my.courseService.filterCourses(searchKeyword(), loadCoursesCallback);
        };

        var loadCoursesCallback = function (courses)
        {
            allCourses([]);
            ko.utils.arrayForEach(courses, function (sub)
            {
                allCourses.push(sub);
            });
           
        };

        var navigateToCourse = function(courseId) {
            window.location.href = my.rootUrl + 'CourseEditorNew?courseId=' + courseId;
        };
        
        return {
            searchKeyword: searchKeyword,
            allCourses:allCourses,
            loadCourses: loadCourses,
            navigateToCourse: navigateToCourse
        };
    }();
    ko.applyBindings(my.allCoursesVm);
    my.allCoursesVm.loadCourses();
    
});