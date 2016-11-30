$(document).ready(function () {

    my.meta = function () {
        var currentUser = {},
            allTrainee = ko.observableArray([]),
            allMentor =ko.observableArray([]),
            currentNotificationId = my.queryParams["notificationId"],
            isAdministrator = ko.observable(false),
            isManager=ko.observable(false),
            isTrainee = ko.observable(false),
            userProfileUrl = ko.observable(""),
            getCurrentUserCallback = function (user) {
                my.meta.currentUser = user;
                my.meta.isManager(user.IsManager);
                my.meta.isAdministrator(user.IsAdministrator);
                my.meta.isTrainee(user.IsTrainee);
                my.meta.userProfileUrl(my.rootUrl + 'Profile/UserProfile?userId=' + user.UserId);
                my.meta.initializeNavbar();
                my.meta.getNotification();
                
                my.meta.fetchAllUser();
            },
			  notifications = ko.observableArray([]),
		      noOfNotification = ko.observable(),
            avatarUrl = function (item) {
                return my.rootUrl + "/Uploads/ProfilePicture/" + item.ProfilePictureName;
            },
            getCurrentUser = function () {
                my.userService.getCurrentUser(my.meta.getCurrentUserCallback);
            },
            initializeNavbar  = function() {
                $(".text").html("Howdy " + my.meta.currentUser.FirstName + " !!");
                $("#avatar").attr("src", my.meta.avatarUrl(my.meta.currentUser));
            },
			getNotificationCallback = function (notificationList)
			{
              my.meta.notifications([]);
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
                  my.meta.notifications.push(item);
              });
              my.meta.noOfNotification(notificationList.length);
          },
             getNotification = function () {
                 
                 if (!my.isNullorEmpty(currentNotificationId))
                 {
                     var notificationInfo = {
                         NotificationId: currentNotificationId
                     };
                     my.userService.updateNotification(notificationInfo, my.meta.getNotificationCallback);
                 }
                 else {
                     my.userService.getNotification(my.meta.getNotificationCallback);
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
            my.userService.updateNotification(notificationInfo, my.meta.updateNotificationCallback);
            window.location.href = link;
        },
        markAllNotificationAsReadCallback = function (updateStatus)
        {
            if (updateStatus) {
                my.meta.notifications([]);
            }
        },
        fetchAllUser = function() {
            my.userService.getActiveUsers(fetchAllUserCallback);
        },
        fetchAllUserCallback =function(result) {
            var trainee = [], trainer = [];
            ko.utils.arrayForEach(result, function(obj) {
                if (obj.IsTrainee)
                    trainee.push(obj);
                else
                    trainer.push(obj);
            });
            allTrainee(trainee);
            allMentor(trainer);
        },
        markAllNotificationAsRead = function() {
            my.userService.markAllNotificationAsRead(markAllNotificationAsReadCallback);
        };

        return {
            currentUser: currentUser,
            getCurrentUserCallback: getCurrentUserCallback,
            getCurrentUser: getCurrentUser,
            initializeNavbar: initializeNavbar,
            avatarUrl: avatarUrl,
            isAdministrator: isAdministrator,
            isManager: isManager,
            userProfileUrl: userProfileUrl,
            isTrainee: isTrainee,
            getNotification: getNotification,
            getNotificationCallback: getNotificationCallback,
            notifications: notifications,
            noOfNotification: noOfNotification,
            updateNotification: updateNotification,
            updateNotificationCallback: updateNotificationCallback,
            markAllNotificationAsRead: markAllNotificationAsRead,
            allTrainee:allTrainee,
            allMentor:allMentor,
            fetchAllUser:fetchAllUser
            
    };
    }();

    my.meta.getCurrentUser();
});