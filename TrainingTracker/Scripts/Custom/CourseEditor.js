$(document).ready(function () {

    
    my.addCourseVm = function () {
      
        var courseToAdd = {
            Id : 0,
            Name : ko.observable(''),
            Description: ko.observable(''),
            Icon: ko.observable('DefaultCourse.jpg'),
            IconUrl : function () {
                return my.rootUrl + "/Uploads/CourseIcon/" + Icon();
            },
            AddedBy: 0,
            IsActive: true,
            CreatedOn: '',
            CourseSubtopics: ko.observableArray([]),
            SubtopicNameToAdd : ko.observable('')
        };

        var subtopicToAdd = {
            Id: 0,
            CourseId: 0,
            Name: ko.observable(''),
            Description: ko.observable(''),
            AddedBy: 0,
            IsActive: true,
            CreatedOn: '',
            SortOrder: ko.observable('')
        };

        var subtopicContentToAdd = {
            Id:ko.observable(0),
            CourseSubtopicId: 0,
            Name: ko.observable(''),
            Description: ko.observable(''),
            Url: ko.observable(''),
            AddedBy: 0,
            IsActive: true,
            CreatedOn: '',
            SortOrder: ko.observable('')
        }
       
        var currentSubtopic = {
            Id: 0,
            CourseId: 0,
            Name: ko.observable(''),
            Description: ko.observable(''),
            AddedBy: 0,
            IsActive: true,
            CreatedOn: '',
            SortOrder: ko.observable(''),
            IsVisible: ko.observable(false)
        };
        
        var selectedSubtopicContent = {
            Id: ko.observable(0),
            CourseSubtopicId: 0,
            Name: ko.observable(''),
            Description: ko.observable(''),
            Url: ko.observable(''),
            AddedBy: 0,
            IsActive: true,
            CreatedOn: '',
            SortOrder: ko.observable(''),
            IsVisible: ko.observable(false),
            ShowUrlField: ko.observable(false)
        };

       
        //var hideCourseEditorPanel = ko.observable(true);
        var courses = ko.observableArray([]);
        var currentSubtopicContents = ko.observableArray([]);
        var currentAssignments = ko.observableArray(['Assignment1', 'Assignment2', 'Assignment3']);

        var courseIndexForIconUpdate = null;

        var uploadImageCallback = function (jsonData) {
            if (!my.isNullorEmpty(jsonData) && !my.isNullorEmpty(courseIndexForIconUpdate)) {
                courses()[courseIndexForIconUpdate].Icon(jsonData);
                courseIndexForIconUpdate = null;
            }
            else {
                alert("some error occured while uploading...");
            }
        };

        var uploadImage = function (data, event) {
            courseIndexForIconUpdate = ko.contextFor(data).$index();

            if (my.isNullorEmpty(courseIndexForIconUpdate)) {
                alert('No course selected');
            }
            else {
                var fileUpload = data[0];
                if ( fileUpload.files.length > 0) {
                    var files = fileUpload.files;
                    var fileData = new FormData();
                    fileData.append(files[0].name, files[0]);

                    my.courseService.uploadImage(fileData, uploadImageCallback);
                }
                else {
                    alert("no file choosen");
                }
            }
        
        };

        var getAllCoursesCallback = function (jsonData) {
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

        var getAllCourses = function ()
        {
            my.courseService.getAllCourses(getAllCoursesCallback);
        };

        var addCourseCallback = function (jsonData) {
            if (jsonData != 0) {
                
                var addedCourse = ko.toJS(courseToAdd);
                addedCourse.IconUrl = function () {
                    return my.rootUrl + "/Uploads/CourseIcon/" + addedCourse.Icon;
                },
                addedCourse.Id = jsonData;
                courses.push(addedCourse);

                courseToAdd.Name("");
            }
        };

        var addCourse = function () {
            if (courseToAdd.Name() != "") {
                my.courseService.addCourse(courseToAdd, addCourseCallback);
            }
            else {
                alert('Name cannot be empty');
            }

            
        };

        var updateCourseCallback = function (jsonData) {
            if (jsonData) {
                alert("updated");
            }
        };

        var updateCourse = function (data, event) {
            var context = ko.contextFor(event.target);
            var index = context.$index();
            selectedCourseIndex = index;

            if (courses()[index].Name() != "") {
                my.courseService.updateCourse(courses()[index], updateCourseCallback)
            }
            else {
                alert("Empty Field");
            }

        };

        var addSubtopicCallback = function(jsonData){
            if (jsonData != 0 ) {
                ko.utils.arrayForEach(courses(), function (course) {
                    if (course.Id == subtopicToAdd.CourseId) {
                        var addedSubtopic = ko.toJS(subtopicToAdd);
                        addedSubtopic.Id = jsonData;
                        course.CourseSubtopics.push(addedSubtopic);
                        course.SubtopicNameToAdd("");
                    }
                });

            }
            else {
                alert('some error occured while adding');
            }
        }

        var addSubtopic = function (data, event) {

            var context = ko.contextFor(event.target);
            var index = context.$index();
            if (courses()[index].SubtopicNameToAdd() != "") {
                subtopicToAdd.CourseId = courses()[index].Id;
                subtopicToAdd.Name = courses()[index].SubtopicNameToAdd();
                my.courseService.addSubtopic(subtopicToAdd, addSubtopicCallback)
            }
            else {
                alert('Name cannot be empty');
            }

        };

        var getSubtopicContentsCallback = function (jsonData) {

            if (jsonData !== null) {
                currentSubtopicContents.splice(0, currentSubtopicContents().length);
                currentSubtopicContents.push(subtopicContentToAdd);

                ko.utils.arrayForEach(jsonData, function (item) {
                    item.Id = ko.observable(item.Id);
                    item.Name = ko.observable(item.Name);
                    item.Description = ko.observable(item.Description);
                    item.Url = ko.observable(item.Url);
                    item.SortOrder = ko.observable(item.SortOrder);

                    currentSubtopicContents.push(item);
                });
               
                selectedSubtopicContent.Description('');
                selectedSubtopicContent.Name('');
                selectedSubtopicContent.Url('');
                selectedSubtopicContent.IsVisible(false);
            }
            else {
                alert('No contents found');
            }

        };

        var getSubtopicContents = function (data, event) {
            var data = ko.toJS(data);
            currentSubtopic.Id = data.Id;
            currentSubtopic.CourseId = data.CourseId;
            currentSubtopic.Name(data.Name);
            currentSubtopic.Description(data.Description);
            currentSubtopic.AddedBy = data.AddedBy;
            currentSubtopic.IsActive = true;
            currentSubtopic.CreatedOn = data.CreatedOn;
            currentSubtopic.SortOrder(data.SortOrder);
            currentSubtopic.IsVisible(true);

            my.courseService.getSubtopicContents(data.Id, getSubtopicContentsCallback);
            
        };

        //var getAssignmentsCallback = function (jsonData) {
        //    ko.utils.arrayForEach(jsonData, function (item) {
        //        item.Id = ko.observable(item.Id);
        //        item.Name = ko.observable(item.Name);
        //        item.Description = ko.observable(item.Description);
        //        currentAssignments().push(item);
        //    });
        //};

        var editSubtopicContent = function (data, event) {
            selectedSubtopicContent.Id(data.Id());
            if (data.CourseSubtopicId == 0)
            {
                data.CourseSubtopicId = currentSubtopic.Id;
            }
            selectedSubtopicContent.CourseSubtopicId = data.CourseSubtopicId;
            selectedSubtopicContent.Name(data.Name);
            selectedSubtopicContent.Description(data.Description());
            selectedSubtopicContent.Url(data.Url);
            selectedSubtopicContent.AddedBy = data.Id;
            selectedSubtopicContent.IsActive = data.IsActive;
            selectedSubtopicContent.CreatedOn = data.CreatedOn;
            selectedSubtopicContent.SortOrder(data.SortOrder);
            selectedSubtopicContent.IsVisible(true);
            selectedSubtopicContent.ShowUrlField(true);

            //my.courseService.getAssignments(data.Id(), getAssignmentsCallback);

            //selectedContentIndex = ko.contextFor(event.target).$index();
        };
    
        var updateSubtopicCallback = function (jsonData) {
            if (jsonData) {
                var courseIndex = -1, subtopicIndex = -1;
                ko.utils.arrayForEach(courses(), function (item, index) {
                    if (item.Id == currentSubtopic.CourseId) {
                        courseIndex = index;
                    }
                });
                var courseList = courses()[courseIndex].CourseSubtopics();
                ko.utils.arrayForEach(courseList, function (item, index) {
                    if (item.Id == currentSubtopic.Id) {
                        subtopicIndex = index;
                    }
                });
                 courses()[courseIndex].CourseSubtopics()[subtopicIndex].Name = currentSubtopic.Name;
                 courses()[courseIndex].CourseSubtopics()[subtopicIndex].Description = currentSubtopic.Description;
                 courses()[courseIndex].CourseSubtopics()[subtopicIndex].SortOrder = currentSubtopic.SortOrder;
                alert('Updated');
            }
            else {
                alert('Some error occured');
            }
        };

        var updateSubtopic = function () {
            if (currentSubtopic.Name() !== "" && currentSubtopic.Name() !== null) {
                my.courseService.updateSubtopic(currentSubtopic, updateSubtopicCallback);
            }
            else {
                alert('Name cannot be empty');
            }
        };

        var deleteSubtopicCallback = function (jsonData) {
            if (jsonData) {
                var courseIndex = -1, subtopicIndex = -1;
                ko.utils.arrayForEach(courses(), function (item, index) {
                    if(item.Id == currentSubtopic.CourseId){
                        courseIndex = index;
                    }
                });

                ko.utils.arrayForEach(courses()[courseIndex].CourseSubtopics, function (item, index) {
                    if (item.Id == currentSubtopic.Id) {
                        subtopicIndex = index;
                    }
                });
                courses()[courseIndex].CourseSubtopics.splice(subtopicIndex, 1);
                courseIndex = -1;
                subtopicIndex = -1;

                currentSubtopic.Id = 0;
                currentSubtopic.Name('');
                currentSubtopic.Description('');
                currentSubtopic.SortOrder('');
                currentSubtopic.IsVisible(false);

                selectedSubtopicContent.Description('');
                selectedSubtopicContent.Name('');
                selectedSubtopicContent.Url('');
                selectedSubtopicContent.IsVisible(false);

                alert('Deleted');
            }
            else {
                alert('Some error occured');
            }
        };

        var deleteSubtopic = function () {
            my.courseService.deleteSubtopic(currentSubtopic.Id, deleteSubtopicCallback);
            
        };

        var updateSubtopicContentCallback = function (jsonData) {
            if (jsonData) {
                alert('updated');
            }
            else {
                alert('some error occured');
            }

        };

        var updateSubtopicContent = function () {
            if (ko.toJS(selectedSubtopicContent.Name()) !== "") {
                my.courseService.updateSubtopicContent(selectedSubtopicContent, updateSubtopicContentCallback);
            }
            else {
                alert("Name cannot be empty");
            }
        };

        var addSubtopicContentCallback = function (jsonData) {
            if (jsonData > 0) {
                var addedSubtopicContent = ko.toJS(selectedSubtopicContent);
                addedSubtopicContent.Id = ko.observable(jsonData);
                addedSubtopicContent.Name = ko.observable(addedSubtopicContent.Name);
                addedSubtopicContent.Description = ko.observable(addedSubtopicContent.Description);
                addedSubtopicContent.Url = ko.observable(addedSubtopicContent.Url);

                currentSubtopicContents.push(addedSubtopicContent);
                
                subtopicContentToAdd.Id(0);
                subtopicContentToAdd.Name('');
                subtopicContentToAdd.Description('');
                subtopicContentToAdd.Url('');
                subtopicContentToAdd.SortOrder('');
                

                selectedSubtopicContent.Description('');
                selectedSubtopicContent.Name('');
                selectedSubtopicContent.Url('');
                
            } else {
                alert('some problem occured while adding');
            }
        };

        var addSubtopicContent = function () {
            if (ko.toJS( selectedSubtopicContent.Name()) !== "") {
                my.courseService.addSubtopicContent(selectedSubtopicContent, addSubtopicContentCallback);
            }
            else {
                alert("Heading cannot be empty");
            }
        };

        var deleteSubtopicContentCallback = function (jsonData) {
            if (jsonData) {
                var contentIndex = -1;
                ko.utils.arrayForEach(currentSubtopicContents(), function (item, index) {
                    if (item.Id() == selectedSubtopicContent.Id()) {
                        contentIndex = index;
                    }
                });

                if (contentIndex !== -1) {
                    currentSubtopicContents.splice(contentIndex, 1);

                    selectedSubtopicContent.Description('');
                    selectedSubtopicContent.Name('');
                    selectedSubtopicContent.Url('');
                    selectedSubtopicContent.IsVisible(false);
                    alert('deleted');
                }
                else {
                    alert('item not found');
                }
            }
            else {
                alert('some error occured');
            }
        };

        var deleteSubtopicContent = function () {
            if (selectedSubtopicContent.Id() > 0) {
                my.courseService.deleteSubtopicContent(selectedSubtopicContent.Id(), deleteSubtopicContentCallback);
            }
            else {
                alert('This content is not saved yet');
            }

        };

        return {
            
            courses : courses,
            courseToAdd: courseToAdd,
            currentSubtopic : currentSubtopic,
            currentSubtopicContents: currentSubtopicContents,
            currentAssignments : currentAssignments,
            selectedSubtopicContent: selectedSubtopicContent,

            addSubtopic : addSubtopic,
            addCourse: addCourse,
            addSubtopicContent: addSubtopicContent,

            addCourseCallback: addCourseCallback,
            addSubtopicCallback: addSubtopicCallback,
            addSubtopicContentCallback: addSubtopicContentCallback,

            updateCourse: updateCourse,
            updateSubtopic: updateSubtopic,
            updateSubtopicContent : updateSubtopicContent,

            updateCourseCallback: updateCourseCallback,
            updateSubtopicCallback: updateSubtopicCallback,
            updateSubtopicContentCallback : updateSubtopicContentCallback,

            deleteSubtopic: deleteSubtopic,
            deleteSubtopicContent: deleteSubtopicContent,

            deleteSubtopicCallback : deleteSubtopicCallback,
            deleteSubtopicContentCallback : deleteSubtopicContentCallback, 

            getAllCourses: getAllCourses,
            getAllCoursesCallback: getAllCoursesCallback,

            uploadImage: uploadImage,
            uploadImageCallback: uploadImageCallback,

            getSubtopicContentsCallback: getSubtopicContentsCallback,
            getSubtopicContents: getSubtopicContents,

            editSubtopicContent: editSubtopicContent,

        };
    }();

    my.addCourseVm.getAllCourses();

});