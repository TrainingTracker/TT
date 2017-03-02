$(document).ready(function (my) {
    "use script";
    my.releaseService = {
        GetReleaseOnFilter: function (keyword,releaseId,pageNumber,callback) {
            my.ajaxService.ajaxGetJson(my.rootUrl + "/Release/GetReleaseOnFilter?keyword="+ keyword + "&releaseId=" + releaseId + "&pageNumber="+pageNumber, null, callback);
        },
        addRelease: function (releaseDetails, callback) {
            my.ajaxService.ajaxPostJson(my.rootUrl + "/Release/AddRelease", releaseDetails, callback);
        }


    };
}(my));