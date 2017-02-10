CREATE TABLE `ForumUserHelpCategories` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
  `CreatedOn` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ;

CREATE TABLE `ForumUserHelpStatus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
  `CreatedOn` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ;


CREATE TABLE `ForumUserHelpPosts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
  `AddedBy` int(11) NOT NULL,
  `CategoryId` int(11) NOT NULL,
  `StatusId` int(11) NOT NULL,
  `CreatedOn` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `FK_ForumUserHelpPosts_User` (`AddedBy`),
  KEY `FK_ForumUserHelpPosts_Category` (`CategoryId`),
  KEY `FK_ForumUserHelpPosts_Status` (`StatusId`),
  CONSTRAINT `FK_ForumUserHelpPosts_Category` FOREIGN KEY (`CategoryId`) REFERENCES `ForumUserHelpCategories` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_ForumUserHelpPosts_Status` FOREIGN KEY (`StatusId`) REFERENCES `ForumUserHelpStatus` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_ForumUserHelpPosts_User` FOREIGN KEY (`AddedBy`) REFERENCES `User` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ;


CREATE TABLE `ForumUserHelpThreads` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PostId` int(11) NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
  `AddedBy` int(11) NOT NULL,
  `CreatedOn` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `FK_ForumUserHelpThreads_User` (`AddedBy`),
  KEY `FK_ForumUserHelpThreads_Post` (`PostId`),
  CONSTRAINT `FK_ForumUserHelpThreads_Post` FOREIGN KEY (`PostId`) REFERENCES `ForumUserHelpPosts` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_ForumUserHelpThreads_User` FOREIGN KEY (`AddedBy`) REFERENCES `User` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ;
