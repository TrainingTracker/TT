$(document).ready(function () {
    my.mirrorReportVm = function () {
        var settings = {
            panelVisibility: {
                AssignmentPanel: ko.observable(false),
                CodeReviewPanel: ko.observable(false),
                RandomReviewPanel: ko.observable(false),
                SkillsPanel: ko.observable(false),
                WeeklyFeedbackPanel: ko.observable(false),
                WeeklyLearningTimeLinePanel: ko.observable(false),
                WeeklyTipPanel: ko.observable(false),
                CoursePanel: ko.observable(false),
                SessionPanel: ko.observable(false)
            },
            filterDetails: {
                UserId: ko.observable(0),
                StartDate: ko.observable(null),
                EndDate: ko.observable(null),
            },
            customLoader:
            {
                AssignmentPanel: ko.observable(false),
                CodeReviewPanel: ko.observable(false),
                RandomReviewPanel: ko.observable(false),
                SkillsPanel: ko.observable(false),
                WeeklyFeedbackPanel: ko.observable(false),
                WeeklyLearningTimeLinePanel: ko.observable(false),
                WeeklyTipPanel: ko.observable(false),
                CoursePanel: ko.observable(false),
                SessionPanel: ko.observable(false)
            },

            chartSettings:
            {
                dataFormat: {
                    countData: [{
                        value: 0,
                        color: 'red',
                        highlight: '#FF7777',
                        label: 'Slow'
                    },
               {
                   value: 0,
                   color: 'orange',
                   highlight: '#ffaf60',
                   label: 'Average'
               },
               {
                   value: 0,
                   color: 'blue',
                   highlight: '#7777FF',
                   label: 'Fast'
               },
               {
                   value: 0,
                   color: 'green',
                   highlight: '#00b300',
                   label: 'Exceptional'
               }],
                    timeLineData: {
                        datasets: [
                            {
                                label: 'Timeline',
                                strokeColor: 'green',
                                data: []
                            }
                        ]
                    }

                },

                doughnutSettings: {
                    animation: true,
                    showTooltips: true,
                    percentageInnerCutout: 50,
                    segmentShowStroke: true,
                    segmentStrokeColor: '#222222'
                },
                lineDateSettings: {
                    responsive: true,
                    scaleOverride: true,
                    scaleBeginAtZero: true,
                    animation: true,
                    animationEasing: "linear",
                    bezierCurve: true,
                    showTooltips: true,
                    scaleShowHorizontalLines: true,
                    scaleShowLabels: true,
                    scaleType: 'date',
                    scaleDateTimeFormat: 'mmm d, yyyy',
                    scaleSteps: 5,
                    scaleStepWidth: 1,
                    pointDotRadius: 10,
                    pointHitDetectionRadius: 10,
                    useUtc: false,
                    scaleLabel: function (value) {
                        var label = "";
                        switch (parseInt(value.value)) {
                            case 1:
                                label = "Slow";
                                break;
                            case 2:
                                label = "Avg.";
                                break;
                            case 3:
                                label = "Fast";
                                break;
                            case 4:
                                label = "Exp.";
                                break;

                        }
                        return label;
                    }
                }
            },
        };

        var panelData =
        {
            AssignmentPanel: ko.observableArray([]),
            CodeReviewPanel: ko.observableArray([]),
            RandomReviewPanel: ko.observableArray([]),
            SkillsPanel: ko.observableArray([]),
            WeeklyFeedbackPanel: ko.observableArray([]),
            WeeklyTimeLineData: ko.observableArray([]),
            WeeklyTipData: ko.observableArray([]),
            CoursePanel: ko.observableArray([]),
            SessionPanel: ko.observableArray([])
        };

       
        var chartData = {
            AssignmentPanel: ko.observableArray([]),
            CodeReviewPanel: ko.observableArray([]),
            RandomReviewPanel: ko.observableArray([]),
            SkillsPanel: ko.observableArray([]),
            WeeklyFeedbackPanel: ko.observableArray([]),
            WeeklyTimeLine: {
                LineData: ko.observable(settings.chartSettings.timeLineData),
                DoughnutData: ko.observableArray([])
            },
            WeeklyTipData: ko.observableArray([]),
            CoursePanel: ko.observableArray([]),
            SessionPanel: ko.observableArray([])
        };

        var loadDataUsingPromise = function (serviceMethod, callback, failureCallback) {
            var deferredObject = $.Deferred();

            serviceMethod().done(function (data) {
                callback(data);
                my.toggleLoader();
                deferredObject.resolve(data);
            }).fail(function () {
                failureCallback();
            });
        };

        var loadAssignmentDataWithPromise = function () {
            settings.customLoader.AssignmentPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getAssignmentFeedbackWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
                assignmentReviewLoadCallback, assignmentReviewFailureCallback); // register call back here
        };

        var assignmentReviewLoadCallback = function (data) {
            panelData.AssignmentPanel(data);
            settings.customLoader.AssignmentPanel(false);
        };

        var assignmentReviewFailureCallback = function (data) {
            console.log("Failde to laod Assignments");
            settings.customLoader.AssignmentPanel(false);
        };

        var loadCodeReviewDataWithPromise = function () {
            settings.customLoader.CodeReviewPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getCodeReviewWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
               codeReviewLoadCallback, codeReviewFailureCallback); // register call back here
        };

        var codeReviewLoadCallback = function (data) {
            panelData.CodeReviewPanel(data);
            settings.customLoader.CodeReviewPanel(false);
        };

        var codeReviewFailureCallback = function (data) {
            console.log("Failed to Load Code Review Data");
            settings.customLoader.CodeReviewPanel(false);
        };

        var loadRandomReviewDataWithPromise = function () {
            settings.customLoader.RandomReviewPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getRandomReviewWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
                randomReviewLoadCallback, randomReviewFailureCallback); // register call back here
        };

        var randomReviewLoadCallback = function (data) {
            panelData.RandomReviewPanel(data);
            settings.customLoader.RandomReviewPanel(false);
        };

        var randomReviewFailureCallback = function (data) {
            console.log("Failed to Load Random Reviews");
            settings.customLoader.RandomReviewPanel(false);
        };

        var loadSkillsFeedbackDataWithPromise = function () {
            settings.customLoader.SkillsPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getSkillsWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
                skillReviewLoadCallback, skillReviewFailureCallback); // register call back here
        };

        var skillReviewLoadCallback = function (data) {
            panelData.SkillsPanel(data);
            settings.customLoader.SkillsPanel(false);
        };

        var skillReviewFailureCallback = function (data) {
            console.log("Failed to load skills Feedback");
            settings.customLoader.SkillsPanel(false);
        };

        var loadWeeklyFeedbackDataWithPromise = function () {
            settings.customLoader.WeeklyFeedbackPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getWeeklyFeedbacksWithFiltersPromise(settings.filterDetails.UserId(),
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
               weeklyReviewLoadCallback, weeklyReviewFailureCallback); // register call back here
        };

        var weeklyReviewLoadCallback = function (data) {
            panelData.WeeklyFeedbackPanel(data);
            settings.customLoader.WeeklyFeedbackPanel(false);
        };

        var weeklyReviewFailureCallback = function (data) {
            console.log("Failed to load Weekly Feedback");
            settings.customLoader.WeeklyFeedbackPanel(false);
        };

        var loadWeeklyLearningTimeLineDataWithPromise = function () {
            settings.customLoader.WeeklyLearningTimeLinePanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getWeeklyLearningTimeLineDataWithFiltersPromise(settings.filterDetails.UserId(), // mdoify
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
               weeklyLearningTimeLineLoadCallback, weeklyLearningTimeLineFailureCallback); // register call back here
        };

        var weeklyLearningTimeLineLoadCallback = function (data) {
            panelData.WeeklyTimeLineData(data);
            settings.customLoader.WeeklyLearningTimeLinePanel(false);
        };

        var weeklyLearningTimeLineFailureCallback = function () {
            console.log("Failed to load Weekly Time Line Data");
            settings.customLoader.WeeklyLearningTimeLinePanel(false);
        };

        var loadWeeklyTipDataWithPromise = function () {
            settings.customLoader.WeeklyTipPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getWeeklyWeeklyTipDataWithFiltersPromise(settings.filterDetails.UserId(), // mdoify
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
               weeklyTipDataLoadCallback, weeklyTipDataFailureCallback); // register call back here
        };

        var weeklyTipDataLoadCallback = function (data) {
            panelData.WeeklyTipData(data);
            getTimelineData(data, chartData.WeeklyTimeLine.LineData);
            settings.customLoader.WeeklyTipPanel(false);
        };

        var weeklyTipDataFailureCallback = function () {
            console.log("Failed to load Weekly Tip Data");
            settings.customLoader.WeeklyTipPanel(false);
        };

        var intitalizeMirrorReportPlugin = function (userId, startDate, endDate, assignment, codeReview, randomReview, skills, weekly, course, session) {
            destroyPlugin();
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
                settings.panelVisibility.WeeklyLearningTimeLinePanel(weekly);
                settings.panelVisibility.WeeklyTipPanel(weekly);

                if (weekly) {
                    loadWeeklyFeedbackDataWithPromise();
                    loadWeeklyLearningTimeLineDataWithPromise();
                    loadWeeklyTipDataWithPromise();
                }
            }

            if (typeof (course) != 'undefined') {
                settings.panelVisibility.CoursePanel(course);
            }

            if (typeof (session) != 'undefined') {
                settings.panelVisibility.SessionPanel(session);
            }
        };

        var getTimelineData = function (data,property) {

            var timeLineData = settings.chartSettings.dataFormat.timeLineData;

            ko.utils.arrayForEach(data, function (item) {
                timeLineData.datasets[0].data.push({
                    y: item.Rating,
                    x: new Date(moment(item.AddedOn).format('MM/DD/YYYY'))
                });
            });
            property(timeLineData);
        }

        var destroyPlugin = function () {
            panelData.AssignmentPanel([]);
            panelData.CodeReviewPanel([]);
            panelData.RandomReviewPanel([]);
            panelData.SkillsPanel([]);
            panelData.WeeklyFeedbackPanel([]);
            panelData.WeeklyTimeLineData([]);
            panelData.WeeklyTipData([]);
            panelData.CoursePanel([]);
            panelData.SessionPanel([]);

            settings.filterDetails.UserId(0);
            settings.filterDetails.StartDate(null);
            settings.filterDetails.EndDate(null);
        }


        return {
            intitalizeMirrorReportPlugin: intitalizeMirrorReportPlugin,
            settings: settings,
            getTimelineData: getTimelineData,
            chartData:chartData,
            //  panelData: panelData,
            destroyPlugin: destroyPlugin
        }
    }();

});