$(document).ready(function () {
    my.memberDetailsVm = function () {
        var user = {
            UserId: ko.observable(),
            FirstName: ko.observable(),
            LastName: ko.observable(),
            MiddleName: ko.observable(),
            Email: ko.observable(),
            Designation: ko.observable(),
            IsFemale: ko.observable("false"),
            EmployeeId: ko.observable(),
            FullName: ko.observable(),
            UserName: ko.observable(),
            IsAdministrator: ko.observable(),
            IsTrainer: ko.observable(),
            IsTrainee: ko.observable(),
            IsManager: ko.observable(),
            IsActive: ko.observable(),
            ProfilePictureName: ko.observable("Dummy.jpg"),
            Password: ko.observable(),
            PhotoUrl: function () {
                return my.rootUrl + "/Uploads/ProfilePicture/" + my.addUserVm.user.ProfilePictureName();
            },
            IsReadOnly: ko.observable(true),
            IsNewProfile: ko.observable(false),
            enableChangePassword: ko.observable(false),
            fileData: ko.observable(""),
            TeamId: ko.observable(),
        },
        isVisible = ko.observable(false);

        message = ko.observable(),

        visibilityForSync = ko.observable(),

        lstUsers = ko.observableArray([]),

        activeLstUsers = ko.observableArray([]),

        membersUnderLead = ko.observableArray([]),

        gpsId = ko.observable(),

        getMembersUnderLeadCallback = function () {
            lstUsers([]),
            ko.utils.arrayForEach(membersUnderLead(), function (item) {
                item.SelectedOption = ko.observable();
                item.FullName = item.FirstName + " " + item.MiddleName + " " + item.LastName;
                var matchFoundForUsername = my.getMatchInArray(activeLstUsers(), item, "UserName");
                if (matchFoundForUsername.length <= 0) {
                    item.Status = ko.observable(false);                    
                    item.IsReadOnly = ko.observable(false);
                    item.IsActive = ko.observable(false);
                } else {
                    item.Status = ko.observable(matchFoundForUsername[0].IsTrainee ? 'Trainee' : (matchFoundForUsername[0].IsTrainer ? 'Trainer' : 'Edit'));
                    item.IsActive = ko.observable(matchFoundForUsername[0].IsActive);
                    item.UserId = matchFoundForUsername[0].UserId;
                    item.IsReadOnly = ko.observable(false);
                    item.SelectedOption = ko.observable(getSelectedValue(item));
                }
                var matchFoundForEmployeeId = my.getMatchInArray(activeLstUsers(), item, "EmployeeId");
                visibilityForSync = matchFoundForEmployeeId.length <= 0 ? true : false;
                item.GPSId = ko.observable(my.getSubstring(item.EmployeeId, 5, item.EmployeeId.Length));
                lstUsers.push(item);
                visibilityForSyncCallback(visibilityForSync);
            });
        },

        getMembersUnderLead = function () {
            my.userService.getMembersUnderLead(getAllUsers);
            isVisible(true);
        },

        saveUserCallback = function (jsonData) {
            if (jsonData.status) {
                my.memberDetailsVm.message("User saved successfully!");
                getMembersUnderLead();
            }
            else {
                my.memberDetailsVm.message("User saving unsuccessful!");
            }
        },

        getAllUsersCallback = function (jsonData) {
            activeLstUsers([]),
            ko.utils.arrayForEach(jsonData.AllUser, function (item) {
                activeLstUsers.push(item);
            });
            getMembersUnderLeadCallback();
        },

        importGPSUser = function (user, isTrainee) {
            isTrainee ? user.IsTrainee = true : user.IsTrainer = true;
            user.IsActive = true;
            user.IsValid = true;
            user.TeamId = my.meta.currentUser.TeamId;
            my.userService.createUser(user, my.memberDetailsVm.saveUserCallback);
        },

        editUser = function (user) {
            var selectedOption = user.SelectedOption();
            if (selectedOption == 1) {
                user.IsTrainer = true;
            } else if (selectedOption == 2) {
                user.IsTrainee = true;
            }
            user.IsValid = true;
            user.TeamId = my.meta.currentUser.TeamId;
            my.userService.updateUser(user, my.memberDetailsVm.saveUserCallback);
        },

        getAllUsers = function (jsonData) {
            membersUnderLead([]),
            my.userService.getAllUsers(my.memberDetailsVm.getAllUsersCallback);
            ko.utils.arrayForEach(jsonData, function (item) {
                membersUnderLead.push(item);
            });
        },

        syncGPSUsersCallback = function (jsonData) {
            if (jsonData.status) {
                my.memberDetailsVm.message("User synced successfully!");
                getMembersUnderLead();
            }
            else {
               my.memberDetailsVm.message("User syncng unsuccessful!");
            }
        },

        syncGPSUser = function () {
            my.userService.syncGPSUsers(my.memberDetailsVm.syncGPSUsersCallback);
        },

        visibilityForSyncCallback = function (visibility) {
            visibilityForSync = visibility;
            my.memberDetailsVm.visibilityForSync(visibility);
        },

        makeEditable = function (user) {
            user.IsReadOnly(true);
        },

        trainingMembers = ko.observableArray([{ Value: 1, Description: "Trainer" },
                                                { Value: 2, Description: "Trainee" }
        ]),

        getSelectedValue = function (item) {
            if (item.Status() == "Trainer") {
                return 1;
            } else if (item.Status() == "Trainee") {
                return 2;
            }
            return 0;
        },

        closeGpsUserSetting = function () {
            isVisible(false);
        }

        return {
            getMembersUnderLead: getMembersUnderLead,
            lstUsers: lstUsers,
            importGPSUser: importGPSUser,
            getAllUsersCallback: getAllUsersCallback,
            saveUserCallback: saveUserCallback,
            activeLstUsers: activeLstUsers,
            lstUsers: lstUsers,
            getAllUsers: getAllUsers,
            membersUnderLead: membersUnderLead,
            syncGPSUser: syncGPSUser,
            syncGPSUsersCallback: syncGPSUsersCallback,
            message: message,
            visibilityForSync: visibilityForSync,
            editUser: editUser,
            makeEditable: makeEditable,
            trainingMembers: trainingMembers,
            closeGpsUserSetting: closeGpsUserSetting,
            isVisible: isVisible,
            gpsId: gpsId
        }
    }();
});
