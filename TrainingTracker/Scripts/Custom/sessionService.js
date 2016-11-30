$(document).ready(function (my) {
    "use strict";
    my.sessionService = {
        uploadVideo: function (videoFile, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/Session/UploadVideo", videoFile, callback);
        },
        uploadSlide: function (presentationFile, callback) {
            my.ajaxService.ajaxUploadImage(my.rootUrl + "/Session/UploadSlide", presentationFile, callback);
        },
    };
}(my));