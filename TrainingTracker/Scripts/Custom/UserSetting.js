$(document).ready(function () {

    my.userSettingVm = function () {
//        var requestedSettingName = my.queryParams["settingName"]
        var selectedSetting = ko.observable("");

        var markAsSelected = function(selectedSettingName) {
            selectedSetting(selectedSettingName);
        }

        var openSettingPanel = function () {
            
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
            else if (requestedSettingName == "Notification") {
                selectedSetting("Notification");
                my.addUserVm.closeDialogue();
                my.notificationSettingVm.openNotificationSetting();
            }
            else {
                selectedSetting("MyProfile");
                my.notificationSettingVm.closeNotificationSetting();
                my.addUserVm.openUserProfile();
            }
            ko.applyBindings(my.userSettingVm);
            notifyStyle();
        }

        var notifyStyle = function () {
            $.notify.addStyle('customAlert', {
                html: "<div data-notify-text /div>",
                classes: {
                    base: {
                        "white-space": "nowrap",
                        "color": "white",
                        "font-size": "18px",
                        "background-color": "#194a71",
                        "padding": "5px 15px",
                        "position": "fixed",
                        "top": "50px",
                        "right": "200px",
                        "text-align": "center",
                        "min-width": "20%"

                    },
                    blue: {
                        "background-color": "#14588f"
                    },
                    red: {
                        "background-color": "#DC4749"
                    }

                }
            });
        }
        return {
            openSettingPanel : openSettingPanel,
            selectedSetting: selectedSetting,
            markAsSelected: markAsSelected
        }
    }();
    my.userSettingVm.openSettingPanel();
});