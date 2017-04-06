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
                                strokeColor:'black',
                                data: []
                            }
                        ]
                    }

                },

                doughnutSettings: {
                    animation: true,
                    percentageInnerCutout: 50,
                    segmentShowStroke: true,
                    segmentStrokeColor: '#222222'
                },
                lineDataSettings: {
                    responsive: true,
                    scaleOverride: true,
                    scaleBeginAtZero: true,
                    animation: true,
                    scaleGridLineColor: "#ccc",
                    animationEasing: "easeOutElastic",
                    bezierCurve: true,
                    bezierCurveTension: 0.2,
                    //showTooltips: true,
                    scaleShowHorizontalLines: true,
                    scaleShowLabels: true,
                    scaleType: 'date',
                    scaleDateTimeFormat: 'mmm d, yyyy',
                    scaleSteps: 5,
                    scaleStepWidth: 1,
                    pointDotRadius: 7,
                    pointHitDetectionRadius: 5,
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
            AssignmentPanel: {
                LineData: ko.observable(settings.chartSettings.timeLineData),
                DoughnutData: ko.observableArray([]),
                MentorDetails:ko.observableArray([])
            },
            CodeReviewPanel:{
                LineData: ko.observable(settings.chartSettings.timeLineData),
                DoughnutData: ko.observableArray([]),
                MentorDetails: ko.observableArray([])
            },
            
            WeeklyFeedbackPanel: {
                LineData: ko.observable(settings.chartSettings.timeLineData),
                DoughnutData: ko.observableArray(settings.chartSettings.countData),
                MentorDetails: ko.observableArray([])
            },
            WeeklyTimeLine: {
                LineData: ko.observable(settings.chartSettings.timeLineData),
                DoughnutData: ko.observableArray([]),
                MentorDetails: ko.observableArray([])
            }
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
            getDoughnutData(data, chartData.AssignmentPanel.DoughnutData);
            getTimelineData(data, chartData.AssignmentPanel.LineData);
            getUniqueMentors(data, chartData.AssignmentPanel.MentorDetails);
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
            getDoughnutData(data, chartData.CodeReviewPanel.DoughnutData);
            setTimeout(getTimelineData(data, chartData.CodeReviewPanel.LineData), 1000);
            getUniqueMentors(data, chartData.CodeReviewPanel.MentorDetails);
            settings.customLoader.CodeReviewPanel(false);
        };

        var codeReviewFailureCallback = function (data) {
            console.log("Failed to Load Code Review Data");
            settings.customLoader.CodeReviewPanel(false);
        };

        var loadRandomReviewDataWithPromise = function () {
            settings.customLoader.RandomReviewPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getRandomReviewAndThreadFeedbackWithFiltersPromise(settings.filterDetails.UserId(),
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
                return my.mirrorService.getAllSkillsForAttendeeWithFiltersPromise(settings.filterDetails.UserId(),
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
            getDoughnutData(data, chartData.WeeklyFeedbackPanel.DoughnutData);
            setTimeout(getTimelineData(data, chartData.WeeklyFeedbackPanel.LineData), 1000);
            getUniqueMentors(data, chartData.WeeklyFeedbackPanel.MentorDetails);
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
            getDoughnutData(data, chartData.WeeklyTimeLine.DoughnutData);
            setTimeout(getTimelineData(data, chartData.WeeklyTimeLine.LineData), 1000);
            getUniqueMentors(data, chartData.WeeklyTimeLine.MentorDetails);
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
            settings.customLoader.WeeklyTipPanel(false);
        };

        var weeklyTipDataFailureCallback = function () {
            console.log("Failed to load Weekly Tip Data");
            settings.customLoader.WeeklyTipPanel(false);
        };

        var loadSessionDataWithPromise = function () {
            settings.customLoader.SessionPanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getAllSessionForAttendeeWithFiltersPromise(settings.filterDetails.UserId(), // mdoify
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
               sessionDataLoadCallback, sessionDataFailureCallback); // register call back here
        };

        var sessionDataLoadCallback = function (data) {
            panelData.SessionPanel(data);
            settings.customLoader.SessionPanel(false);
        };

        var sessionDataFailureCallback = function () {
            console.log("Failed to load session Data");
            settings.customLoader.SessionPanel(false);
        };

        var loadCourseDataWithPromise = function () {
            settings.customLoader.CoursePanel(true);
            loadDataUsingPromise(function () {
                return my.mirrorService.getAllAssignedCourseForTraineeWithFiltersPromise(settings.filterDetails.UserId(), // mdoify
                    settings.filterDetails.StartDate(),
                    settings.filterDetails.EndDate());
            },
               courseDataLoadCallback, courseDataFailureCallback); // register call back here
        };

        var courseDataLoadCallback = function (data) {
            panelData.CoursePanel(data);
            settings.customLoader.CoursePanel(false);
        };

        var courseDataFailureCallback = function () {
            console.log("Failed to load scourse Data");
            settings.customLoader.CoursePanel(false);
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
                loadCourseDataWithPromise();

            }

            if (typeof (session) != 'undefined') {
                settings.panelVisibility.SessionPanel(session);
                loadSessionDataWithPromise();
            }
        };

        var getTimelineData = function (data,property) {

            var timeLineData = jQuery.extend(true, {},  settings.chartSettings.dataFormat.timeLineData);

            ko.utils.arrayForEach(data, function (item) {
                timeLineData.datasets[0].data.push({
                    y: item.Rating,
                    x: new Date(moment(item.AddedOn).format('MM/DD/YYYY'))
                });
            });
            property(timeLineData);
        }

        var getDoughnutData = function (data, property) {
            var doughnutData = jQuery.extend(true, {}, settings.chartSettings.dataFormat.countData);

            doughnutData[3].value = ko.utils.arrayFilter(data, function (item) {
                return item.Rating === 4;
            }).length;

            doughnutData[2].value = ko.utils.arrayFilter(data, function (item) {
                return item.Rating === 3;
            }).length;

            doughnutData[1].value = ko.utils.arrayFilter(data, function (item) {
                return item.Rating === 2;
            }).length;

            doughnutData[0].value = ko.utils.arrayFilter(data, function (item) {
                return item.Rating === 1;
            }).length;
            property(doughnutData);
        }

        var getUniqueMentors = function (data, property) {
            property([]);
          
            var uniqueMentors = data.filter(function (a) {
                if (!this[a.AddedBy.UserId]) {
                    this[a.AddedBy.UserId] = true;
                    return true;
                }
            }, Object.create(null));

            ko.utils.arrayForEach(uniqueMentors, function (item) {
                var mentorObject = {
                    UserId: item.AddedBy.UserId,
                    FullName: item.AddedBy.FullName,
                    ProfilePictureName: item.AddedBy.ProfilePictureName,
                    FeedbackCount : function() {
                     return   ko.utils.arrayFilter(data, function (mentor) {
                         return item.AddedBy.UserId == mentor.AddedBy.UserId;
                        }).length;
                    }()
                }
                property.push(mentorObject);
                property.sort(function(l, r) {
                    return l.FeedbackCount == r.FeedbackCount ? 0 : (l.FeedbackCount > r.FeedbackCount ? 1 : -1);
                });
            });
           
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

            getTimelineData([], chartData.AssignmentPanel.LineData);
            chartData.AssignmentPanel.DoughnutData([]);
            chartData.AssignmentPanel.MentorDetails([]);

            getTimelineData([], chartData.CodeReviewPanel.LineData);
            chartData.CodeReviewPanel.DoughnutData([]);
            chartData.CodeReviewPanel.MentorDetails([]);

            getTimelineData([], chartData.WeeklyFeedbackPanel.LineData);
            chartData.WeeklyFeedbackPanel.DoughnutData([]);
            chartData.WeeklyFeedbackPanel.MentorDetails([]);

            getTimelineData([], chartData.WeeklyTimeLine.LineData);
            chartData.WeeklyTimeLine.DoughnutData([]);
            chartData.WeeklyTimeLine.MentorDetails([]);


            settings.filterDetails.UserId(0);
            settings.filterDetails.StartDate(null);
            settings.filterDetails.EndDate(null);
        }


        return {
            intitalizeMirrorReportPlugin: intitalizeMirrorReportPlugin,
            settings: settings,
            getTimelineData: getTimelineData,
            chartData:chartData,
             panelData: panelData,
            destroyPlugin: destroyPlugin
        }
    }();

});