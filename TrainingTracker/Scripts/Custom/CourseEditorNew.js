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
            CourseId: 0,//FK
            CourseSubtopicId: 0,//FK
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
            CourseSubtopicId : 0,
            Name: ko.observable(''),
            Description: ko.observable(''),
            AddedBy: 0,
            IsActive: true,
            CreatedOn: '',
            IsSelected: ko.observable(false),
            IsEditInProgress: ko.observable(false)

        }
        
        var dataToBeRefreshed = null;

        var dataToAdd = null;

        var IsTopicOrderChanged = ko.observable(false);
        var IsSubtopicContentOrderChanged = ko.observable(false);

        var selectedSubtopicId = ko.observable(0);

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
            editorContent.CourseId = 0;
            editorContent.CourseSubtopicId = 0;
            editorContent.Name('');
            editorContent.Description('');
            editorContent.Icon('DefaultCourse.jpg');
            editorContent.HasIcon(false);
            editorContent.Url('');
            editorContent.HasUrl(false);
            editorContent.SortOrder('');
            editorContent.HasSortOrder(false);
            editorContent.ContentType('');
            editorContent.HasContent(false);
        }

        var resetCourse = function () {
            course.Id(0);
            course.Name('');
            course.Description('');
            course.Icon('DefaultCourse.jpg');
            course.IconUrl = function () {
                return my.rootUrl + "/Uploads/CourseIcon/" + course.Icon();
            };
            course.AddedBy = 0;
            course.IsActive = true;
            course.IsEditInProgress(false);
            course.IsPublished(false);
            course.CreatedOn = '';
            
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
                IsSubtopicContentOrderChanged(false);
                IsTopicOrderChanged(false);
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
                course.IconUrl = function () {
                    return my.rootUrl + "/Uploads/CourseIcon/" + course.Icon();
                };
                
                ko.applyBindings(my.courseEditorVm);
                edit(course, 'course');
                //For course Id undefined or 0 need to so some operation
                
            }
        };

        var saveOrderCallback = function (jsonData) {
            if (jsonData) {
                alert('reordered');
            }
            else {
                alert('error');
            }
        }
        var saveOrder = function (type) {
            var sortedList = [];
            if (type == 'subtopic') {
                ko.utils.arrayForEach(subtopicsList(), function (item, index) {
                    item.SortOrder(index + 1);
                    sortedList.push({ id: item.Id(), SortOrder: item.SortOrder() });
                });
                my.courseService.saveSubtopicOrder(sortedList, saveOrderCallback);
                IsTopicOrderChanged(false);
            }
            else if (type == 'subtopicContent') {
                ko.utils.arrayForEach(subtopicContentsList(), function (item, index) {
                    item.SortOrder(index + 1);
                    sortedList.push({ id: item.Id(), SortOrder: item.SortOrder() });
                    });
                my.courseService.saveSubtopicContentOrder(sortedList, saveOrderCallback);
                IsSubtopicContentOrderChanged(false);
            }
            
        }

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
                    assignmentClone.Description(item.Description);
                    assignmentClone.CourseSubtopicId = item.CourseSubtopicId;
                    assignmentClone.AddedBy = item.AddedBy;
                    assignmentClone.CreatedOn = item.CreatedOn;

                    assignmentsList.push(assignmentClone);
                });
            }
            else {
                alert('no assignment found');
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
                    IsSubtopicContentOrderChanged(false);
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

        var navigateToAnotherCourse = function (id) {
            window.location.href = my.rootUrl + '/LearningPath/CourseEditorNew?courseId='+ id;
        }

        var saveChangesCallback = function (jsonData) {
            if (jsonData) {
                dataToBeRefreshed.Name(editorContent.Name());
                dataToBeRefreshed.Description(editorContent.Description());
                if (editorContent.HasUrl()) {
                    dataToBeRefreshed.Url(editorContent.Url());
                }
                if (editorContent.HasIcon()) {
                    dataToBeRefreshed.Icon(editorContent.Icon());
                }
                alert('saved');
            }
            else {
                alert('error occured while in saving data');
            }
        };

        var addChangesCallback = function (jsonData) {
            if (jsonData > 0)
            {
                editorContent.Id(jsonData);

                if (editorContent.ContentType() == 'subtopic') {
                    var newData = ko.mapping.fromJS(ko.mapping.toJS(subtopic));

                    newData.Id(jsonData);
                    newData.Name(dataToAdd.Name());
                    newData.Description(dataToAdd.Description());
                    newData.CourseId = dataToAdd.CourseId;
                    newData.IsEditInProgress(true);
                    newData.IsSelected(true);

                    dataToBeRefreshed = newData;
                    subtopicsList.push(newData);
                    IsTopicOrderChanged(false);
                    selectedSubtopicId(jsonData);

                }
                else if (editorContent.ContentType() == 'subtopicContent') {
                    var newData = ko.mapping.fromJS(ko.mapping.toJS(subtopicContent));

                    newData.Id(jsonData);
                    newData.Name(dataToAdd.Name());
                    newData.Description(dataToAdd.Description());
                    newData.CourseSubtopicId = dataToAdd.CourseSubtopicId;
                    newData.Url(dataToAdd.Url());
                    newData.IsEditInProgress(true);
                    newData.IsSelected(true);

                    dataToBeRefreshed = newData;
                    subtopicContentsList.push(newData);
                    IsSubtopicContentOrderChanged(false);
                }
                else if (editorContent.ContentType() == 'assignment') {
                    var newData = ko.mapping.fromJS(ko.mapping.toJS(assignment));

                    newData.Id(jsonData);
                    newData.Name(dataToAdd.Name());
                    newData.Description(dataToAdd.Description());
                    newData.CourseSubtopicId = dataToAdd.CourseSubtopicId;
                    newData.IsEditInProgress(true);
                    newData.IsSelected(true);

                    dataToBeRefreshed = newData;
                    assignmentsList.push(newData);

                }
                else if (editorContent.ContentType() == 'course') {
                    course.Id(jsonData);
                    course.Name(dataToAdd.Name());
                    course.Description(dataToAdd.Description());
                    course.Icon(dataToAdd.Icon());
                }
            }
            else {
                alert('error occured while in saving data');
            }
        }

        var saveChanges = function () {
            if (validateEditorContents()) {

                if (editorContent.ContentType() == 'course') {
                    if (editorContent.Id() > 0) {
                        my.courseService.updateCourse(editorContent, saveChangesCallback);
                    }
                    else {
                        my.courseService.addCourse(editorContent, addChangesCallback);
                    }
                }
                else if (editorContent.ContentType() == 'subtopic') {
                    if (editorContent.Id() > 0) {
                        my.courseService.updateSubtopic(editorContent, saveChangesCallback);
                    }
                    else {
                        //  editorContent.CourseId = course.Id();
                        my.courseService.addSubtopic(editorContent, addChangesCallback)
                    }
                }
                else if (editorContent.ContentType() == 'subtopicContent') {
                    if (editorContent.Id() > 0) {
                        my.courseService.updateSubtopicContent(editorContent, saveChangesCallback);
                    }
                    else {
                        //editorContent.CourseId = course.Id();
                        //editorContent.CourseSubtopicId = selectedSubtopicId;
                        my.courseService.addSubtopicContent(editorContent, addChangesCallback);
                    }
                }
                else if (editorContent.ContentType() == 'assignment') {
                    if (editorContent.Id() > 0) {
                        my.courseService.updateAssignment(editorContent, saveChangesCallback);
                    }
                    else {
                        my.courseService.addAssignment(editorContent, addChangesCallback);
                    }
                }
                else {
                    alert('no content type matched');
                }
            }
        }

        var deleteCallback = function (jsonData) {
            if (jsonData) {
             
                var indexOfItem = -1;
                if (editorContent.ContentType() == 'course') {
                    
                    selectedSubtopicId(0);
                    resetCourse();
                    resetAssignmentsList();
                    resetSubtopicContentsList();
                    resetSubtopicsList();
                }
                else if (editorContent.ContentType() == 'subtopic') {
                    selectedSubtopicId(0);
                    resetAssignmentsList();
                    resetSubtopicContentsList();
                    ko.utils.arrayForEach(subtopicsList(), function (item, index) {
                        if (item.Id() == editorContent.Id())
                        {
                            indexOfItem = index;
                        }
                    });
                    subtopicsList.splice(indexOfItem, 1);
                    IsTopicOrderChanged(false);
                }
                else if (editorContent.ContentType() == 'subtopicContent') {
                   
                    ko.utils.arrayForEach(subtopicContentsList(), function (item, index) {
                        if (item.Id() == editorContent.Id()) {
                            indexOfItem = index;
                        }
                    });
                    subtopicContentsList.splice(indexOfItem, 1);
                    IsSubtopicContentOrderChanged(false);
                }
                else if (editorContent.ContentType() == 'assignment') {
                    ko.utils.arrayForEach(assignmentsList(), function (item, index) {
                        if (item.Id() == editorContent.Id()) {
                            indexOfItem = index;
                        }
                    });
                    assignmentsList.splice(indexOfItem, 1);
                }
                resetEditor();
            }
            else {
                alert('server refuses to delete');
            }

        }

        var deleteData = function () {
            if (editorContent.ContentType() == 'course') {
                my.courseService.deleteCourse(editorContent.Id(), deleteCallback);
                
            }
            else if (editorContent.ContentType() == 'subtopic') {
                my.courseService.deleteSubtopic(editorContent.Id(), deleteCallback)
            }
            else if (editorContent.ContentType() == 'subtopicContent') {
                my.courseService.deleteSubtopicContent(editorContent.Id(), deleteCallback);
            }
            else if (editorContent.ContentType() == 'assignment') {
                my.courseService.deleteAssignment(editorContent.Id(), deleteCallback);
            }
            else {
                alert('no content type matched');
            }
        
        }

        var edit = function (data, dataType) {
            editorContent.ContentType(dataType);

            if (dataType == 'course') {
                if (data.course !== undefined) {
                    data = data.course;
                }
                
                editorContent.HasIcon(true);
                editorContent.Icon(data.Icon());
                editorContent.IconUrl = function () {
                    return my.rootUrl + "/Uploads/CourseIcon/" + editorContent.Icon();
                };
                editorContent.HasUrl(false);
                editorContent.HasSortOrder(false);

                selectedSubtopicId(0);
                unselectListItems(subtopicsList);
                unselectListItems(subtopicContentsList);
            }
            else if (dataType == 'subtopic') {
                if (data.Id() == 0) {
                    resetSubtopicContentsList();
                }
                selectedSubtopicId(data.Id());
                if (data.CourseId > 0) {
                    editorContent.CourseId = data.CourseId;
                }
                else {
                    editorContent.CourseId = course.Id();
                }
                editorContent.HasUrl(false);
                editorContent.SortOrder(data.SortOrder());
                editorContent.HasSortOrder(true);
                editorContent.HasIcon(false);

                if (data.hasOwnProperty('IsSelected')) {
                    unselectListItems(subtopicsList);
                    unselectListItems(subtopicContentsList);
                    
                }
                
            }
            else if (dataType == 'subtopicContent') {

                if (data.CourseSubtopicId > 0) {
                    editorContent.CourseSubtopicId = data.CourseSubtopicId;
                }
                else {
                    editorContent.CourseSubtopicId = selectedSubtopicId();
                }
                
                editorContent.SortOrder(data.SortOrder());
                editorContent.HasSortOrder(true);
                editorContent.HasIcon(false);

                editorContent.HasUrl(true);
                editorContent.Url(data.Url());
                if (data.hasOwnProperty('IsSelected')) {
                    unselectListItems(subtopicContentsList);
                    unselectListItems(assignmentsList);
                }
                
            }
            else if (dataType == 'assignment') {
                if (data.SubtopicId > 0) {
                    editorContent.CourseSubtopicId = data.SubtopicId;
                }
                else {
                    editorContent.CourseSubtopicId = selectedSubtopicId();
                }
                editorContent.HasSortOrder(false);
                editorContent.HasIcon(false);
                editorContent.HasUrl(false);

                unselectListItems(subtopicContentsList);
                unselectListItems(assignmentsList);
                
            }

            resetEditInProgressKeyOfListItems(subtopicsList);
            resetEditInProgressKeyOfListItems(subtopicContentsList);
            resetEditInProgressKeyOfListItems(assignmentsList);
            course.IsEditInProgress(false);
            

            editorContent.HasContent(true);
            editorContent.Id(data.Id());
            editorContent.Name(data.Name());
            editorContent.Description(data.Description());
            
            if (data.Id() > 0) {
                dataToBeRefreshed = data;
                data.IsEditInProgress(true);
                if (data.hasOwnProperty('IsSelected')) { 
                    data.IsSelected(true);
                }
                dataToAdd = null;
            }
            else {
                dataToAdd = editorContent;
                dataToBeRefreshed = null;
            }

        };

        var validateEditorContents = function () {
            if (editorContent.Name() == '') {
                alert('Heading cannot be empty');
                return false;
            }
            else if (editorContent.HasUrl())
            {   
                var urlRegExp = /^(http[s]?:\/\/){0,1}(www\.){0,1}[a-zA-Z0-9\.\-]+\.[a-zA-Z]{2,5}[\.]{0,1}/;
                if (editorContent.Url() !== '' && !urlRegExp.test(editorContent.Url()))
                {
                    alert('enter proper url');
                    return false;
                }
            }
            return true;
           
        }

        return {
            course: course,
            subtopic: subtopic,
            subtopicContent: subtopicContent,
            assignment: assignment,
            subtopicsList: subtopicsList,
            subtopicContentsList: subtopicContentsList,
            selectedSubtopicId : selectedSubtopicId,
            assignmentsList : assignmentsList,
            editorContent: editorContent,
            searchKeyword: searchKeyword,
            filteredCourseList: filteredCourseList,
            courseSearchHasFocus: courseSearchHasFocus,
            courseList: courseList,
            dataToBeRefreshed: dataToBeRefreshed,
            IsTopicOrderChanged: IsTopicOrderChanged,
            IsSubtopicContentOrderChanged: IsSubtopicContentOrderChanged,

            saveOrder : saveOrder,
            navigateToAnotherCourse: navigateToAnotherCourse,
            uploadImage : uploadImage,
            edit: edit,
            saveChanges: saveChanges,
            deleteData : deleteData,
            getAssignments : getAssignments,
            getSubtopicContents : getSubtopicContents,
            getCourseWithSubtopics: getCourseWithSubtopics,
            getFilteredCourses: getFilteredCourses,
            getAllCourses : getAllCourses
        }
    }();
    
    my.courseEditorVm.getCourseWithSubtopics();
    my.courseEditorVm.searchKeyword.subscribe(function () {
        my.courseEditorVm.getFilteredCourses();
    });
    my.courseEditorVm.subtopicsList.subscribe(function () {
        my.courseEditorVm.IsTopicOrderChanged(true);
        
    });
    my.courseEditorVm.subtopicContentsList.subscribe(function () {
        my.courseEditorVm.IsSubtopicContentOrderChanged(true);
        
    });
});