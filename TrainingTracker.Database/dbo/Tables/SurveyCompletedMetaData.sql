CREATE TABLE [dbo].[SurveyCompletedMetaData] (
    [SurveyCompletedMetaDataId] INT      IDENTITY (1, 1) NOT NULL,
    [SurveyId]                  INT      NOT NULL,
    [DateCompleted]             DATETIME DEFAULT (getdate()) NOT NULL,
    [SurveyTakenBy]             INT      NOT NULL,
    PRIMARY KEY CLUSTERED ([SurveyCompletedMetaDataId] ASC),
    CONSTRAINT [FK_SurveyCompletedMetaData_Survey] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId]),
    CONSTRAINT [FK_SurveyCompletedMetaData_SurveyUser] FOREIGN KEY ([SurveyTakenBy]) REFERENCES [dbo].[User] ([UserId])
);

