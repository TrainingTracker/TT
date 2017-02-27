$(document).ready(function (my) {
    "use strict";
    my.sessionService = {
        uploadVideo: function (videoFileObject, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/Session/UploadVideo" , videoFileObject , callback);
        },
        uploadSlide: function (presentationFileObject, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/Session/UploadSlide" , presentationFileObject , callback);
        },
        getSessionsOnFilter: function (currentPage, searchKeyword, statusId, sessionId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Session/GetSessionsOnFilter?pageNumber=" + currentPage + "&seminarType=" + statusId + "&searchKeyword=" + searchKeyword + "&sessionId=" + sessionId, null, callback);
        },
        getSessionsVm: function (currentPage, searchKeyword, statusId, sessionId, callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Session/GetSessionVm?pageNumber=" + currentPage + "&seminarType=" + statusId + "&searchKeyword=" + searchKeyword + "&sessionId=" + sessionId, null, callback);
        },
        addNewSession: function (session, getSessionVmCallback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Session/AddNewSession", session, getSessionVmCallback);
        },
        updateSessionDetails: function (session, getSessionVmCallback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Session/UpdateSessionDetails", session, getSessionVmCallback);
        }
    };
}(my));