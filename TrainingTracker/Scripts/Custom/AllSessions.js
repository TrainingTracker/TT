$(document).ready(function () {
    //my.sessionVm = function () {
    //    var currentUser = {},
    //        sessionId = my.queryParams["sessionId"],
    //        viewSessionLoaded = false,
    //        todayDate = new Date(),
    //        showDialog = ko.observable(false),
    //        sessionDetails = {
    //            Id: ko.observable(0),
    //            Title: ko.observable(""),
    //            Description: ko.observable(""),
    //            Date: ko.observable(""),
    //            Presenter: ko.observable(0),
    //            Attendee: ko.observableArray(),
    //            VideoFileName: ko.observable(""),
    //            SlideName: ko.observable("")
    //        },
    //        sessionVideo = ko.observable({
    //            dataURL: ko.observable("")
    //        }),
    //        sessionSlide = ko.observable({
    //            dataURL: ko.observable("")
    //        }),

    //        sessionSettings = {
    //            isNewSession: ko.observable(true),
    //            sessionHeader: ko.observable("Add New Session"),
    //            sessionId: ko.observable(0),
    //            errorText: ko.observable(""),
    //            isEditable: ko.observable(false),
    //            allSelected: ko.observable(false),
    //            allSelectedText: ko.observable("Check to select All")
    //        },
    //        showDialogueFunction = function () {
    //            my.sessionVm.showDialog(false);
    //        },
    //        getCurrentUserCallback = function (user) {
    //            my.sessionVm.currentUser = user;m
    //            my.sessionVm.getSessionsOnFilter();
    //        },
    //        getCurrentUser = function () {
    //            my.userService.getCurrentUser(my.sessionVm.getCurrentUserCallback);
    //        },
    //        getSessionsOnFilter = function () {
    //            var pageSize = typeof (my.sessionVm.selectedFilter.pageSize()) == 'undefined' ? 10 : my.sessionVm.selectedFilter.pageSize();
    //            var seminarType = typeof (my.sessionVm.selectedFilter.seminarType()) == 'undefined' ? 0 : my.sessionVm.selectedFilter.seminarType();
    //            my.userService.getSessionsOnFilter(1, seminarType, '', my.sessionVm.getSessionsOnFilterCallback);
    //        },
    //        getSessionsOnFilterCallback = function (sessionJson) {
    //            my.sessionVm.sessions([]);
    //            my.sessionVm.allAttendees([]);

    //            ko.utils.arrayForEach(sessionJson.SessionList, function (item) {
    //                my.sessionVm.sessions.push(item);
    //            });
    //            ko.utils.arrayForEach(sessionJson.AllAttendees, function (item) {

    //                if (item.IsTrainee && item.IsActive) {
    //                    my.sessionVm.allAttendees.push(item);
    //                }
    //            });

    //            if (!my.sessionVm.viewSessionLoaded && typeof (my.sessionVm.sessionId) != 'undefined') {
    //                my.sessionVm.viewSessionLoaded = true;
    //                my.sessionVm.loadSessionDetails(my.sessionVm.sessionId);
    //            }
    //        },
    //        filter = {
    //            pageSize: ko.observableArray(["10", "20", "50"]),
    //            seminarType: ko.observableArray([{ Id: 1, Description: "To Be Presented" }, { Id: 2, Description: "Already Presented" }])
    //        },
    //        selectedFilter = {
    //            pageSize: ko.observable(20),
    //            seminarType: ko.observable()
    //        },
    //        sessions = ko.observableArray(),
    //        allAttendees = ko.observableArray(),
    //        addSession = function () {
    //            if (!my.sessionVm.validateSessionData() || my.sessionVm.sessionDetails.Id() > 0) return;
    //            my.sessionVm.sessionDetails.Presenter(my.sessionVm.currentUser.UserId);
    //            my.userService.addEditSession(my.sessionVm.sessionDetails, my.sessionVm.addEditSessionCallback);
    //        },
    //        editSession = function () {
    //            if (!my.sessionVm.validateSessionData() || my.sessionVm.sessionDetails.Presenter() != my.sessionVm.currentUser.UserId) return;
    //            my.userService.addEditSession(my.sessionVm.sessionDetails, my.sessionVm.addEditSessionCallback);
    //        },

    //        validateSessionData = function () {
    //            if (my.isNullorEmpty(my.sessionVm.sessionDetails.Title()) || my.isNullorEmpty(my.sessionVm.sessionDetails.Description()) || my.isNullorEmpty(my.sessionVm.sessionDetails.Date())) {
    //                my.sessionVm.sessionSettings.errorText("All fields are mandatory.");
    //                return false;
    //            }
    //            else if (my.sessionVm.sessionDetails.Attendee().length == 0) {
    //                my.sessionVm.sessionSettings.errorText("Session should have atleast one Attendees");
    //                return false;
    //            }
    //            my.sessionVm.sessionSettings.errorText("");
    //            return true;
    //        },
    //        addEditSessionCallback = function (sessionJson) {
    //            my.sessionVm.showDialog(false);
    //            my.sessionVm.getSessionsOnFilter();
    //        },
    //        openSessionDailog = function () {
    //            my.sessionVm.sessionSettings.allSelectedText("Check to select all");
    //            my.sessionVm.sessionSettings.allSelected(false);
    //            my.sessionVm.sessionDetails.Id(0);
    //            my.sessionVm.sessionDetails.Description("");
    //            my.sessionVm.sessionDetails.Title("");
    //            my.sessionVm.sessionDetails.Date(moment(new Date()).format('MM/DD/YYYY'));
    //            my.sessionVm.sessionDetails.Presenter(my.sessionVm.currentUser.UserId);
    //            my.sessionVm.sessionSettings.isNewSession(true);
    //            my.sessionVm.sessionDetails.Attendee([]);
    //            sessionSettings.sessionHeader("Add Session Details");
    //            my.sessionVm.sessionSettings.isEditable(false);
    //            my.sessionVm.showDialog(true);
    //        },
    //        checkboxSelectAll = function () {
    //            if (my.sessionVm.sessionDetails.Attendee().length == my.sessionVm.allAttendees().length || my.sessionVm.sessionSettings.allSelectedText() == 'Uncheck to clear all') {
    //                my.sessionVm.sessionSettings.allSelectedText("Check to select all");
    //                my.sessionVm.sessionSettings.allSelected(false);
    //                my.sessionVm.sessionDetails.Attendee([]);
    //                return false;
    //            }

    //            my.sessionVm.sessionSettings.allSelectedText("Uncheck to clear all");
    //            my.sessionVm.sessionSettings.allSelected(false);
    //            my.sessionVm.sessionDetails.Attendee([]);
    //            ko.utils.arrayForEach(my.sessionVm.allAttendees(), function (item) {
    //                if (item.IsTrainee)
    //                    my.sessionVm.sessionDetails.Attendee.push(item.UserId.toString());
    //            });
    //            return true;
    //        },
    //        observeAttendee = function () {
    //            if (my.sessionVm.sessionDetails.Attendee().length == my.sessionVm.allAttendees().length) {
    //                my.sessionVm.sessionSettings.allSelectedText("Uncheck to clear all");
    //                my.sessionVm.sessionSettings.allSelected(true);
    //            }
    //            else if (my.sessionVm.sessionDetails.Attendee().length == 0) {
    //                my.sessionVm.sessionSettings.allSelectedText("Check to select all");
    //                my.sessionVm.sessionSettings.allSelected(false);
    //            }
    //        },

    //        loadSessionDetails = function (id, task) {

    //            var filteredSession = ko.utils.arrayFilter(my.sessionVm.sessions(), function (item) {
    //                return item.Id == id;
    //            });

    //            if (filteredSession.length == 0) return;

    //            my.sessionVm.sessionDetails.Id(filteredSession[0].Id);
    //            my.sessionVm.sessionDetails.Description(filteredSession[0].Description);
    //            my.sessionVm.sessionDetails.Title(filteredSession[0].Title);
    //            my.sessionVm.sessionDetails.Date(moment(filteredSession[0].Date).format('MM/DD/YYYY'));
    //            my.sessionVm.sessionDetails.Presenter(filteredSession[0].Presenter);
    //            my.sessionVm.sessionSettings.isNewSession(false);
    //            my.sessionVm.sessionDetails.Attendee([]);
    //            my.sessionVm.sessionDetails.VideoFileName(filteredSession[0].VideoFileName);
    //            my.sessionVm.sessionDetails.SlideName(filteredSession[0].SlideName);
    //            ko.utils.arrayForEach(filteredSession[0].SessionAttendees, function (item) {
    //                my.sessionVm.sessionDetails.Attendee.push(item.UserId.toString());
    //            });

    //            if (my.sessionVm.sessionDetails.Attendee().length == my.sessionVm.allAttendees().length) {
    //                my.sessionVm.sessionSettings.allSelectedText("Uncheck to clear all");
    //                my.sessionVm.sessionSettings.allSelected(true);
    //            }
    //            else {
    //                my.sessionVm.sessionSettings.allSelectedText("Check to select all");
    //                my.sessionVm.sessionSettings.allSelected(false);
    //            }

    //            var isEditable = (my.sessionVm.sessionDetails.Presenter() === my.sessionVm.currentUser.UserId) && (moment(moment(my.sessionVm.sessionDetails.Date()).format('MM/DD/YYYY')).isSameOrAfter(moment(my.sessionVm.todayDate).format('MM/DD/YYYY')));
    //            my.sessionVm.sessionSettings.isEditable(isEditable);

    //            if (my.sessionVm.sessionSettings.isEditable()) {
    //                sessionSettings.sessionHeader("Edit Session Details");

    //            } else {
    //                sessionSettings.sessionHeader("View Session Details");
    //            }

    //            my.sessionVm.showDialog(true);
    //        },
    //        uploadVideoCallback = function (jsonData) {

    //            if (!my.isNullorEmpty(jsonData))
    //            {
    //                my.sessionVm.sessionDetails.VideoFileName(jsonData);
    //                editSession();
    //            }
    //            my.sessionVm.sessionVideo().clear();
    //        },
    //        uploadVideo = function (newValue) {
    //            if (my.sessionVm.sessionSettings.isNewSession()) return;

    //            var formData = new FormData($('#videoUploadForm')[0]);
    //            my.sessionService.uploadVideo(formData, my.sessionVm.uploadVideoCallback);
    //        },
    //        uploadSlide = function () {
    //            if (my.sessionVm.sessionSettings.isNewSession()) return;

    //            var formData = new FormData($('#slideUploadForm')[0]);
    //            my.sessionService.uploadSlide(formData, my.sessionVm.uploadSlideCallback);
    //        },
    //        uploadSlideCallback = function (jsonData) {

    //            if (!my.isNullorEmpty(jsonData)) {
    //                my.sessionVm.sessionDetails.SlideName(jsonData);
    //                editSession();
    //            }
    //            my.sessionVm.sessionSlide().clear();
    //        },
    //        loadSessionVideo = function (videoFileName) {
    //            var myPlayer = videojs("my-video");
    //            var fileName = my.rootUrl + "/Uploads/SessionVideo/" + videoFileName;
    //            myPlayer.pause().src(fileName).load().play();
    //        },
    //        downloadSessionSlide = function (sessionSlideName) {
    //            if (!my.isNullorEmpty(sessionSlideName)) {
    //                window.location.assign(my.rootUrl + "/Uploads/SessionSlide/" + sessionSlideName);
    //            }
    //        };

    //    return {
    //        //fileData: fileData,
    //        currentUser: currentUser,
    //        getCurrentUser: getCurrentUser,
    //        getCurrentUserCallback: getCurrentUserCallback,
    //        filter: filter,
    //        selectedFilter: selectedFilter,
    //        getSessionsOnFilter: getSessionsOnFilter,
    //        getSessionsOnFilterCallback: getSessionsOnFilterCallback,
    //        sessions: sessions,
    //        allAttendees: allAttendees,
    //        sessionDetails: sessionDetails,
    //        sessionSettings: sessionSettings,
    //        showDialog: showDialog,
    //        showDialogueFunction: showDialogueFunction,
    //        addSession: addSession,
    //        editSession: editSession,
    //        addEditSessionCallback: addEditSessionCallback,
    //        todayDate: todayDate,
    //        loadSessionDetails: loadSessionDetails,
    //        openSessionDailog: openSessionDailog,
    //        validateSessionData: validateSessionData,
    //        checkboxSelectAll: checkboxSelectAll,
    //        sessionId: sessionId,
    //        viewSessionLoaded: viewSessionLoaded,
    //        observeAttendee: observeAttendee,
    //        uploadVideo: uploadVideo,
    //        uploadVideoCallback: uploadVideoCallback,
    //        loadSessionVideo: loadSessionVideo,
    //        sessionVideo: sessionVideo,
    //        sessionSlide: sessionSlide,
    //        uploadSlide: uploadSlide,
    //        uploadSlideCallback: uploadSlideCallback,
    //        downloadSessionSlide: downloadSessionSlide,
    //    };
    //}();

    //my.sessionVm.sessionVideo().dataURL.subscribe(function (dataURL) {
    //    if (my.isNullorEmpty(my.sessionVm.sessionVideo().dataURL())) return;

    //    my.sessionVm.uploadVideo();
    //});

    //my.sessionVm.sessionSlide().dataURL.subscribe(function (dataURL) {

    //   if (my.isNullorEmpty(my.sessionVm.sessionSlide().dataURL())) return;
    //    my.sessionVm.uploadSlide();
    //});

    //my.sessionVm.getCurrentUser();

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
            EditAllowed: ko.observable(false)
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

            my.sessionService.getSessionsOnFilter(1, "", 0, sessionId, loadSessionDataCallback);
        };

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
            observeAttendee();
        };

        var getUserImage = function(userId) {
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

        var redirectToUserProfile = function(userId) {
            window.location = my.rootUrl + "/Profile/UserProfile?userId=" + userId;
        }

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
            redirectToUserProfile: redirectToUserProfile
        };
    }();

    ko.applyBindings(my.sessionVm);
    my.sessionVm.getSessionVm();
});


