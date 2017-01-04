$(document).ready(function (my) {
    "use strict";
    my.courseService = {

        addCourse: function (courseData, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/AddCourse", courseData, callback);
        },

        getAllCourses: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/GetAllCourses", null, callback);
        },
        getCourseWithSubtopics: function (courseId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/GetCourseWithSubtopics?courseId=" + courseId, null, callback);
        },
        getCourseWithAllData: function (courseId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/GetCourseWithAllData?courseId=" + courseId, null, callback);
        },
        addSubtopic: function (subtopicData, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/AddCourseSubtopic", subtopicData, callback);
        },

        addSubtopicContent: function (data, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/AddSubtopicContent", data, callback);
        },

        addAssignment : function(data, callback){
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/AddAssignment", data, callback);
        },

        updateCourse: function (courseData, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/UpdateCourse", courseData, callback);
        },

        uploadImage: function (imagefile, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/LearningPath/UploadImage", imagefile, callback);
        },

        uploadFile: function (file, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/LearningPath/UploadFile", file, callback);
        },
        
        getSubtopicContents : function(subtopicId, callback){
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/GetSubtopicContents?subtopicId=" + subtopicId, null, callback);
        },

        updateSubtopic: function (subtopicData, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/UpdateCourseSubtopic", subtopicData, callback);
        },

        updateSubtopicContent: function (data, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/UpdateSubtopicContent", data, callback);
        },

        updateAssignment : function(data, callback){
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/UpdateAssignment", data, callback);
        },

        deleteCourse: function (id, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/DeleteCourse?id=" + id, null, callback);
        },

        deleteSubtopic: function (id, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/DeleteCourseSubtopic?id=" + id, null, callback);
        },

        deleteSubtopicContent: function (id, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/DeleteSubtopicContent?id=" + id, null, callback);
        },
          
        deleteAssignment: function (id, callback)
        {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/DeleteAssignment?id=" + id, null, callback);
        },

        getAssignments: function (id, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/GetAssignments?subtopicContentId=" + id, null, callback);
        },
        
        filterCourses : function(searchKeyword, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/FilterCourses?searchKeyword=" + searchKeyword, null, callback);
        },

        saveSubtopicOrder: function (data, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/SaveSubtopicOrder", data, callback);
        },

        saveSubtopicContentOrder: function (data, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/LearningPath/SaveSubtopicContentOrder", data, callback);
        },

        publishCourse: function (id, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/PublishCourse?id=" + id, null, callback);
        },

        saveSubtopicContentProgress: function (subtopicContentId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/LearningPath/SaveSubtopicContentProgress?subtopicContentId=" + subtopicContentId, null, callback);
        }

    };
}(my));