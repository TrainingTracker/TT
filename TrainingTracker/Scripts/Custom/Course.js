$(document).ready(function () {

    my.courseVm = function () {
        var courseInfo = {
            CourseId: my.queryParams["courseId"],
            Course: ko.observable(),
            Icon: my.rootUrl + "/Uploads/CourseIcon/DefaultCourse.jpg",
        },
        showSelectedTopic = function (topic) {
            my.courseVm.selectedTopic.Name(topic.Name);
            my.courseVm.selectedTopic.Description(topic.Description);
            my.courseVm.selectedTopic.SubtopicContents([]);
            $.each(topic.SubtopicContents, function (arrayId, item) {
                my.courseVm.selectedTopic.SubtopicContents.push(item);
            });
        },
        selectedTopic = {
                Name: ko.observable(""),
                Description: ko.observable(""),
                SubtopicContents: ko.observableArray([])
        },
        getCourseCallback = function (jsonData) {
            if (jsonData !== null) {
                my.courseVm.courseInfo.Course = jsonData;
                my.courseVm.courseInfo.Icon = my.rootUrl + "/Uploads/CourseIcon/" + jsonData.Icon;
                my.courseVm.showSelectedTopic(my.courseVm.courseInfo.Course.CourseSubtopics[0]);
                ko.applyBindings(my.courseVm);
            }
        },
        getCourse = function () {
            my.courseService.getCourseWithAllData(my.courseVm.courseInfo.CourseId, getCourseCallback);
        };


        return {
            courseInfo: courseInfo,
            getCourse: getCourse,
            selectedTopic: selectedTopic,
            showSelectedTopic: showSelectedTopic
        }
    }();
    my.courseVm.getCourse();
});