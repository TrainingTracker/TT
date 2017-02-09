CREATE TABLE TaskSchedulerJob
(
	Id INT AUTO_INCREMENT NOT NULL,
    Description TINYTEXT  NOT NULL,
    ExecutionIntervalSeconds INT NOT NULL,
    LastExecution DATETIME,
    IsActive BIT NOT NULL ,
    RowCreatedTimeStamp DATETIME NOT NULL,
    
     CONSTRAINT PK_TaskSchedulerJobs PRIMARY KEY (Id)
);

CREATE TABLE EmailContent
(
	Id INT AUTO_INCREMENT NOT NULL,
    TaskSchedulerJobId INT NOT NULL, 
    SubjectText TEXT NOT NULL,
    BodyText LONGTEXT NOT NULL,
    IsRichBody BIT NOT NULL,
    FromAddress VARCHAR(300),
    IsSent BIT NOT NULL DEFAULT 0,
    SentTimeStamp DATETIME,
    Attempts TINYINT NOT NULL DEFAULT 0,
    
    CONSTRAINT PK_EmailContent PRIMARY KEY (Id),
    CONSTRAINT FK_EmailContent_SchedulerJob FOREIGN KEY (TaskSchedulerJobId)
				REFERENCES TaskSchedulerJob(Id)
);

CREATE TABLE EmailRecipientType
(
	Id INT AUTO_INCREMENT NOT NULL,
    Description Text NOT NULL,
    
       CONSTRAINT PK_EmailRecipientType PRIMARY KEY (Id)
);

CREATE TABLE EmailRecipient
(
	Id INT AUTO_INCREMENT NOT NULL,
    EmailContentId INT NOT NULL,
    EmailAddress VARCHAR(300) NOT NULL,
    EmailRecipientType INT NOT NULL,
    
    CONSTRAINT PK_EmailRecipient PRIMARY KEY (Id),
	CONSTRAINT FK_EmailRecipient_EmailRecipientType FOREIGN KEY (EmailRecipientType)
			   REFERENCES EmailRecipientType(Id),
    CONSTRAINT FK_EmailRecipient_EmailContent FOREIGN KEY (EmailContentId)
			   REFERENCES EmailContent(Id)
);

INSERT INTO EmailRecipientType(Description)
VALUES ('To');

INSERT INTO EmailRecipientType(Description)
VALUES ('Carbon Copy');

INSERT INTO EmailRecipientType(Description)
VALUES ('Blind Carbon Copy');

INSERT INTO TaskSchedulerJob(Description,ExecutionIntervalSeconds,IsActive,RowCreatedTimeStamp)
VALUES ("Default",0,0,now());

INSERT INTO TaskSchedulerJob(Description,ExecutionIntervalSeconds,IsActive,RowCreatedTimeStamp)
VALUES ("Very Frequent Email Task",120,1,now());
