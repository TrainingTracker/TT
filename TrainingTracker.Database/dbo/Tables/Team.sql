CREATE TABLE [dbo].[Team] (
    [TeamId]         INT           IDENTITY (1, 1) NOT NULL,
    [TeamName]       NVARCHAR (50) NOT NULL,
    [TeamManager]    INT           NOT NULL,
    [IsDeleted]      BIT           NOT NULL,
    [DateInserted]   DATETIME      NOT NULL,
    [WeeklySurveyId] INT           NOT NULL,
    PRIMARY KEY CLUSTERED ([TeamId] ASC),
    CONSTRAINT [FK_Team_Survey] FOREIGN KEY ([WeeklySurveyId]) REFERENCES [dbo].[Survey] ([SurveyId]),
    CONSTRAINT [Fk_UserId_TeamManager] FOREIGN KEY ([TeamManager]) REFERENCES [dbo].[User] ([UserId])
);

