$(document).ready(function() {

    my.userSettingVm = function() {
//        var requestedSettingName = my.queryParams["settingName"]
        var selectedSetting = ko.observable("");

        var markAsSelected = function(selectedSettingName) {
            openSettingPanel(selectedSettingName);
        }

        var openSettingPanel = function(settingName) {

            var requestedSettingName = settingName||my.queryParams["settingName"];

            switch (requestedSettingName) {
                case 'ManageUsers':
                {
                    selectedSetting("ManageUsers");
                    my.notificationSettingVm.closeNotificationSetting();
                    my.memberDetailsVm.closeGpsUserSetting();
                    my.addUserVm.openAllUsersProfile();
                    my.editCrSystemRatingConfigVm.isVisible(false);
                    break;
                }
                case 'Notification':
                {
                    selectedSetting("Notification");
                    my.addUserVm.closeDialogue();
                    my.memberDetailsVm.closeGpsUserSetting();
                    my.notificationSettingVm.openNotificationSetting();
                    my.editCrSystemRatingConfigVm.isVisible(false);
                    break;
                }
                case 'GPSMembers':
                {
                    selectedSetting("GPSMembers");
                    my.notificationSettingVm.closeNotificationSetting();
                    my.memberDetailsVm.getMembersUnderLead();
                    my.memberDetailsVm.getAllDesignation();
                    my.editCrSystemRatingConfigVm.isVisible(false);
                    break;
                }
                case 'EditCrSystemRatingConfig':
                {
                    selectedSetting("EditCrSystemRatingConfig");
                    my.notificationSettingVm.closeNotificationSetting();
                    my.memberDetailsVm.closeGpsUserSetting();
                    my.addUserVm.closeDialogue();
                    my.editCrSystemRatingConfigVm.getRatingConfig();
                    break;
                }
                case 'MyProfile':
                default:
                {
                    selectedSetting("MyProfile");
                    my.notificationSettingVm.closeNotificationSetting();
                    my.memberDetailsVm.closeGpsUserSetting();
                    my.addUserVm.openUserProfile();
                    my.editCrSystemRatingConfigVm.isVisible(false);
                }
            }
           
        }

        var notifyStyle = function() {
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
            openSettingPanel: openSettingPanel,
            selectedSetting: selectedSetting,
            markAsSelected: markAsSelected,
            initialise: function() {
                openSettingPanel();
                ko.applyBindings(my.userSettingVm);
                notifyStyle();
            }
        }
    }();
    my.userSettingVm.initialise();
});