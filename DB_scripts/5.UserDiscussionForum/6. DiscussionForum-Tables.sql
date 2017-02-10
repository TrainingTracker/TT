CREATE TABLE `ForumDiscussionCategories` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
  `CreatedOn` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ;

CREATE TABLE `ForumDiscussionStatus` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
  `CreatedOn` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`)
) ;


CREATE TABLE `ForumDiscussionPosts` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `Title` varchar(250) CHARACTER SET utf8mb4 NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
  `AddedBy` int(11) NOT NULL,
  `CategoryId` int(11) NOT NULL,
  `StatusId` int(11) NOT NULL,
  `CreatedOn` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `FK_ForumDiscussionPosts_User` (`AddedBy`),
  KEY `FK_ForumDiscussionPosts_Category` (`CategoryId`),
  KEY `FK_ForumDiscussionPosts_Status` (`StatusId`),
  CONSTRAINT `FK_ForumDiscussionPosts_Category` FOREIGN KEY (`CategoryId`) REFERENCES `ForumDiscussionCategories` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_ForumDiscussionPosts_Status` FOREIGN KEY (`StatusId`) REFERENCES `ForumDiscussionStatus` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_ForumDiscussionPosts_User` FOREIGN KEY (`AddedBy`) REFERENCES `User` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ;


CREATE TABLE `ForumDiscussionThreads` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `PostId` int(11) NOT NULL,
  `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
  `AddedBy` int(11) NOT NULL,
  `CreatedOn` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`Id`),
  KEY `FK_ForumDiscussionThreads_User` (`AddedBy`),
  KEY `FK_ForumDiscussionThreads_Post` (`PostId`),
  CONSTRAINT `FK_ForumDiscussionThreads_Post` FOREIGN KEY (`PostId`) REFERENCES `ForumDiscussionPosts` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `FK_ForumDiscussionThreads_User` FOREIGN KEY (`AddedBy`) REFERENCES `User` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ;