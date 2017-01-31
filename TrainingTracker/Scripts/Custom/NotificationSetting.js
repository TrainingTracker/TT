$(document).ready(function () {

    my.notificationSettingVm = function () {
        var traineeList = ko.observableArray([]);
        var isVisible = ko.observable(false);
        var subscribedTraineeList = [];

        var closeNotificationSetting = function () {
            isVisible(false);
        }
        var getSubscribedTraineeeCallback = function (jsonData) {
            ko.utils.arrayForEach(traineeList(), function (item) {
                ko.utils.arrayForEach(jsonData, function(i) {
                    if (i.SubscribedForUserId == item.SubscribedForUserId) {
                        item.IsDeleted(false);
                        item.Id = i.Id;
                    }
                });
            });
            isVisible(true);
        }
        var openNotificationSetting = function () {
            traineeList([]);
            ko.utils.arrayForEach(my.meta.allTrainee(), function (item) {
                traineeList.push({
                    Id: 0,
                    FirstName: item.FirstName,
                    FullName: item.FullName,
                    SubscribedByUserId: my.meta.currentUser.UserId,
                    SubscribedForUserId: item.UserId,
                    IsDeleted: ko.observable(true)
                });
            });
            my.userService.getSubscribedTraineee(getSubscribedTraineeeCallback);
        }
        var updateSubscribedTraineeeCallback = function(jsonData) {
            if (jsonData) {
                alert("changes saved");
            }
        }
        var saveNotificationSetting = function() {
            subscribedTraineeList = [];
            ko.utils.arrayForEach(traineeList(), function (item) {
                //if ((item.Id > 0 && !item.IsDeleted()) || (item.Id == 0 && !item.IsDeleted())) {
                //    subscribedTraineeList.push(item);
                //}
                if (!(item.Id == 0 && item.IsDeleted())) {
                    subscribedTraineeList.push(item);
                }
            });
            my.userService.updateSubscribedTraineee(subscribedTraineeList, updateSubscribedTraineeeCallback);
        }
        return {
            isVisible: isVisible,
            traineeList: traineeList,
            openNotificationSetting: openNotificationSetting,
            closeNotificationSetting: closeNotificationSetting,
            saveNotificationSetting : saveNotificationSetting
        }
    }();
    
});