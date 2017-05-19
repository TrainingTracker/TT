$(document).ready(function () {
    my.gpsUserSettingVm = function () {
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
            IsChecked: ko.observable(),
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

        unsyncedUsers = ko.observableArray([]),

        filteredUsers = ko.observableArray([]),

        gpsId = ko.observable(),

        saveMessage = ko.observable(),

        allDesignation = ko.observableArray([]),

        filteredTrainee = ko.observableArray([]),

        multipleImportStatus = ko.observable(),

        filterKeyword = ko.observable(""),

        selectedDesignation = ko.observable("All"),

        autoCompleteUserData = ko.observableArray([]),

        visibilityForSelectAllRows = ko.observable(false);

        getMembersUnderLeadCallback = function () {
            lstUsers([]),
            filteredUsers([]),
            ko.utils.arrayForEach(membersUnderLead(), function (item) {
                item.SelectedOption = ko.observable();
                if (item.MiddleName != null && item.MiddleName != "") {
                    item.FullName = item.FirstName + " " + item.MiddleName + " " + item.LastName;
                } else {
                    item.FullName = item.FirstName + " " + item.LastName;
                }
                var matchFoundForUsername = my.getMatchInArray(activeLstUsers(), item, "UserName");
                if (matchFoundForUsername.length <= 0) {
                    item.Status = ko.observable(false);
                    item.IsActive = ko.observable(false);
                } else {
                    item.Status = ko.observable(matchFoundForUsername[0].IsTrainee ? 'Trainee' : (matchFoundForUsername[0].IsTrainer ? 'Trainer' : 'Edit'));
                    item.IsActive = ko.observable(matchFoundForUsername[0].IsActive);
                    item.UserId = matchFoundForUsername[0].UserId;
                    item.SelectedOption = ko.observable(getSelectedValue(item));
                }
                item.IsReadOnly = ko.observable(false);
                item.IsChecked = ko.observable(false);
                var matchFoundForEmployeeId = my.getMatchInArray(activeLstUsers(), item, "EmployeeId");
                visibilityForSync = matchFoundForEmployeeId.length <= 0;
                item.GPSId = ko.observable(my.getSubstring(item.EmployeeId, 5, item.EmployeeId.Length));
                lstUsers.push(item);
                filteredUsers.push(item);
                visibilityForSyncCallback(visibilityForSync);
            });
            my.gpsUserSettingVm.lstUsers().length <= 0 ? my.gpsUserSettingVm.message("No members available!") : my.gpsUserSettingVm.message("");
        },

        getMembersUnderLead = function () {
            unsyncedUsers([]),
            my.gpsUserSettingVm.saveMessage('');
            my.userService.getMembersUnderLead(getAllUsers);
            isVisible(true);
        },

        closeDialogue = function () {

        }

        saveUserCallback = function (jsonData) {
            if (jsonData.status) {
                getMembersUnderLead();
                my.gpsUserSettingVm.saveMessage("User saved successfully!");
            }
            else {
                my.gpsUserSettingVm.saveMessage("User saving unsuccessful!");
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
            user.IsTrainee = isTrainee;
            user.IsTrainer = !isTrainee;
            user.IsActive = true;
            user.IsValid = true;
            user.TeamId = my.meta.currentUser.TeamId;
            my.userService.createUser(user, saveUserCallback);
        },

        editUser = function (user) {
            var selectedOption = user.SelectedOption();
            switch (selectedOption) {
                case 1:
                    user.IsTrainer = true;
                    break;
                case 2:
                    user.IsTrainee = true;
                    break;
            }
            user.IsValid = true;
            user.TeamId = my.meta.currentUser.TeamId;
            my.userService.updateUser(user, saveUserCallback);
        },

        getAllUsers = function (jsonData) {
            membersUnderLead([]),
            my.userService.getAllUsers(getAllUsersCallback);
            ko.utils.arrayForEach(jsonData, function (item) {
                membersUnderLead.push(item);
            });
        },

        syncGPSUsersCallback = function (jsonData) {
            if (jsonData != null) {
                getMembersUnderLead();
            }
            ko.utils.arrayForEach(jsonData, function (item) {
                unsyncedUsers.push(item);
            });
        },

        syncGPSUser = function () {
            my.userService.syncGPSUsers(syncGPSUsersCallback);
        },

        visibilityForSyncCallback = function (visibility) {
            visibilityForSync = visibility;
            my.gpsUserSettingVm.visibilityForSync(visibility);
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
        },

        messageVisibilityForImport = function () {
            ko.utils.arrayForEach(lstUsers(), function (item) {
                return (item.Status != false);
            });
        },

        importMultiple = function (isTrainee) {
            ko.utils.arrayForEach(lstUsers(), function (item) {
                if (item.IsChecked) {
                    importGPSUser(item, isTrainee);
                    item.IsChecked = ko.observable(false);
                }
            });
            visibilityForMultipleImport();
            return true;
        },

        visibilityForMultipleImport = function () {
            ko.utils.arrayForEach(lstUsers(), function (item) {
                if ((!item.Status()) && item.IsChecked()) {
                    my.gpsUserSettingVm.multipleImportStatus(true);
                    visibilityForSelectAllRows(true);
                } else {
                    my.gpsUserSettingVm.multipleImportStatus(false);
                    visibilityForSelectAllRows(false);
                }
            });

            return true;
        },

        getAllDesignation = function () {
            my.userService.getAllDesignation(getAllDesignationCallBack);
        }

        getAllDesignationCallBack = function (jsonData) {
            allDesignation([]);
            ko.utils.arrayForEach(jsonData, function (item) {
                allDesignation.push(item);
            });
            allDesignation.push({ DesignationName: "All" });
        },

        filterByDesignation = function (value) {
            filteredUsers([]);
            if (value != "All") {
                filteredTrainee = ko.utils.arrayFilter(lstUsers(), function (item) {
                    return value == item.Designation;
                });
                ko.utils.arrayForEach(filteredTrainee, function (filteredItem) {
                    ko.utils.arrayForEach(lstUsers(), function (item) {
                        filteredUsers.push(item);
                    });
                });
            } else {
                filteredTrainee = ko.utils.arrayFilter(lstUsers(), function (item) {
                    filteredUsers.push(item);
                });
            }
            selectedDesignation(value);
        },

        searchByName = function (filterKeyword) {
            searchByNameCallback(filterKeyword, "filteredUsers");
        },

        stringStartsWith = function (string, startsWith) {
            string = string || "";
            if (startsWith.length > string.length)
                return false;
            return string.substring(0, startsWith.length) === startsWith;
        };

        searchByNameCallback = function (filterKeyword, container) {
            autoCompleteUserData([]);
            eval(container)([]);
            if (!(typeof (my.gpsUserSettingVm.filterKeyword()) == 'undefined' || my.gpsUserSettingVm.filterKeyword() == '')) {
                if (container == "filteredUsers") {
                    filteredUsers([]);
                    filteredTrainee = ko.utils.arrayFilter(lstUsers(), function (item) {
                        return item.FullName.toUpperCase().includes(my.gpsUserSettingVm.filterKeyword().trim().toUpperCase());
                    });
                } else if (container == "autoCompleteUserData") {
                    filteredTrainee = ko.utils.arrayFilter(lstUsers(), function (item) {
                        return stringStartsWith(item.FullName.toUpperCase(), my.gpsUserSettingVm.filterKeyword().trim().toUpperCase());

                    });
                }
                ko.utils.arrayForEach(filteredTrainee, function (filteredItem) {
                    ko.utils.arrayForEach(lstUsers(), function (item) {
                        eval(container).push(item);
                    });
                });
            } else {
                eval(container)([]);
            }
        },

        getAutoCompleteUserData = function (filterKeyword) {
            autoCompleteUserData([]);
            searchByNameCallback(filterKeyword, "autoCompleteUserData");
        },

        selectAllRows = function () {
            ko.utils.arrayForEach(filteredUsers(), function (item) {
                if (!item.Status()) {
                    item.IsChecked(true);
                }
            });
        }

        return {
            getMembersUnderLead: getMembersUnderLead,
            importGPSUser: importGPSUser,
            lstUsers: lstUsers,
            membersUnderLead: membersUnderLead,
            syncGPSUser: syncGPSUser,
            message: message,
            visibilityForSync: visibilityForSync,
            editUser: editUser,
            makeEditable: makeEditable,
            trainingMembers: trainingMembers,
            closeGpsUserSetting: closeGpsUserSetting,
            isVisible: isVisible,
            gpsId: gpsId,
            unsyncedUsers: unsyncedUsers,
            messageVisibilityForImport: messageVisibilityForImport,
            saveMessage: saveMessage,
            multipleImportStatus: multipleImportStatus,
            visibilityForMultipleImport: visibilityForMultipleImport,
            importMultiple: importMultiple,
            getAllDesignation: getAllDesignation,
            allDesignation: allDesignation,
            filterByDesignation: filterByDesignation,
            filteredUsers: filteredUsers,
            searchByName: searchByName,
            filterKeyword: filterKeyword,
            getAutoCompleteUserData: getAutoCompleteUserData,
            autoCompleteUserData: autoCompleteUserData,
            selectedDesignation: selectedDesignation,
            selectAllRows: selectAllRows,
            visibilityForSelectAllRows: visibilityForSelectAllRows
        }
    }();
});

