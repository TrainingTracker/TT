$(document).ready(function () {

    my.courseEditorVm = function () {

        /********All the Objects *******/
        var course = {
            Id: ko.observable(0),
            Name: ko.observable(''),
            Description: ko.observable(''),
            Icon: ko.observable('DefaultCourse.jpg'),
            IconUrl: function () {
                return my.rootUrl + "/Uploads/CourseIcon/" + Icon();
            },
            AddedBy: 0,
            IsActive: true,
            IsEditInProgress : ko.observable(false),
            IsPublished : ko.observable(false),
            CreatedOn: ''

        };

        var subtopic = {
            Id: ko.observable(0),
            CourseId: 0,
            Name: ko.observable(''),
            Description: ko.observable(''),
            AddedBy: 0,
            IsActive: true,
            CreatedOn: '',
            SortOrder: ko.observable(''),
            IsSelected: ko.observable(false),
            IsEditInProgress : ko.observable(false)
        };

        var subtopicContent = {
            Id: ko.observable(0),
            CourseSubtopicId: 0,
            Name: ko.observable(''),
            Description: ko.observable(''),
            Url: ko.observable(''),
            AddedBy: 0,
            IsActive: true,
            CreatedOn: '',
            SortOrder: ko.observable(''),
            IsSelected: ko.observable(false),
            IsEditInProgress : ko.observable(false)
        }

        var editorContent = {
            Id: ko.observable(0),
            Name: ko.observable(''),
            Description: ko.observable(''),
            Icon: ko.observable('DefaultCourse.jpg'),
            IconUrl: function () {
                return my.rootUrl + "/Uploads/CourseIcon/" + Icon();
            },
            HasIcon: ko.observable(false),
            Url: ko.observable(''),
            HasUrl : ko.observable(false),
            SortOrder: ko.observable(''),
            HasSortOrder: ko.observable(false),

        }

        var subtopicsList = ko.observableArray([]);

        var subtopicContentsList = ko.observableArray([]);

        var courseId = my.queryParams["courseId"];

        /******** Reset Funtions**********/

        var resetEditor = function () {
            editorContent.Id()
        }

        var getCourseCallback = function (jsonData) {
            if (jsonData !== null) {
                ko.utils.arrayForEach(jsonData, function (item) {
                    item.SubtopicNameToAdd = ko.observable("");
                    item.Name = ko.observable(item.Name);
                    item.Description = ko.observable(item.Description);
                    item.Icon = ko.observable(item.Icon);
                    item.IconUrl = function () {
                        return my.rootUrl + "/Uploads/CourseIcon/" + item.Icon();
                    };
                    item.FileData = ko.observable('');
                    // one more thing that can be done is make the properties of objects inside item.CourseSubtopics Array to observable
                    item.CourseSubtopics = ko.observableArray(item.CourseSubtopics);
                    courses.push(item);
                });
            }
            else {
                alert('no courses found');
            }
            ko.applyBindings(my.addCourseVm);
        };

        var getCourse = function () {
            if (courseId > 0) {
                my.courseService.getCourse(getCourseCallback);
            }
            else {
                //For course Id undefined or 0 need to so some operation
                alert(courseId);
            }
        };
    }
    

});