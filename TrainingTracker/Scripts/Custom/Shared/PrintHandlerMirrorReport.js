$(document).ready(function () {
    my.printHandlerVm = function () {

        var queryFilters = {
            UserId :  my.queryParams["UserId"] || 0,
            StartDate:moment(my.queryParams["StartDate"]).format('MM/DD/YYYY')  || moment(new Date()).format('MM/DD/YYYY'),
            EndDate: moment(my.queryParams["EndDate"]).format('MM/DD/YYYY') || moment(new Date()).format('MM/DD/YYYY'),
            Include: {
                CodeReview: my.queryParams["cr"] || false,
                WeeklyReview: my.queryParams["wf"] || false,
                WorkLearningTimeLine: my.queryParams["wf"] || false,
                Skills: my.queryParams["s"] || false,
                Assignment: my.queryParams["a"] || false,
                RandomReview: my.queryParams["rr"] || false,
                Session: my.queryParams["sess"] || false,
                Courses: my.queryParams["c"] || false,
                SharingActivities: my.queryParams["wf"] || false
            }      
        };

        var traineeDetails = {
            FullName : ko.observable(""),
            Email: ko.observable(""),
            ProfilePictureFileName : ko.observable(""),
            StartDate: ko.observable(""),
            EndDate: ko.observable(""),
         }

        var printReport = function () {

            loadDataUsingPromise(function () {
                return my.userService.getUserDetailsWithFiltersPromise(queryFilters.UserId);
            }, getUserDetailsLoadCallback, getUserDetailsFailureCallback);

            my.mirrorReportVm.intitalizeMirrorReportPlugin(queryFilters.UserId,
                                                           queryFilters.StartDate,
                                                           queryFilters.EndDate,
                                                           (queryFilters.Include.Assignment == 'true'),
                                                           (queryFilters.Include.CodeReview == 'true'),
                                                           (queryFilters.Include.RandomReview == 'true'),
                                                           (queryFilters.Include.Skills == 'true'),
                                                           (queryFilters.Include.WeeklyReview == 'true'),
                                                           (queryFilters.Include.Courses == 'true'),
                                                           (queryFilters.Include.Session == 'true'));
        };

        var getUserDetailsLoadCallback = function (data) {
            traineeDetails.FullName ( data.FirstName + " " + data.LastName);
            traineeDetails.Email ( data.Email);
            traineeDetails.ProfilePictureFileName(my.avatarUrl(data.ProfilePictureName));
            traineeDetails.StartDate(queryFilters.StartDate);
            traineeDetails.EndDate(queryFilters.EndDate);
            
        };

        var getUserDetailsFailureCallback = function () {
            console.log("error occured while fetching results");
        };

        var loadDataUsingPromise = function (serviceMethod, callback, failureCallback) {
            var deferredObject = $.Deferred();

            serviceMethod().done(function (data) {
                callback(data);
                my.toggleLoader();
                deferredObject.resolve(data);

                setTimeout(function () {
                    $('#divWaitHandler').hide();
                    $('#printWrapper').css('opacity', 1);
                    window.print()
                }, 5000);

            }).fail(function () {
                failureCallback();
            });
        };


        return {
            printReport: printReport,
            traineeDetails: traineeDetails
        };
    }();

    ko.applyBindings(my.printHandlerVm);
    my.printHandlerVm.printReport();   
});