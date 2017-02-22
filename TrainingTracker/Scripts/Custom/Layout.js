$(document).ready(function () {

    my.meta = function () {
        var currentUser = {},
            allTrainee = ko.observableArray([]),
            allMentor = ko.observableArray([]),
            allActiveUsers = ko.observableArray([]),
            currentNotificationId = my.queryParams["notificationId"],
            isAdministrator = ko.observable(false),
            isManager=ko.observable(false),
            isTrainee = ko.observable(false),
            userProfileUrl = ko.observable(""),
			notifications = ko.observableArray([]),
		    noOfNotification = ko.observable(0),
            newActionsToPerform = ko.observableArray([]),
		    noOfPendingActions = ko.observable(0),
            avatarUrl = function (item) {
                return my.rootUrl + "/Uploads/ProfilePicture/" + item.ProfilePictureName;
            },
            initializeNavbar  = function() {
                $(".text").html("Howdy " + my.meta.currentUser.FirstName + " !!");
                $("#avatar").attr("src", my.meta.avatarUrl(my.meta.currentUser));
            },
			getNotificationCallback = function (notificationList)
			{
			    my.meta.notifications([]);
			    my.meta.newActionsToPerform([]);
              ko.utils.arrayForEach(notificationList, function (item) 
              {
                  var appender = (item.Link.indexOf('?') > -1) ? '&' : '?';
                  item.Link = item.Link + appender + 'notificationId=' + item.NotificationId;
                  if (item.TypeOfNotification != 1)
                  {
                      item.Action = "Added by";
                      item.AddedBy = item.UserDetails.FirstName + ' ' + item.UserDetails.LastName;
                      item.ProfilePictureName = item.UserDetails.ProfilePictureName;
                  }
                  else
                  {
                      item.ProfilePictureName = "system_notification.jpg";
                      item.Action = "Added on";
                  }


                  if (item.TypeOfNotification == 14) {
                      my.meta.newActionsToPerform.push(item);
                  }
                  my.meta.notifications.push(item);
                  
                  
              });
              my.meta.noOfPendingActions(my.meta.newActionsToPerform().length);
              my.meta.noOfNotification(my.meta.notifications().length - my.meta.newActionsToPerform().length);
          },
            getNotification = function () {
                 
                 if (!my.isNullorEmpty(currentNotificationId))
                 {
                     var notificationInfo = {
                         NotificationId: currentNotificationId
                     };
                     my.userService.updateNotification(notificationInfo, getNotificationCallback);
                 }
                 else {
                     my.userService.getNotification(getNotificationCallback);
                 }                 
             },
            updateNotificationCallback = function (updateStatus) {
                if (!updateStatus) {
                    //alert("Update notification failure");//To be changed in future Now for test
                }
            },
            updateNotification = function (notificationId, type, link) {
                var notificationInfo = {
                    NotificationId: notificationId,
                    TypeOfNotification: type
                };           
                my.userService.updateNotification(notificationInfo, updateNotificationCallback);
                window.location.href = link;
            },
            markAllNotificationAsReadCallback = function (updateStatus)
            {
                if (updateStatus) {
                    my.meta.notifications([]);
                    my.meta.noOfNotification(0);
                }
            },
            markAllNotificationAsRead = function() {
                my.userService.markAllNotificationAsRead(markAllNotificationAsReadCallback);
            },

            getAllActiveUsersCallback = function (result) {
                allActiveUsers(result);
                var trainee = [], trainer = [];
                ko.utils.arrayForEach(result, function(obj) {
                    if (obj.IsTrainee)
                        trainee.push(obj);
                    else
                        trainer.push(obj);
                });
                allTrainee(trainee);
                allMentor(trainer);
                if (my.isNullorEmpty(window.sessionStorage.getItem("allActiveUsers"))) {
                    window.sessionStorage.setItem("allActiveUsers", JSON.stringify(result));
                }

            },
             getCurrentUserCallback = function (user) {

                 if (my.isNullorEmpty(window.sessionStorage.getItem("currentUser"))) {
                     window.sessionStorage.setItem("currentUser", JSON.stringify(user));
                 }

                 my.meta.currentUser = user;
                 my.meta.isManager(user.IsManager);
                 my.meta.isAdministrator(user.IsAdministrator);
                 my.meta.isTrainee(user.IsTrainee);
                 my.meta.userProfileUrl(my.rootUrl + '/Profile/UserProfile?userId=' + user.UserId);
               
             },

            getCurrentUserPromise = function () {
                var deferredObject = $.Deferred();

                var currentUserData = JSON.parse(window.sessionStorage.getItem("currentUser"));
                if (my.isNullorEmpty(currentUserData)) {
                    my.userService.getCurrentUserPromise().done(function (data) {
                        getCurrentUserCallback(data);
                        my.toggleLoader();
                        deferredObject.resolve(data);
                    }).fail(function(data) {
                        alert("ajax promise failed while fetching current user data");
                        console.log(JSON.stringify(data));
                    });
                
                } else {
                    getCurrentUserCallback(currentUserData);
                    deferredObject.resolve(currentUserData);
                }
           
                return deferredObject.promise();
            },

            getAllActiveUsersPromise = function () {
                var deferredObject = $.Deferred();

                var allActiveUserData = JSON.parse(window.sessionStorage.getItem("allActiveUsers"));
                if (my.isNullorEmpty(allActiveUserData)) {
                    my.userService.getAllActiveUsersPromise()
                        .done(function(data) {
                            getAllActiveUsersCallback(data);
                            my.toggleLoader();
                            deferredObject.resolve(data);
                        }).fail(function(data) {
                            alert("Ajax promise failed while fetching all users data");
                            console.log(JSON.stringify(data));
                    });

                } else {
                    getAllActiveUsersCallback(allActiveUserData);
                    deferredObject.resolve(allActiveUserData);
                }
            
                return deferredObject.promise();
            },

            loadedCurrentUserPromise = function() {
                var deferredObject = $.Deferred();

                if (!my.isNullorEmpty(my.meta.currentUser) && !my.isNullorEmpty(my.meta.currentUser.Id)) {
                    deferredObject.resolve();
                } else {
                    getCurrentUserPromise().done(function() {
                        deferredObject.resolve();
                    });
                }
                return deferredObject.promise();
            },

            loadedAllActiveUsersPromise = function() {
                var deferredObject = $.Deferred();

                if (!my.isNullorEmpty(my.meta.allActiveUsers) && my.meta.allActiveUsers().length > 0) {
                    deferredObject.resolve();
                } else {
                    getAllActiveUsersPromise().done(function() {
                        deferredObject.resolve();
                    });
                }

                return deferredObject.promise();
            },
             signOut = function() {
                sessionStorage.removeItem("currentUser");
                sessionStorage.removeItem("allActiveUsers");
                window.location.href = my.rootUrl + "/Login/SignOut";
            },

            loadLayoutWithMetaData = function() {
                getCurrentUserPromise().done(function() {
                    initializeNavbar();
                    my.meta.getNotification();
                    my.meta.getAllActiveUsersPromise();
                });
            }

        return {
            currentUser: currentUser,
            getAllActiveUsersPromise : getAllActiveUsersPromise,
            avatarUrl: avatarUrl,
            isAdministrator: isAdministrator,
            isManager: isManager,
            userProfileUrl: userProfileUrl,
            isTrainee: isTrainee,
            getNotification: getNotification,
            notifications: notifications,
            noOfNotification: noOfNotification,
            updateNotification: updateNotification,
            markAllNotificationAsRead: markAllNotificationAsRead,
            allTrainee:allTrainee,
            allMentor: allMentor,
            allActiveUsers : allActiveUsers,
            getCurrentUserPromise: getCurrentUserPromise,
            newActionsToPerform: newActionsToPerform,
            noOfPendingActions: noOfPendingActions,
            loadLayoutWithMetaData: loadLayoutWithMetaData,
            loadedAllActiveUsersPromise : loadedAllActiveUsersPromise,
            loadedCurrentUserPromise : loadedCurrentUserPromise,
            signOut: signOut
    };
    }();

    my.meta.loadLayoutWithMetaData();
});