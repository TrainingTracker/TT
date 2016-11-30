CREATE TABLE [dbo].[WeeklyFeedbackSurveyMapping] (
    [Id]                        INT IDENTITY (1, 1) NOT NULL,
    [FeedbackId]                INT NOT NULL,
    [SurveyCompletedMetaDataId] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_WeeklyFeedbackSurveyMapping_Feedback] FOREIGN KEY ([FeedbackId]) REFERENCES [dbo].[Feedback] ([FeedbackId]),
    CONSTRAINT [FK_WeeklyFeedbackSurveyMapping_SurveyCompletedMetaData] FOREIGN KEY ([SurveyCompletedMetaDataId]) REFERENCES [dbo].[SurveyCompletedMetaData] ([SurveyCompletedMetaDataId])
);

