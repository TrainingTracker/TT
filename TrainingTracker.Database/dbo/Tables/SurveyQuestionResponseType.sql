CREATE TABLE [dbo].[SurveyQuestionResponseType] (
    [TypeId]      INT           IDENTITY (1, 1) NOT NULL,
    [Description] NVARCHAR (50) NULL,
    PRIMARY KEY CLUSTERED ([TypeId] ASC)
);

