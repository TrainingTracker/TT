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
            IsEditInProgress: ko.observable(false),
            IsPublished: ko.observable(false),
            CreatedOn: '',
            IsEditInProgress: ko.observable(false)

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
            IsEditInProgress: ko.observable(false)
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
            IsEditInProgress: ko.observable(false)
        }

        var editorContent = {
            Id: ko.observable(0),
            Name: ko.observable(''),
            Description: ko.observable(''),
            Icon: ko.observable('DefaultCourse.jpg'),
            IconUrl: function () {
                return my.rootUrl + "/Uploads/CourseIcon/" + editorContent.Icon();
            },
            HasIcon: ko.observable(false),
            Url: ko.observable(''),
            HasUrl: ko.observable(false),
            SortOrder: ko.observable(''),
            HasSortOrder: ko.observable(false),
            HasContent: ko.observable(false),
            ContentType : ko.observable('')

        }

        var assignment = {
            Id: ko.observable(0),
            Name: ko.observable(''),
            Description: ko.observable(''),

        }

        var searchKeyword = ko.observable('');

        var courseSearchHasFocus = ko.observable(false);

        var courseList = ko.observableArray([]);

        var filteredCourseList = ko.observableArray([]);

        var subtopicsList = ko.observableArray([]);

        var subtopicContentsList = ko.observableArray([]);

        var assignmentsList = ko.observableArray([]);

        var courseId = my.queryParams["courseId"];

        /******** Reset Funtions**********/

        var resetEditor = function () {
            editorContent.Id(0);
            editorContent.Name('');
            editorContent.Description('');
            editorContent.Icon('DefaultCourse.jpg');
            editorContent.HasIcon(false);
            editorContent.Url('');
            editorContent.HasUrl(false);
            editorContent.SortOrder('');
            editorContent.HasSortOrder(false);
            editorContent.ContentType('');
        }

        var resetSubtopicsList = function () {
            subtopicsList([]);
        }

        var resetSubtopicContentsList = function () {
            subtopicContentsList([]);
        }

        var resetAssignmentsList = function () {
            assignmentsList([]);
        };

        var unselectListItems = function (list) {
            ko.utils.arrayForEach(list(), function (item, index) {
                if (item.hasOwnProperty('IsSelected')) {
                    item.IsSelected(false);
                }
            });
        }

        var resetEditInProgressKeyOfListItems = function (list) {
            ko.utils.arrayForEach(list(), function (item, index) {
                if (item.hasOwnProperty('IsEditInProgress')) {
                    item.IsEditInProgress(false);
                }
            });
        }

        /***************************/

        var getCourseWithSubtopicsCallback = function (jsonData) {
            if (jsonData !== null) {
                course.Id(jsonData.Id);
                course.Name(jsonData.Name);
                course.Description(jsonData.Description);
                course.Icon(jsonData.Icon);
                course.IconUrl = function () {
                    return my.rootUrl + "/Uploads/CourseIcon/" + course.Icon();
                };
                course.AddedBy = jsonData.AddedBy;
                course.IsPublished(jsonData.IsPublished);
                course.CreatedOn = jsonData.CreatedOn;

                ko.utils.arrayForEach(jsonData.CourseSubtopics, function (item) {
                    var subtopicClone = ko.mapping.fromJS(ko.mapping.toJS(subtopic));
                    
                    subtopicClone.Id(item.Id);
                    subtopicClone.Name(item.Name);
                    subtopicClone.CourseId = item.CourseId;
                    subtopicClone.Description(item.Description),
                    subtopicClone.AddedBy = item.AddedBy;
                    subtopicClone.CreatedOn = item.CreatedOn;
                    subtopicClone.SortOrder(item.SortOrder);

                    subtopicsList.push(subtopicClone);
                });
            }
            else {
                alert('no courses found');
            }

            ko.applyBindings(my.courseEditorVm);
        };

        var getCourseWithSubtopics = function () {
            if (courseId > 0) {
                my.courseService.getCourseWithSubtopics(courseId, getCourseWithSubtopicsCallback);
            }
            else {
                //For course Id undefined or 0 need to so some operation
                alert(courseId);
            }
        };


        var uploadImageCallback = function (jsonData) {
            if (!my.isNullorEmpty(jsonData)) {
                editorContent.Icon(jsonData);
            }
            else {
                alert("some error occured while uploading...");
            }
        };

        var uploadImage = function (data, event) {
            
            var fileUpload = data[0];
            if (fileUpload.files.length > 0) {
                var files = fileUpload.files;
                var fileData = new FormData();
                fileData.append(files[0].name, files[0]);

                my.courseService.uploadImage(fileData, uploadImageCallback);
            }
            else {
                alert("no file choosen");
            }
            
        };

        var getAssignmentsCallback = function (jsonData) {
            resetAssignmentsList();
            if (jsonData !== null) {
                ko.utils.arrayForEach(jsonData, function (item) {
                    var assignmentClone = ko.mapping.fromJS(ko.mapping.toJS(assignment));

                    assignmentClone.Id(item.Id);
                    assignmentClone.Name(item.Name);
                    assignmentClone.Description(item.Description),

                    assignmentsList.push(assignmentClone);
                });
            }
            else {
                alert('no assignmnet found');
            }
        }

        var getAssignments = function (data) {
            my.courseService.getAssignments(data.Id(), getAssignmentsCallback);
        }

        var getSubtopicContentsCallback = function (jsonData) {
            if (jsonData !== null) {
                resetSubtopicContentsList();

                ko.utils.arrayForEach(jsonData, function (item, index) {
                    var content = ko.mapping.fromJS(ko.mapping.toJS(subtopicContent));
                    content.Id(item.Id);
                    content.CourseSubtopicId = item.CourseSubtopicId;
                    content.Name(item.Name);
                    content.Description(item.Description);
                    content.Url(item.Url);
                    content.AddedBy = item.AddedBy;
                    content.IsActive = true;
                    content.CreatedOn = item.CreatedOn;
                    content.SortOrder(item.SortOrder);

                    subtopicContentsList.push(content);
                });
            }
            else {
                alert('No contents found');
            }
        };

        var getSubtopicContents = function (data) {

            if (data.hasOwnProperty('IsSelected')) {
                unselectListItems(subtopicsList);
                data.IsSelected(true);
            }
            my.courseService.getSubtopicContents(data.Id(), getSubtopicContentsCallback);
        };

        var getFilteredCourses = function () {
            if (courseList().length == 0)
            {
                getAllCourses();
            }
            filteredCourseList([]);
            var filter = searchKeyword().toLowerCase();

            /******* In first call courseList is not updating( WHY??? )********/

            if (filter !== "") {
                ko.utils.arrayForEach(courseList(), function (item) {
                    //if (ko.utils.stringStartsWith(item.Name.toLowerCase(), filter)) {
                    if (item.Name.toLowerCase().indexOf(filter) !== -1) {
                        filteredCourseList.push(item);
                    }
                });
            }
        };

        
        var getAllCoursesCallback = function (jsonData) {
            ko.utils.arrayForEach(jsonData, function (item) {
                courseList.push(item);
            });
        };

        var getAllCourses = function () {
            my.courseService.getAllCourses(getAllCoursesCallback);
        };

        var edit = function (data, dataType) {
            
            
            editorContent.ContentType(dataType);

            if (dataType == 'course') {
                data = data.course;
                editorContent.HasIcon(true);
                editorContent.Icon(data.Icon());
                editorContent.IconUrl = function () {
                    return my.rootUrl + "/Uploads/CourseIcon/" + editorContent.Icon();
                };
                editorContent.HasUrl(false);
                editorContent.HasSortOrder(false);
            }
            else if (dataType == 'subtopic') {
                
                editorContent.HasUrl(false);
                editorContent.SortOrder(data.SortOrder());
                editorContent.HasSortOrder(true);
                editorContent.HasIcon(false);

                if (data.hasOwnProperty('IsSelected')) {
                    unselectListItems(subtopicsList);
                    data.IsSelected(true);
                }
                
            }
            else if (dataType == 'subtopicContent') {

                editorContent.SortOrder(data.SortOrder());
                editorContent.HasSortOrder(true);
                editorContent.HasIcon(false);

                editorContent.HasUrl(true);
                editorContent.Url(data.Url());
                if (data.hasOwnProperty('IsSelected')) {
                    unselectListItems(subtopicContentsList);
                    data.IsSelected(true);
                }
                
            }

            resetEditInProgressKeyOfListItems(subtopicsList);
            resetEditInProgressKeyOfListItems(subtopicContentsList);
            course.IsEditInProgress(false);
            data.IsEditInProgress(true);

            editorContent.HasContent(true);
            editorContent.Id(data.Id());
            editorContent.Name(data.Name());
            editorContent.Description(data.Description());
           

        };

        return {
            course: course,
            subtopicsList: subtopicsList,
            subtopicContentsList: subtopicContentsList,
            assignmentsList : assignmentsList,
            editorContent: editorContent,
            searchKeyword: searchKeyword,
            filteredCourseList: filteredCourseList,
            courseSearchHasFocus : courseSearchHasFocus,
            
            uploadImage : uploadImage,
            edit: edit,
            getAssignments : getAssignments,
            getSubtopicContents : getSubtopicContents,
            getCourseWithSubtopics: getCourseWithSubtopics,
            getFilteredCourses: getFilteredCourses
        }
    }();
    
    my.courseEditorVm.getCourseWithSubtopics();
    my.courseEditorVm.searchKeyword.subscribe(function () {
        my.courseEditorVm.getFilteredCourses();
    });
});