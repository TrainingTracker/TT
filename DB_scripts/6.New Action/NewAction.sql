INSERT INTO Notification(NotificationTitle,Description,Link,NotificationType,AddedBy,AddedOn)
VALUES ("New Action Required","","/Setting/userSetting?settingName=Notification",14,102,now());

SET @InsertedId =  LAST_INSERT_ID();

INSERT INTO UserNotificationMapping(userid,NotificationId,seen) -- 538

SELECT userId,@InsertedId,0 FROM `User` Where (IsManager=1 OR IsTrainer=1) and IsActive=1