
$(document).ready(function() {
    my.chartVm = function () {
        var userFeedbackData = {};
        var chartData =
        {
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
                        label: 'Code Review',
                        strokeColor: 'green',
                        data: []
                    },
                    {
                    label: 'Weekly Feedback',
                    strokeColor: 'red',
                    data: []
                    },
                    {
                        label: 'Assignment',
                        strokeColor: 'Yellow',
                        data: []
                    }

                ]
                }

    };
        var settings = {
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
                scaleLabel: function (value)
                {
                    var label = "";
                    switch (parseInt(value.value))
                    {
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
            },
            feedbackTypeNavBar: ko.observableArray([]),
            activeTab: ko.observable(4)
    };
        var timelineData = ko.observable(chartData.timeLineData);
        var totalCountData = ko.observableArray(chartData.countData);
        var refreshChartDataAndRedraw = function() {
            // loadCountChart();
            if (my.chartVm.settings.feedbackTypeNavBar().indexOf('4') >= 0) my.chartVm.loadCountChart('CR');
            else if (my.chartVm.settings.feedbackTypeNavBar().indexOf('5') >= 0) my.chartVm.loadCountChart('WF');
            else my.chartVm.loadCountChart('Ass');
            refreshTimeLineChartAndRedraw();
        };

        var refreshTimeLineChartAndRedraw = function() {
            chartData.timeLineData.datasets[0].data = [];
            chartData.timeLineData.datasets[1].data = [];
            chartData.timeLineData.datasets[2].data = [];
            
            ko.utils.arrayForEach(userFeedbackData.CodeReviewFeedbacks, function(item) {
                chartData.timeLineData.datasets[0].data.push({
                    y: item.Rating,
                    x: new Date(moment(item.AddedOn).format('MM/DD/YYYY'))
                });
            });
                
            ko.utils.arrayForEach(userFeedbackData.WeeklyFeedbacks, function (item)
            {
                chartData.timeLineData.datasets[1].data.push({
                        y: item.Rating,
                        x: new Date(moment(item.AddedOn).format('MM/DD/YYYY'))
                    });
                });
            ko.utils.arrayForEach(userFeedbackData.AssignmentFeedbacks, function (item)
            {
                        
                chartData.timeLineData.datasets[2].data.push({
                            y: item.Rating,
                            x: new Date(moment(item.AddedOn).format('MM/DD/YYYY'))
                        });
                        //my.sessionVm.sessionDetails.Attendee.push(item.UserId.toString());
            });
            my.chartVm.timelineData(chartData.timeLineData);
        };

        var refreshCountChartDataAndRedraw = function (arr)
        {
            
          ////  chartData.countData.data = [5, 2, 4, 7];
            my.chartVm.chartData.countData[0].value = arr[0];
            my.chartVm.chartData.countData[1].value = arr[1];
            my.chartVm.chartData.countData[2].value = arr[2];
            my.chartVm.chartData.countData[3].value = arr[3];
            my.chartVm.totalCountData(my.chartVm.chartData.countData);
        };
        var loadCountChart = function (type) {
            if (type == '') my.chartVm.refreshCountChartDataAndRedraw([0, 0, 0, 0]);
            var data = [];
            var countArray = [0,0,0,0];
            switch(type) {
                case 'WF':
                    data = userFeedbackData.WeeklyFeedbacks;
                    my.chartVm.settings.activeTab(5);
                    break;
                case 'CR':
                    data = userFeedbackData.CodeReviewFeedbacks;
                    my.chartVm.settings.activeTab(4);
                    break;
                case 'Ass':
                    data = userFeedbackData.AssignmentFeedbacks;
                    my.chartVm.settings.activeTab(3);
                    break;
            }

            if (!(my.chartVm.settings.feedbackTypeNavBar().indexOf(my.chartVm.settings.activeTab().toString()) >= 0)) {
                refreshChartDataAndRedraw();
                return;
            }

            if (data.length > 0)
            {
                countArray = [];
                for (var i = 1; i <= 4; i++) {
                    
                    var filteredResult = ko.utils.arrayFilter(data, function(item) {
                        return item.Rating == i;
                    });
                    countArray.push(filteredResult.length);
                }
                my.chartVm.refreshCountChartDataAndRedraw(countArray);
                return;
            }
            my.chartVm.refreshCountChartDataAndRedraw(countArray);
        };
        var loadDataCallback = function (jsonData) {
            userFeedbackData = jsonData;
            // alert('data is here');
            // console.log(jsonData);
            refreshChartDataAndRedraw();

        };
        var loadUserPlotData = function (traineeId, startDate, endDate, arrayFeedbackType, trainer) {
            if (traineeId <= 0 || arrayFeedbackType.length == 0) return;
            if (typeof (trainer) == 'undefined') trainer = 0; // to fetch all data
            settings.feedbackTypeNavBar(arrayFeedbackType);
            my.userService.getUserFeedbackForPlot(traineeId, startDate, endDate, arrayFeedbackType, trainer, my.chartVm.loadDataCallback);
        };
        return {            
            timelineData: timelineData,
            chartData: chartData,
            totalCountData: totalCountData,
            refreshCountChartDataAndRedraw: refreshCountChartDataAndRedraw,
            loadUserPlotData:loadUserPlotData,
            settings: settings,
            loadDataCallback: loadDataCallback,
            loadCountChart: loadCountChart,
            refreshChartDataAndRedraw: refreshChartDataAndRedraw
        };
    }();
});