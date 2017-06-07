
CREATE TABLE CodeReviewMetaData (
 `CodeReviewMetaDataId` INT NOT NULL AUTO_INCREMENT, 
 `ProjectName` TINYTEXT NOT NULL,
 `Description` LONGTEXT NOT NULL,
 `FeedbackId` INT NULL, 
 `AddedBy` INT NOT NULL,
 `AddedFor` INT NOT NULL, 
 `IsDiscarded` BIT,
 `CreatedOn` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
 
 PRIMARY KEY (`CodeReviewMetaDataId`),
 
 CONSTRAINT `FK_CodeReviewMetaData_Feedback` FOREIGN KEY (`FeedbackId`) 
											 REFERENCES `Feedback` (`FeedbackId`),
                                             
 CONSTRAINT `FK_CodeReviewMetaData_AddedBy` FOREIGN KEY (`AddedBy`)
											REFERENCES `User` (`UserId`),
                                            
 CONSTRAINT `FK_CodeReviewMetaData_AddedFor` FOREIGN KEY (`AddedFor`) 
											 REFERENCES `User` (`UserId`)
);

CREATE TABLE CodeReviewTag (
`CodeReviewTagId` INT NOT NULL AUTO_INCREMENT, 
`CodeReviewMetaDataId` INT NOT NULL,
`IsDeleted` BIT NOT NULL,
`SkillId` INT NULL, 
`CreatedOn` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

PRIMARY KEY (`CodeReviewTagId`),

CONSTRAINT `FK_CodeReviewTag_CodeReviewMetaData` FOREIGN KEY (`CodeReviewMetaDataId`)
												 REFERENCES `CodeReviewMetaData` (`CodeReviewMetaDataId`),
                                                 
CONSTRAINT `FK_CodeReviewTag_Skills` FOREIGN KEY (`SkillId`) 
									 REFERENCES `Skills` (`SkillId`)
);

CREATE TABLE CodeReviewPointType(
`CodeReviewPointTypeId` INT NOT NULL AUTO_INCREMENT,
`Title` TINYTEXT NOT NULL,
`Description` TEXT NULL,
`Sequence` INT ,
`IsDeleted` BIT,
`CreatedOn` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,

PRIMARY KEY (`CodeReviewPointTypeId`)

);

CREATE TABLE CodeReviewPoints (
`CodeReviewPointId` INT NOT NULL AUTO_INCREMENT,
`CodeReviewTagId` INT NOT NULL,
`PointTitle` LONGTEXT NOT NULL,
`Description` LONGTEXT,
`CodeReviewPointType` INT NOT NULL,
`CreatedOn` TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP,
`ModifiedOn` TIMESTAMP,
`IsDeleted` BIT NOT NULL ,

PRIMARY KEY (`CodeReviewPointId`),

CONSTRAINT `FK_CodeReviewPoints_CodeReviewTag` FOREIGN KEY (`CodeReviewTagId`)
												 REFERENCES `CodeReviewTag` (`CodeReviewTagId`),
                                                 
CONSTRAINT `FK_CodeReviewPoints_CodeReviewPointType` FOREIGN KEY (`CodeReviewPointType`)
												 REFERENCES `CodeReviewPointType` (`CodeReviewPointTypeId`)

);


-- INSERT DATA INTO CodeReviewPointType

INSERT INTO CodeReviewPointType (`Title`,`Description`,`Sequence`,`IsDeleted`)
VALUES ("Exceptional", "Point better than expected",1,0),
       ("Good","Point matching the expectations",2,0),
       ("Corrected","Point corrected from previous review",3,0),
       ("Bad","Poor",4,0),
	   ("Critical","Need immediate attentions",5,0),
       ("Suggestion","Suggestions",6,0);
       

SELECT * FROM CodeReviewPointType