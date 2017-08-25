CREATE TABLE `CrRatingCalcConfig` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `TeamId` int(11) NOT NULL,
  `CreatedOn` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `LastModifiedById` int(11) DEFAULT NULL,
  `LastModifiedOn` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_crcalcconf_team_idx` (`TeamId`),
  KEY `fk_crcalcconf_user_idx` (`LastModifiedById`),
  CONSTRAINT `fk_crcalcconf_team` FOREIGN KEY (`TeamId`) REFERENCES `team` (`TeamId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_crcalcconf_user` FOREIGN KEY (`LastModifiedById`) REFERENCES `User` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;



CREATE TABLE `CrRatingCalcRangeConfig` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `FeedbackTypeId` int(11) NOT NULL,
  `RangeMin` float NOT NULL,
  `RangeMax` float NOT NULL,
  `CrRatingCalcConfigId` int(11) NOT NULL,
  `CreatedOn` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `LastModifiedById` int(11) DEFAULT NULL,
  `LastModifiedOn` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `fk_crratingcalcrangeconfig_feedbacktype_idx` (`FeedbackTypeId`),
  KEY `uq_crratingcalcconfig_feedback` (`FeedbackTypeId`,`CrRatingCalcConfigId`),
  KEY `fk_crratingcalcrangeconfig_user_idx` (`LastModifiedById`),
  KEY `fk_crratingcalcrangeconfig_crratingcalcconfig_idx` (`CrRatingCalcConfigId`),
  CONSTRAINT `fk_crratingcalcrangeconfig_crratingcalcconfig` FOREIGN KEY (`CrRatingCalcConfigId`) REFERENCES `CrRatingCalcConfig` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_crratingcalcrangeconfig_feedbacktype` FOREIGN KEY (`FeedbackTypeId`) REFERENCES `feedbacktype` (`FeedbackTypeId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_crratingcalcrangeconfig_user` FOREIGN KEY (`LastModifiedById`) REFERENCES `User` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `CrRatingCalcWeightConfig` (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `ReviewPointTypeId` int(11) NOT NULL,
  `Weight` float NOT NULL,
  `CrRatingCalcConfigId` int(11) NOT NULL,
  `CreatedOn` timestamp NULL DEFAULT CURRENT_TIMESTAMP,
  `LastModifiedById` int(11) DEFAULT NULL,
  `LastModifiedOn` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `uq_crratingcalcconfig_pointtype` (`ReviewPointTypeId`,`CrRatingCalcConfigId`),
  KEY `fk_crratincalcconfig_reviewpointtype_idx` (`ReviewPointTypeId`),
  KEY `fk_crratingcalcweightconfig_user_idx` (`LastModifiedById`),
  KEY `fk_crratingcalcweightconfig_crratingcalcconfig_idx` (`CrRatingCalcConfigId`),
  CONSTRAINT `fk_crratingcalcweightconfig_crratingcalcconfig` FOREIGN KEY (`CrRatingCalcConfigId`) REFERENCES `CrRatingCalcConfig` (`Id`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_crratingcalcweightconfig_reviewpointtype` FOREIGN KEY (`ReviewPointTypeId`) REFERENCES `codereviewpointtype` (`CodeReviewPointTypeId`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  CONSTRAINT `fk_crratingcalcweightconfig_user` FOREIGN KEY (`LastModifiedById`) REFERENCES `User` (`UserId`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
