$(document).ready(function (my) {
    "use strict";
    my.sessionService = {
        uploadVideo: function (videoFile, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/Session/UploadVideo", videoFile, callback);
        },
        uploadSlide: function (presentationFile, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/Session/UploadSlide", presentationFile, callback);
        },
        getSessionsOnFilter: function (currentPage, searchKeyword, statusId, sessionId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Session/GetSessionsOnFilter?pageNumber=" + currentPage + "&seminarType=" + statusId + "&searchKeyword=" + searchKeyword + "&sessionId=" + sessionId, null, callback);
        },
        getSessionsVm: function (currentPage, searchKeyword, statusId, sessionId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Session/GetSessionVm?pageNumber=" + currentPage + "&seminarType=" + statusId + "&searchKeyword=" + searchKeyword + "&sessionId=" + sessionId, null, callback);
        }
    };
}(my));