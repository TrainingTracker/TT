
CREATE TABLE CourseUserMapping(
	Id INT AUTO_INCREMENT NOT NULL,
	CourseId INT NOT NULL,
	UserId INT NOT NULL,
	StartedOn DATETIME NOT NULL,
	CompletedOn DATETIME NULL
    CONSTRAINT PK_CourseUserMapping PRIMARY KEY (Id),
	CONSTRAINT FK_CourseUserMapping_Course FOREIGN KEY (CourseId)
				REFERENCES Course(Id),
    CONSTRAINT FK_CourseUserMapping_User FOREIGN KEY (UserId)
				REFERENCES User(UserId)
);

ALTER TABLE AssignmentUserMap
 DROP COLUMN StartedOn;
 ALTER TABLE AssignmentUserMap
 DROP COLUMN CompletedOn;
 
 ALTER TABLE AssignmentUserMap
	ADD COLUMN IsCompleted BIT NOT NULL DEFAULT 0;
ALTER TABLE AssignmentUserMap
	ADD COLUMN IsApproved BIT NOT NULL DEFAULT 0;    
 