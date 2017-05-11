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

        unsyncedUsers = ko.observableArray([]),

        lstUserMultipleImport = ko.observableArray([]),

        filteredUsers = ko.observableArray([]),

        gpsId = ko.observable(),

        saveMessage = ko.observable(),

        allDesignation = ko.observableArray([]),

        allDesignation([]),

        filteredTrainee = ko.observableArray([]),

        multipleImportStatus = ko.observable(),

        filterKeyword = ko.observable(""),

        selectedDesignation = ko.observable(""),

        autoCompleteUserData = ko.observableArray([]),

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
                    item.IsReadOnly = ko.observable(false);
                    item.IsActive = ko.observable(false);
                } else {
                    item.Status = ko.observable(matchFoundForUsername[0].IsTrainee ? 'Trainee' : (matchFoundForUsername[0].IsTrainer ? 'Trainer' : 'Edit'));
                    item.IsActive = ko.observable(matchFoundForUsername[0].IsActive);
                    item.UserId = matchFoundForUsername[0].UserId;
                    item.IsReadOnly = ko.observable(false);
                    item.SelectedOption = ko.observable(getSelectedValue(item));
                }
                item.IsChecked = ko.observable(false);
                var matchFoundForEmployeeId = my.getMatchInArray(activeLstUsers(), item, "EmployeeId");
                visibilityForSync = matchFoundForEmployeeId.length <= 0 ? true : false;
                item.GPSId = ko.observable(my.getSubstring(item.EmployeeId, 5, item.EmployeeId.Length));
                lstUsers.push(item);
                filteredUsers.push(item);
                visibilityForSyncCallback(visibilityForSync);
            });
            my.memberDetailsVm.lstUsers().length <= 0 ? my.memberDetailsVm.message("No members available!") : my.memberDetailsVm.message("");
        },

        getMembersUnderLead = function () {
            unsyncedUsers([]),
            my.memberDetailsVm.saveMessage('');
            my.userService.getMembersUnderLead(getAllUsers);
            isVisible(true);
        },

        closeDialogue = function () {

        }

        saveUserCallback = function (jsonData) {
            if (jsonData.status) {
                getMembersUnderLead();
                my.memberDetailsVm.saveMessage("User saved successfully!");
            }
            else {
                my.memberDetailsVm.saveMessage("User saving unsuccessful!");
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
            if (jsonData != null) {
                getMembersUnderLead();
            }
            ko.utils.arrayForEach(jsonData, function (item) {
                unsyncedUsers.push(item);
            });
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
        },

        messageVisibilityForImport = function () {
            ko.utils.arrayForEach(lstUsers(), function (item) {
                if (item.Status == false) {
                    return true;
                }
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
        },

        visibilityForMultipleImport = function () {
            ko.utils.arrayForEach(lstUsers(), function (item) {
                ((!item.Status) && item.IsChecked) ? my.memberDetailsVm.multipleImportStatus(false) : my.memberDetailsVm.multipleImportStatus(true);
            });
        },

        getAllDesignation = function () {
            my.userService.getAllDesignation(getAllDesignationCallBack);
        }

        getAllDesignationCallBack = function (jsonData) {
            allDesignation([]);
            ko.utils.arrayForEach(jsonData, function (item) {
                allDesignation.push(item);
            });
        },

        filterByDesignation = function (value) {
            filteredTrainee = ko.utils.arrayFilter(lstUsers(), function (item) {
                return value == item.Designation;
            });
            filteredUsers([]);
            ko.utils.arrayForEach(filteredTrainee, function (filteredItem) {
                ko.utils.arrayForEach(lstUsers(), function (item) {
                    filteredUsers.push(item);
                });
            });
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
            if (!(typeof (my.memberDetailsVm.filterKeyword()) == 'undefined' || my.memberDetailsVm.filterKeyword() == '')) {
                if (container == "filteredUsers") {
                    filteredUsers([]);
                    filteredTrainee = ko.utils.arrayFilter(lstUsers(), function (item) {
                        return item.FullName.toUpperCase().includes(my.memberDetailsVm.filterKeyword().trim().toUpperCase());
                    });
                } else if (container == "autoCompleteUserData") {
                    filteredTrainee = ko.utils.arrayFilter(lstUsers(), function (item) {
                        return stringStartsWith(item.FullName.toUpperCase(), my.memberDetailsVm.filterKeyword().trim().toUpperCase());

                    });
                }
                my.memberDetailsVm.filterKeyword = ko.observable("");
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
            gpsId: gpsId,
            unsyncedUsers: unsyncedUsers,
            messageVisibilityForImport: messageVisibilityForImport,
            saveMessage: saveMessage,
            lstUserMultipleImport: lstUserMultipleImport,
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
            stringStartsWith: stringStartsWith,
            selectedDesignation: selectedDesignation
        }
    }();
});
