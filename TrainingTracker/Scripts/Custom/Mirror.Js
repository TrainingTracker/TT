$(document).ready(function () {
    my.mirrorVm = function () {
        var settings =
        {

            filteredTrainee: ko.observableArray([]),
            filterKeyword: ko.observable(""),
            filterForReports: {
                Feedback: {
                    Assignment: ko.observable(true),
                    CodeReview: ko.observable(true),
                    RandomReview: ko.observable(true),
                    Skills: ko.observable(true),
                    WeeklyFeedback: ko.observable(true),
                },
                Session: ko.observable(true),
                Courses: ko.observable(true),
                AllSelected: ko.observable(true),
                AllFeedback: ko.observable(true),
            },
            filterDateRange: {
                StartDate: ko.observable(moment(new Date()).format('MM/DD/YYYY')),
                EndDate: ko.observable(moment(new Date()).format('MM/DD/YYYY'))
            },
            selectedTrainee: {
                IsSelected: ko.observable(false),
                UserId: ko.observable(),
                FullName: ko.observable(),
                Email: ko.observable(),
                ImagePath: ko.observable(),
                UserName: ko.observable(),
                DateAddedToSystem: ko.observable(moment(new Date()).format('MM/DD/YYYY'))
            },
            panel: {
                emptyPanel: ko.observable(true),
                summaryPanel: ko.observable(false),
                reportPanel: ko.observable(false)
            }
        }

        var photoUrl = function (pictureName) {
            return my.rootUrl + "/Uploads/ProfilePicture/" + (pictureName || "dummy.jpg");
        };

        // my.meta.allTrainee()

        var filterTrainee = function () {
            if (my.isNullorEmpty(my.meta.allTrainee()) || my.isNullorEmpty(settings.filterKeyword().trim())) {
                settings.filteredTrainee([]);
                return;
            }

            var filteredTrainee = ko.utils.arrayFilter(my.meta.allTrainee(), function (item) {
                return item.FullName.toUpperCase().includes(settings.filterKeyword().trim().toUpperCase()) || item.UserName.toUpperCase().includes(settings.filterKeyword().toUpperCase());
            });
            settings.filteredTrainee(filteredTrainee);
        };

        var setSelectedTrainee = function (traineeId) {
            var filteredTrainee = ko.utils.arrayFilter(settings.filteredTrainee(), function (item) {
                return traineeId == item.UserId;
            });

            if (filteredTrainee.length == 0) return;

            settings.selectedTrainee.UserId(filteredTrainee[0].UserId);
            settings.selectedTrainee.FullName(filteredTrainee[0].FullName);
            settings.selectedTrainee.Email(filteredTrainee[0].Email);
            settings.selectedTrainee.ImagePath(photoUrl(filteredTrainee[0].ProfilePictureName));
            settings.selectedTrainee.UserName(filteredTrainee[0].UserName);
            settings.selectedTrainee.DateAddedToSystem(moment(filteredTrainee[0].DateAddedToSystem).format("MM/DD/YYYY"));
            settings.filterDateRange.StartDate(moment(filteredTrainee[0].DateAddedToSystem).format("MM/DD/YYYY"));
            settings.selectedTrainee.IsSelected(true);

            // hide show reporting panel
            settings.panel.emptyPanel(true);
            settings.panel.summaryPanel(false);
            settings.panel.reportPanel(false);
            clearSearchSelection();
        }

        var clearSearchSelection = function () {
            settings.filterKeyword("");
            settings.filteredTrainee([]);
        }

        var manageCustomCheckbox = function () {
            manageSelectAllFeedbackCheckbox();
            manageSelectAllCheckbox();
            return true;
        }

        var manageAllFeedbackClick = function () {
            if (settings.filterForReports.AllFeedback()) {
                settings.filterForReports.Feedback.Assignment(true);
                settings.filterForReports.Feedback.CodeReview(true);
                settings.filterForReports.Feedback.RandomReview(true);
                settings.filterForReports.Feedback.Skills(true);
                settings.filterForReports.Feedback.WeeklyFeedback(true);
            } else {
                settings.filterForReports.Feedback.Assignment(false);
                settings.filterForReports.Feedback.CodeReview(false);
                settings.filterForReports.Feedback.RandomReview(false);
                settings.filterForReports.Feedback.Skills(false);
                settings.filterForReports.Feedback.WeeklyFeedback(false);
            }
            manageCustomCheckbox();
            return true;
        }

        var manageSelectAllCheckboxClick = function () {
            if (settings.filterForReports.AllSelected()) {
                settings.filterForReports.Session(true);
                settings.filterForReports.AllFeedback(true);
                settings.filterForReports.Courses(true);
                settings.filterForReports.Feedback.Assignment(true);
                settings.filterForReports.Feedback.CodeReview(true);
                settings.filterForReports.Feedback.RandomReview(true);
                settings.filterForReports.Feedback.Skills(true);
                settings.filterForReports.Feedback.WeeklyFeedback(true);
            }
            else {
                settings.filterForReports.Session(false);
                settings.filterForReports.Courses(false);
                settings.filterForReports.AllFeedback(false);
                settings.filterForReports.Feedback.Assignment(false);
                settings.filterForReports.Feedback.CodeReview(false);
                settings.filterForReports.Feedback.RandomReview(false);
                settings.filterForReports.Feedback.Skills(false);
                settings.filterForReports.Feedback.WeeklyFeedback(false);
            }
            return true;
        }

        var manageSelectAllCheckbox = function () {
            if (settings.filterForReports.AllFeedback()
                && settings.filterForReports.Session()
                && settings.filterForReports.Courses()) {
                settings.filterForReports.AllSelected(true);
                return true;
            }
            settings.filterForReports.AllSelected(false);
        }

        var manageSelectAllFeedbackCheckbox = function() {
            if (settings.filterForReports.Feedback.Assignment()
                && settings.filterForReports.Feedback.CodeReview()
                && settings.filterForReports.Feedback.RandomReview()
                && settings.filterForReports.Feedback.Skills()
                && settings.filterForReports.Feedback.WeeklyFeedback()) {
                settings.filterForReports.AllFeedback(true);
                return true;
            }
            settings.filterForReports.AllFeedback(false);
        };

        var loadSummaryPanel = function () {

            if (typeof (my.mirrorSummaryVm) == 'undefined') return; // add alert for failure in load of plugin
            // hide show reporting panel
            settings.panel.emptyPanel(false);
            settings.panel.summaryPanel(true);
            settings.panel.reportPanel(false);

            my.mirrorSummaryVm.intitalizeMirrorSummaryPlugin(settings.selectedTrainee.UserId(),
                                                               settings.filterDateRange.StartDate(),
                                                             settings.filterDateRange.EndDate(),
                                                             settings.filterForReports.Feedback.Assignment(),
                                                             settings.filterForReports.Feedback.CodeReview(),
                                                             settings.filterForReports.Feedback.RandomReview(),
                                                             settings.filterForReports.Feedback.Skills(),
                                                             settings.filterForReports.Feedback.WeeklyFeedback(),
                                                             settings.filterForReports.Courses(),
                                                             settings.filterForReports.Session());
        };

        var setTraineeView= function() {
            if (my.meta.isTrainee() === true) {
                settings.selectedTrainee.UserId(my.meta.currentUser.UserId);
                settings.selectedTrainee.FullName(my.meta.currentUser.FirstName + " " + my.meta.currentUser.LastName);
                settings.selectedTrainee.Email(my.meta.currentUser.Email);
                settings.selectedTrainee.ImagePath(photoUrl(my.meta.currentUser.ProfilePictureName));
                settings.selectedTrainee.UserName(my.meta.currentUser.UserName);
                settings.selectedTrainee.DateAddedToSystem(moment(my.meta.currentUser.DateAddedToSystem).format("MM/DD/YYYY"));
                settings.filterDateRange.StartDate(moment(my.meta.currentUser.DateAddedToSystem).format("MM/DD/YYYY"));
                settings.selectedTrainee.IsSelected(true);
            }
        }

        var loadReportPanel = function () {

            if (typeof (my.mirrorReportVm) == 'undefined') return; // add alert for failure in load of plugin
            // hide show reporting panel
            settings.panel.emptyPanel(false);
            settings.panel.summaryPanel(false);
            settings.panel.reportPanel(true);

            my.mirrorReportVm.intitalizeMirrorReportPlugin(settings.selectedTrainee.UserId(),
                                                             settings.filterDateRange.StartDate(),
                                                             settings.filterDateRange.EndDate(),
                                                             settings.filterForReports.Feedback.Assignment(),
                                                             settings.filterForReports.Feedback.CodeReview(),
                                                             settings.filterForReports.Feedback.RandomReview(),
                                                             settings.filterForReports.Feedback.Skills(),
                                                             settings.filterForReports.Feedback.WeeklyFeedback(),
                                                             settings.filterForReports.Courses(),
                                                             settings.filterForReports.Session());
        };

        return {
            settings: settings,
            photoUrl: photoUrl,
            filterTrainee: filterTrainee,
            setSelectedTrainee: setSelectedTrainee,
            manageCustomCheckbox: manageCustomCheckbox,
            manageSelectAllCheckboxClick: manageSelectAllCheckboxClick,
            manageAllFeedbackClick: manageAllFeedbackClick,
            loadSummaryPanel: loadSummaryPanel,
            loadReportPanel: loadReportPanel,
            setTraineeView: setTraineeView
        }
    }();

    // subscribe the layout observable on promise re-load
    my.meta.allTrainee.subscribe(function () {
        my.mirrorVm.filterTrainee();
    });

    my.meta.isTrainee.subscribe(function () {
        if (my.meta.isTrainee() == true) {
            my.mirrorVm.setTraineeView();
        }      
    });

    if (my.meta.isTrainee() == true) {
        my.mirrorVm.setTraineeView();
    }

    ko.applyBindings(my.mirrorVm);

});