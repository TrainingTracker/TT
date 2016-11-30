CREATE TABLE [dbo].[User] (
    [UserId]             INT           IDENTITY (1, 1) NOT NULL,
    [FirstName]          VARCHAR (100) NULL,
    [LastName]           VARCHAR (100) NULL,
    [UserName]           VARCHAR (50)  NULL,
    [Password]           VARCHAR (50)  NULL,
    [Email]              VARCHAR (100) NULL,
    [Designation]        VARCHAR (100) NULL,
    [ProfilePictureName] VARCHAR (100) NULL,
    [IsFemale]           BIT           NULL,
    [IsAdministrator]    BIT           NULL,
    [IsTrainer]          BIT           NULL,
    [IsTrainee]          BIT           NULL,
    [IsManager]          BIT           NULL,
    [DateAddedToSystem]  DATETIME      DEFAULT (getdate()) NULL,
    [IsActive]           BIT           DEFAULT ((1)) NULL,
    [TeamId]             INT           NULL,
    CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED ([UserId] ASC),
    CONSTRAINT [Fk_User_Team] FOREIGN KEY ([TeamId]) REFERENCES [dbo].[Team] ([TeamId]),
    CONSTRAINT [UC_UserName] UNIQUE NONCLUSTERED ([UserName] ASC)
);

