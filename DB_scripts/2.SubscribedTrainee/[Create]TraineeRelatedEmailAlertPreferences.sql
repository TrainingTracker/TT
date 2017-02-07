
DROP Table SubscribedTrainee;

CREATE TABLE `TraineeReletedEmailAlertPreferences` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `SubscribedByUserId` int(11) NOT NULL,
  `SubscribedForUserId` int(11) NOT NULL,
  `IsSubscribedForComment` bit NOT NULL,
  `IsSubscribedForWeeklyFeedback` bit(1) NOT NULL,
  `IsSubscribedForCodeReview` bit(1) NOT NULL,
  `IsSubscibedForAssignment` bit(1) NOT NULL,
  `IsSubscibedForSkill` bit(1) NOT NULL,
  `IsDeleted` bit(1) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_TraineeReletedEmailAlertPreferences_User_SubscribedBy` (`SubscribedByUserId`),
  KEY `FK_TraineeReletedEmailAlertPreferences_User_SubscribedFor` (`SubscribedForUserId`),
  CONSTRAINT `FK_TraineeReletedEmailAlertPreferences_User_SubscribedBy` FOREIGN KEY (`SubscribedByUserId`) REFERENCES `User` (`UserId`),
  CONSTRAINT `FK_TraineeReletedEmailAlertPreferences_User_SubscribedFor` FOREIGN KEY (`SubscribedForUserId`) REFERENCES `User` (`UserId`)
);
