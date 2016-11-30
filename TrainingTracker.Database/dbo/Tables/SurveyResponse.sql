CREATE TABLE [dbo].[SurveyResponse] (
    [SurveyResponse]            INT            IDENTITY (1, 1) NOT NULL,
    [SurveyQuestionId]          INT            NOT NULL,
    [SurveyAnswerId]            INT            NULL,
    [AdditionalNote]            NVARCHAR (MAX) NULL,
    [SurveyCompletedMetaDataId] INT            NOT NULL,
    [DateCreated]               DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([SurveyResponse] ASC),
    CONSTRAINT [FK_SurveyResponse_SurveyAnswer] FOREIGN KEY ([SurveyAnswerId]) REFERENCES [dbo].[SurveyAnswer] ([SurveyAnswerId]),
    CONSTRAINT [FK_SurveyResponse_SurveyCompletedMetaData] FOREIGN KEY ([SurveyCompletedMetaDataId]) REFERENCES [dbo].[SurveyCompletedMetaData] ([SurveyCompletedMetaDataId]),
    CONSTRAINT [FK_SurveyResponse_SurveyQuestion] FOREIGN KEY ([SurveyQuestionId]) REFERENCES [dbo].[SurveyQuestion] ([SurveyQuestionId])
);

