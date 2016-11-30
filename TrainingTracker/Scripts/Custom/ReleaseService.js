$(document).ready(function (my) {
    "use script";
    my.releaseService = {
        getAllRelease: function (callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Release/GetAllReleases", null, callback);
        },
        addRelease: function (releaseDetails, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Release/AddRelease", releaseDetails, callback);
        },
        UpdateRelease: function (releaseDetails, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Release/UpdateRelease", releaseDetails, callback);
        }
    };
}(my));