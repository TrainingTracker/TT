$(document).ready(function () {

    my.userSettingVm = function () {
//        var requestedSettingName = my.queryParams["settingName"]
        var selectedSetting = ko.observable("");

        var markAsSelected = function(selectedSettingName) {
            selectedSetting(selectedSettingName);
        }

        var openSettingPanel = function () {
            ko.options.deferUpdates = true;
            var requestedSettingName = my.queryParams["settingName"];
            if (requestedSettingName == undefined || requestedSettingName == "MyProfile") {
                selectedSetting("MyProfile");
                my.notificationSettingVm.closeNotificationSetting();
                my.addUserVm.openUserProfile();
            }
            else if (requestedSettingName == "ManageUsers") {
                selectedSetting("ManageUsers");
                my.notificationSettingVm.closeNotificationSetting();
                my.addUserVm.openAllUsersProfile();
            }
            else if (requestedSettingName == "Notificaton") {
                selectedSetting("Notificaton");
                my.addUserVm.closeDialogue();
                my.notificationSettingVm.openNotificationSetting();
            }
            else {
                selectedSetting("MyProfile");
                my.notificationSettingVm.closeNotificationSetting();
                my.addUserVm.openUserProfile();
            }
            ko.applyBindings(my.userSettingVm);
        }
        return {
            openSettingPanel : openSettingPanel,
            selectedSetting: selectedSetting,
            markAsSelected: markAsSelected
        }
    }();
    my.userSettingVm.openSettingPanel();
});