$(document).ready(function () {
    my.mirrorSummaryVm = function () {
        var settings = {
            panelVisibility : {
                AssignmentPanel: ko.observable(false),
                CodeReviewPanel: ko.observable(false),
                RandomReviewPanel: ko.observable(false),
                SkillsPanel: ko.observable(false),
                WeeklyFeedbackPanel: ko.observable(false),
                CoursePanel: ko.observable(false),
                SessionPanel: ko.observable(false)
            },
            traineeDetails: {
                UserId : ko.observable(0)
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

        var intitalizeMirrorSummaryPlugin = function (userId,assignment, codeReview, randomReview, skills, weekly, course, session) {
            settings.traineeDetails.UserId(userId);

            if (typeof (assignment) != 'undefined' && assignment == true) {
                settings.panelVisibility.AssignmentPanel(true);
            }

            if (typeof (codeReview) != 'undefined' && codeReview == true) {
                settings.panelVisibility.CodeReviewPanel(true);
            }

            if (typeof (randomReview) != 'undefined' && randomReview == true) {
                settings.panelVisibility.RandomReviewPanel(true);
            }

            if (typeof (skills) != 'undefined' && skills == true) {
                settings.panelVisibility.SkillsPanel(true);
            }

            if (typeof (weekly) != 'undefined' && weekly == true) {
                settings.panelVisibility.WeeklyFeedbackPanel(true);
            }

            if (typeof (course) != 'undefined' && course == true) {
                settings.panelVisibility.CoursePanel(true);
            }

            if (typeof (session) != 'undefined' && session == true) {
                settings.panelVisibility.SessionPanel(true);
            }
        };

        return{
            intitalizeMirrorSummaryPlugin: intitalizeMirrorSummaryPlugin,
            settings: settings,
            panelData:panelData
        }
    }();
});