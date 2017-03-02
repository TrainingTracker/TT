$(document).ready(function () {
    my.sessionVm = function () {
        var attendies = ko.observableArray([]);
        var todayDate = new Date();
        var sessions = {
            CurrentPage: ko.observable(1),
            PageCount: ko.observable(0),
            PageSize: ko.observable(0),
            RowCount: ko.observable(0),
            DisplaySessions: ko.observableArray([])
        };

        var currentSession = {
            Id: ko.observable(0),
            Title: ko.observable(""),
            Description: ko.observable(""),
            Date: ko.observable(""),
            Presenter: ko.observable(0),
            Attendee: ko.observableArray([]),
            VideoFileName: ko.observable(""),
            SlideName: ko.observable(""),

        };

        var alerts = {
            postValidation: ko.observable(""),
            postAddedSuccess: ko.observable("")
        };

        var settings = {
            statusId: ko.observable(0),
            wildcard: ko.observable(""),
            searchSessionId: ko.observable(my.queryParams["sessionId"]),
            newSessionPanel: ko.observable(false),
            allSelectedText: ko.observable("Select All"),
            allSelected: ko.observable(false),
            EditAllowed: ko.observable(false),
            sessionVideo : ko.observable({
                dataURL: ko.observable("")
            }),
            sessionSlide : ko.observable({
                dataURL: ko.observable("")
            }),
        };

        var photoUrl = function (pictureName) {
            return my.rootUrl + "/Uploads/ProfilePicture/" + (pictureName || "dummy.jpg");
        };

        var getSessionVm = function () {
            if (!settings.searchSessionId()) {
                settings.searchSessionId(0);
            }
            my.sessionService.getSessionsVm(sessions.CurrentPage(),
                settings.wildcard(),
                settings.statusId(),
                settings.searchSessionId(),
                getSessionVmCallback);
        };

        var getSessionVmCallback = function (response) {
            if (response) {
                attendies(response.AllAttendees);
                callbackFillData(response);
            }
        };

        var getSessionOnFilterCallback = function (response) {
            if (response) {
                callbackFillData(response);
            }
        }

        var callbackFillData = function (response) {
            sessions.CurrentPage(response.SessionList.CurrentPage);
            sessions.PageCount(response.SessionList.PageCount);
            sessions.PageSize(response.SessionList.PageSize);
            sessions.RowCount(response.SessionList.RowCount);
            sessions.DisplaySessions([]);
            $.each(response.SessionList.Results, function (arrayId, item) {
                sessions.DisplaySessions.push(item);
            });
            loadDefaultSession(response.DefaultSession);
        }

        var getSessionOnFilter = function () {
            if (!settings.searchSessionId()) {
                settings.searchSessionId(0);
            }

            my.sessionService.getSessionsOnFilter(sessions.CurrentPage(),
                settings.wildcard(),
                settings.statusId(),
                settings.searchSessionId(),
                getSessionOnFilterCallback);
        }

        var loadDefaultSession = function (response) {
            if (response) {
                currentSession.Id(response.Id);
                currentSession.Title(response.Title),
                currentSession.Description(response.Description),
                currentSession.Date(moment(response.Date).format('MM/DD/YYYY')),
                currentSession.Presenter(response.Presenter),
                commaSeparatedUserIdForAttendies(response.SessionAttendees),
                currentSession.VideoFileName(response.VideoFileName),
                currentSession.SlideName(response.SlideName);
            }
            settings.newSessionPanel(false);
            //  settings.EditAllowed(false);
        };

        var commaSeparatedUserIdForAttendies = function (sessionAttendies) {
            currentSession.Attendee([]);

            ko.utils.arrayForEach(sessionAttendies, function (user) {
                if (currentSession.Attendee().indexOf(user.UserId) < 0) {
                    currentSession.Attendee.push(user.UserId);
                }
            });
            observeAttendee();
        }

        var getNextPage = function () {
            sessions.CurrentPage(sessions.CurrentPage() + 1);
            getSessionOnFilter();
        };

        var getPreviousPage = function () {
            sessions.CurrentPage(sessions.CurrentPage() - 1);
            getSessionOnFilter();
        };

        var resetFilterAndGetData = function () {
            sessions.CurrentPage(1);
            sessions.PageCount(0);
            sessions.PageSize(0);
            sessions.RowCount(0);
            getSessionOnFilter();
        };

        var loadSessionData = function (sessionId) {
            settings.EditAllowed(false);
            my.sessionService.getSessionsOnFilter(1, "", 0, sessionId, loadSessionDataCallback);
        };

        var editSessionData = function (sessionId) {
            settings.EditAllowed(true);
            my.sessionService.getSessionsOnFilter(1, "", 0, sessionId, loadSessionDataCallback);

        }

        var loadSessionDataCallback = function (response) {

            loadDefaultSession(response.DefaultSession);
        };

        var checkboxSelectAll = function () {
            var activeAttendies = ko.utils.arrayFilter(attendies(), function (user) {
                return user.IsActive === true;
            });

            if (activeAttendies.length == currentSession.Attendee().length || settings.allSelectedText() == 'Uncheck to clear all') {
                settings.allSelectedText("Check to select all");
                settings.allSelected(false);
                currentSession.Attendee([]);
                return false;
            }

            settings.allSelectedText("Uncheck to clear all");
            settings.allSelected(true);
            currentSession.Attendee([]);
            ko.utils.arrayForEach(activeAttendies, function (user) {
                currentSession.Attendee.push(user.UserId);
            });
            return true;
        };

        var observeAttendee = function () {

            var activeAttendies = ko.utils.arrayFilter(attendies(), function (user) {
                return user.IsActive === true;
            });

            if (currentSession.Attendee().length == activeAttendies.length) {
                settings.allSelectedText("Uncheck to clear all");
                settings.allSelected(true);
            } else if (currentSession.Attendee().length == 0) {
                settings.allSelectedText("Check to select all");
                settings.allSelected(false);
            }
            return true;
        };

        var loadAddSessionPanel = function () {
            currentSession.Id(0);
            currentSession.Title("");
            currentSession.Description("");
            currentSession.Date("");
            currentSession.Presenter(my.meta.currentUser);
            currentSession.Attendee([]);
            currentSession.VideoFileName("");
            currentSession.SlideName("");
            settings.newSessionPanel(true);
            settings.EditAllowed(false);
            observeAttendee();
        };

        var getUserImage = function (userId) {
            var activeAttendies = ko.utils.arrayFilter(attendies(), function (user) {
                return user.UserId == userId;
            });

            if (activeAttendies.length != 1) {
                return "Dummy.jpg";
            }
            return activeAttendies[0].ProfilePictureName;
        };

        var getUserFullName = function (userId) {
            var activeAttendies = ko.utils.arrayFilter(attendies(), function (user) {
                return user.UserId == userId;
            });

            if (activeAttendies.length != 1) {
                return "Undefined";
            }
            return activeAttendies[0].FirstName + ' ' + activeAttendies[0].LastName;

        }

        var redirectToUserProfile = function (userId) {
            window.location = my.rootUrl + "/Profile/UserProfile?userId=" + userId;
        }

        var validateAndAddUpdateSession = function () {
            if (validateSessionDetails()) {

                var session =
                {
                    Id: currentSession.Id(),
                    Title: currentSession.Title,
                    Description: currentSession.Description(),
                    Date: currentSession.Date(),
                    VideoFileName: currentSession.VideoFileName(),
                    SlideName: currentSession.SlideName(),
                    SessionAttendees: []
                }

                ko.utils.arrayForEach(currentSession.Attendee(), function (user) {
                    var sessionTrainees =
                    {
                        UserId: user
                    }
                    session.SessionAttendees.push(sessionTrainees);
                });

                if (session.Id == 0) {
                    my.sessionService.addNewSession(session, getSessionOnFilterCallback);
                }
                else {
                    my.sessionService.updateSessionDetails(session);
                }
            }
        }

        var validateSessionDetails = function () {
            if (my.isNullorEmpty(currentSession.Title()) || my.isNullorEmpty(currentSession.Description()) || my.isNullorEmpty(currentSession.Date())) {
                alerts.postValidation("All fields are mandatory.");
                return false;
            } else if (currentSession.Attendee().length == 0) {
                alerts.postValidation("Session should have atleast one Attendee");
                return false;
            }
            alerts.postValidation("");
            return true;
        };

        var cancelAddEditSession = function () {
            settings.EditAllowed(false);
            settings.newSessionPanel(false);
            getSessionOnFilter();
        };

        var uploadVideoCallback = function (jsonData) {

            if (!my.isNullorEmpty(jsonData)) {
                currentSession.VideoFileName(jsonData);
                validateAndAddUpdateSession();
            }
            settings.sessionVideo().clear();
        };

        var uploadVideo = function (newValue)
        {
            var formData = new FormData($('#videoUploadForm')[0]);

            my.sessionService.uploadVideo(formData, uploadVideoCallback, errorCallback);
        };

        var uploadSlide = function () {
          
            var formData = new FormData($('#slideUploadForm')[0]);
            my.sessionService.uploadSlide(formData, uploadSlideCallback, errorCallback);
        };

        var uploadSlideCallback = function (jsonData) {

            if (!my.isNullorEmpty(jsonData)) {
                currentSession.SlideName(jsonData);
                validateAndAddUpdateSession();
            }
            settings.sessionSlide().clear();
        };

        var loadSessionVideo = function () {
            if (!my.isNullorEmpty(currentSession.VideoFileName())) {
                $('#videoModal').modal('show');
                var myPlayer = videojs("my-video");
                var fileName = my.rootUrl + "/Uploads/SessionVideo/" + currentSession.VideoFileName();
                myPlayer.pause().src(fileName).load().play();
            }           
        };

        var downloadSessionSlide = function () {
            if (!my.isNullorEmpty(currentSession.SlideName())) {
                window.location.assign(my.rootUrl + "/Uploads/SessionSlide/" + currentSession.SlideName());
            }
        };

        var errorCallback = function() {
            $.alert({
                title: 'Failed Import!',
                columnClass: 'col-md-6 col-md-offset-3 col-sm-8 col-sm-offset-2 col-xs-10',
                useBootstrap: true,
                content: 'Unable to Import the File!',
            });
        };

        return {
            attendies: attendies,
            getSessionVm: getSessionVm,
            currentSession: currentSession,
            AllSessions: sessions.DisplaySessions,
            getNextPage: getNextPage,
            getPreviousPage: getPreviousPage,
            resetFilterAndGetData: resetFilterAndGetData,
            loadSessionData: loadSessionData,
            settings: settings,
            photoUrl: photoUrl,
            todayDate: todayDate,
            sessions: sessions,
            alerts: alerts,
            checkboxSelectAll: checkboxSelectAll,
            observeAttendee: observeAttendee,
            loadAddSessionPanel: loadAddSessionPanel,
            getUserImage: getUserImage,
            getUserFullName: getUserFullName,
            redirectToUserProfile: redirectToUserProfile,
            validateAndAddUpdateSession: validateAndAddUpdateSession,
            editSessionData: editSessionData,
            cancelAddEditSession: cancelAddEditSession,
            uploadVideo: uploadVideo,
            uploadSlide: uploadSlide,
            downloadSessionSlide: downloadSessionSlide,
            loadSessionVideo: loadSessionVideo
        };
    }();

    my.sessionVm.settings.sessionVideo().dataURL.subscribe(function (dataURL) {
        if (my.isNullorEmpty(my.sessionVm.settings.sessionVideo().dataURL())) return;

        my.sessionVm.uploadVideo();
    });

    my.sessionVm.settings.sessionSlide().dataURL.subscribe(function (dataURL) {

        if (my.isNullorEmpty(my.sessionVm.settings.sessionSlide().dataURL())) return;
        my.sessionVm.uploadSlide();
    });

    ko.applyBindings(my.sessionVm);
    my.sessionVm.getSessionVm();
});


