$(document).ready(function () {

    my.courseVm = function () {
        var courseInfo = {
            CourseId: my.queryParams["courseId"],
            TraineeId: my.queryParams["traineeId"],
            Course: ko.observable(),
            Icon: my.rootUrl + "/Uploads/CourseIcon/DefaultCourse.jpg",
        },
        showSelectedTopic = function (topic) {
            if (!my.isNullorEmpty(topic)) {
                my.courseVm.selectedTopic.Id(topic.Id);
                my.courseVm.selectedTopic.Name(topic.Name);
                my.courseVm.selectedTopic.Description(topic.Description);
                my.courseVm.selectedTopic.SubtopicContents([]);
                my.courseVm.selectedTopic.Assignments([]);
                $.each(topic.SubtopicContents, function (arrayId, item) {
                    my.courseVm.selectedTopic.SubtopicContents.push(item);
                });
                $.each(topic.Assignments, function (arrayId, item) {
                    my.courseVm.selectedTopic.Assignments.push(item);
                });
            }
        },
        selectedTopic = {
            Id: ko.observable(0),
            Name: ko.observable(""),
            Description: ko.observable(""),
            SubtopicContents: ko.observableArray([]),
            Assignments: ko.observableArray([])
        },
        getCourseCallback = function (jsonData) {
            if (jsonData !== null) {
                my.courseVm.courseInfo.Course = jsonData;
                ko.utils.arrayForEach(my.courseVm.courseInfo.Course.CourseSubtopics, function (subtopic) {
                    ko.utils.arrayForEach(subtopic.SubtopicContents, function (item) {
                        item.IsCompleted = ko.observable(item.IsCompleted);
                    });
                    ko.utils.arrayForEach(subtopic.Assignments, function (item) {
                        item.IsCompleted = ko.observable(item.IsCompleted);
                    });
                });
                
                my.courseVm.courseInfo.Icon = my.rootUrl + "/Uploads/CourseIcon/" + jsonData.Icon;
                if (my.courseVm.courseInfo.Course.CourseSubtopics.length > 0) {
                    my.courseVm.showSelectedTopic(my.courseVm.courseInfo.Course.CourseSubtopics[0]);
                }
                ko.applyBindings(my.courseVm);
            }
        },
        getCourse = function () {
            if (my.courseVm.courseInfo.TraineeId == undefined) {
                my.courseVm.courseInfo.TraineeId = null;
            }
            my.courseService.getCourseWithAllData(my.courseVm.courseInfo.CourseId, my.courseVm.courseInfo.TraineeId, getCourseCallback);
        },

        saveSubtopicContentProgressCallback = function (jsonData) {

        },

        updateAssignmentProgressCallback = function (jsonData) {

        },

        saveProgress = function (data) {
            if (!data.IsCompleted()) {
                data.IsCompleted(true);
                my.courseService.saveSubtopicContentProgress(data.Id, saveSubtopicContentProgressCallback);
            }
            
        };

        updateAssignmentProgress = function (data) {
            if (!data.IsCompleted()) {
                data.IsCompleted(true);
                my.courseService.updateAssignmentProgress(data, updateAssignmentProgressCallback);
            }
        };

        return {
            courseInfo: courseInfo,
            getCourse: getCourse,
            selectedTopic: selectedTopic,
            showSelectedTopic: showSelectedTopic,
            saveProgress: saveProgress,
            updateAssignmentProgress: updateAssignmentProgress
        }
    }();
    my.courseVm.getCourse();
});