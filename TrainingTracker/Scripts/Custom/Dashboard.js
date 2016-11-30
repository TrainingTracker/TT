$(document).ready(function () {

    my.dashboardVm = function () {
        var users = ko.observableArray([]),
            photoUrl = function(item) {
                return my.rootUrl + "/Uploads/ProfilePicture/" + item.ProfilePictureName;
            },
            getDashboardVmCallback = function(userList) {
                ko.utils.arrayForEach(userList.Trainees, function(item) {
                    item.PhotoUrl = my.dashboardVm.photoUrl(item.User);
                    my.dashboardVm.users.push(item);
                });
 
                ko.applyBindings(my.dashboardVm);
            },
            getDashboardVm = function() {
                my.userService.getDashboardVm(my.dashboardVm.getDashboardVmCallback);
            },
            feedback = [],            
            getFeedback = function (size, feedbacktype, userId)
            {
               // var  allData = [],
              var  allData  = $.grep(users(), function(value) {
                    //console.log("I was here");
                    return value.User.UserId == userId;
                });
                
                switch(feedbacktype) {
                    case "Weekly":
                        my.dashboardVm.feedback = allData[0].WeeklyFeedback;
                        break;
                    case "Skills":
                        my.dashboardVm.feedback = allData[0].SkillsFeedback;
                        break;
                    case "All":
                        my.dashboardVm.feedback = allData[0].RemainingFeedbacks;
                        break;
                    }
                
                if (my.dashboardVm.feedback.length ) {
                    
                    var temp = [];
                    for (var i = 0; i < parseInt(size) && i < my.dashboardVm.feedback.length ; i++) {
                        if (i == 0 && parseInt(size) > 1) continue;
                        temp.push(my.dashboardVm.feedback[i]);
                       
                    }
                    my.dashboardVm.feedback = [];
                    
                    if (temp.length > 1 || parseInt(size) > 1)
                    {                       
                        my.dashboardVm.feedback = temp;
                    } else {
                       
                        my.dashboardVm.feedback = temp[0];
                    }
                }

                return (my.dashboardVm.feedback.length) || (  typeof(my.dashboardVm.feedback.Title) != 'undefined' && my.dashboardVm.feedback.Title != "" )? true : false;
            },
            eachUserPendingFeedbackWeek = ko.observable([]),
            loadWeekForFeedbackNotPresent = function (data) {
                eachUserPendingFeedbackWeek(data);
            };
        

        return {
            users: users,
            getDashboardVmCallback: getDashboardVmCallback,
            getDashboardVm: getDashboardVm,
            photoUrl: photoUrl,
            feedback: feedback,
            getFeedback: getFeedback,
            eachUserPendingFeedbackWeek: eachUserPendingFeedbackWeek,
            loadWeekForFeedbackNotPresent: loadWeekForFeedbackNotPresent
        };
    }();

    my.dashboardVm.getDashboardVm();
    //ko.applyBindings(my.dashboardVm);
    
});