CREATE TABLE [dbo].[Survey] (
    [SurveyId]    INT           IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (50) NOT NULL,
    [IsOpen]      BIT           NOT NULL,
    [DateCreated] DATETIME      DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([SurveyId] ASC)
);

