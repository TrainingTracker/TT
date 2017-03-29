$(document).ready(function () {
    my.mirrorSummaryVm = function () {
        var settings = {
            panelVisibility: {
                AssignmentPanel: ko.observable(false),
                CodeReviewPanel: ko.observable(false),
                RandomReviewPanel: ko.observable(false),
                SkillsPanel: ko.observable(false),
                WeeklyFeedbackPanel: ko.observable(false),
                CoursePanel: ko.observable(false),
                SessionPanel: ko.observable(false)
            },
            filterDetails: {
                UserId: ko.observable(0),
                StartDate: ko.observable(null),
                EndDate: ko.observable(null),
            },
            customLoader :
            {
                AssignmentPanel: ko.observable(false),
                CodeReviewPanel: ko.observable(false),
                RandomReviewPanel: ko.observable(false),
                SkillsPanel: ko.observable(false),
                WeeklyFeedbackPanel: ko.observable(false),
                CoursePanel: ko.observable(false),
                SessionPanel: ko.observable(false)
            }
        };

        var panelData =
        {
            AssignmentPanel: ko.observableArray([]),
            CodeReviewPanel: ko.observableArray([]),
            RandomReviewPanel: ko.observableArray([]),
            SkillsPanel: ko.observableArray([]),
            WeeklyFeedbackPanel: ko.observableArray([]),
            CoursePanel: ko.observableArray([]),
            SessionPanel: ko.observableArray([])
        }

        var loadDataUsingPromise = function (serviceMethod, callback) {
            var deferredObject = $.Deferred();

            serviceMethod().done(function (data) {
                callback(data);
                my.toggleLoader();
                deferredObject.resolve(data);
            }).fail(function () {
                
            });
        };

        var loadAssignmentDataWithPromise = function () {
            settings.customLoader.AssignmentPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getAssignmentFeedbackWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
                assignmentReviewLoadCallback); // register call back here
        };

        var assignmentReviewLoadCallback = function(data) {
            console.log(data);
            settings.customLoader.AssignmentPanel(false);
        };

        var loadCodeReviewDataWithPromise = function () {
            settings.customLoader.CodeReviewPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getCodeReviewWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
               codeReviewLoadCallback); // register call back here
        };

        var codeReviewLoadCallback = function(data) {
            console.log(data);
            settings.customLoader.CodeReviewPanel(false);
        };

        var loadRandomReviewDataWithPromise = function () {
            settings.customLoader.RandomReviewPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getRandomReviewWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
                randomReviewLoadCallback); // register call back here
        };

        var randomReviewLoadCallback = function (data) {
            console.log(data);
            settings.customLoader.RandomReviewPanel(false);
        };

        var loadSkillsFeedbackDataWithPromise = function () {
            settings.customLoader.SkillsPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getSkillsWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
                skillReviewLoadCallback); // register call back here
        };

        var skillReviewLoadCallback = function(data) {
            console.log(data);
            settings.customLoader.SkillsPanel(false);
        };

        var loadWeeklyFeedbackDataWithPromise = function () {
            settings.customLoader.WeeklyFeedbackPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getWeeklyFeedbacksWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
               weeklyReviewLoadCallback); // register call back here
        };

        var weeklyReviewLoadCallback = function (data) {
            console.log(data);
            settings.customLoader.WeeklyFeedbackPanel(false);
        };

        var intitalizeMirrorSummaryPlugin = function (userId, startDate, endDate, assignment, codeReview, randomReview, skills, weekly, course, session) {
            settings.filterDetails.UserId(userId);
            settings.filterDetails.StartDate(startDate);
            settings.filterDetails.EndDate(endDate);

            if (typeof (assignment) != 'undefined') {
                settings.panelVisibility.AssignmentPanel(assignment);

                if (assignment) {
                    loadAssignmentDataWithPromise();
                }
            }

            if (typeof (codeReview) != 'undefined') {
                settings.panelVisibility.CodeReviewPanel(codeReview);

                if (codeReview) {
                    loadCodeReviewDataWithPromise();
                }
            }

            if (typeof (randomReview) != 'undefined') {
                settings.panelVisibility.RandomReviewPanel(randomReview);

                if (randomReview) {
                    loadRandomReviewDataWithPromise();
                }
            }

            if (typeof (skills) != 'undefined') {
                settings.panelVisibility.SkillsPanel(skills);

                if (skills) {
                    loadSkillsFeedbackDataWithPromise();
                }
            }

            if (typeof (weekly) != 'undefined') {
                settings.panelVisibility.WeeklyFeedbackPanel(weekly);

                if (weekly) {
                    loadWeeklyFeedbackDataWithPromise();
                }
            }

            if (typeof (course) != 'undefined') {
                settings.panelVisibility.CoursePanel(course);
            }

            if (typeof (session) != 'undefined') {
                settings.panelVisibility.SessionPanel(session);
            }
        };

        return {
            intitalizeMirrorSummaryPlugin: intitalizeMirrorSummaryPlugin,
            settings: settings,
            panelData: panelData
        }
    }();
});