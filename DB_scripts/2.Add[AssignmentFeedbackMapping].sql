CREATE TABLE AssignmentFeedbackMapping(
	Id INT AUTO_INCREMENT NOT NULL,
	AssignmentId INT NOT NULL,
	FeedbackId INT NOT NULL,
    CONSTRAINT PK_AssignmentFeedbackMapping PRIMARY KEY (Id),
	CONSTRAINT FK_AssignmentFeedbackMapping_Assignment FOREIGN KEY (AssignmentId)
				REFERENCES Assignment(Id),
    CONSTRAINT FK_AssignmentFeedbackMapping_Feedback FOREIGN KEY (FeedbackId)
				REFERENCES Feedback(FeedbackId)
);
