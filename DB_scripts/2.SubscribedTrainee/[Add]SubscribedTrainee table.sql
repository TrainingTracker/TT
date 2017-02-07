
CREATE TABLE SubscribedTrainee(
	Id INT AUTO_INCREMENT NOT NULL,
	SubscribedByUserId INT NOT NULL,
	SubscribedForUserId INT NOT NULL,
    IsDeleted BIT NOT NULL,
	
    CONSTRAINT PK_SubscribedTrainee PRIMARY KEY (Id),
	CONSTRAINT FK_SubscribedTrainee_User_SubscribedBy FOREIGN KEY (SubscribedByUserId)
				REFERENCES User(UserId),
	CONSTRAINT FK_SubscribedTrainee_User_SubscribedFor FOREIGN KEY (SubscribedForUserId)
				REFERENCES User(UserId)
);