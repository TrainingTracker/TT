CREATE TABLE [dbo].[SurveyAnswer] (
    [SurveyAnswerId]   INT            IDENTITY (1, 1) NOT NULL,
    [SurveyQuestionId] INT            NOT NULL,
    [OptionText]       NVARCHAR (200) NOT NULL,
    [IsDeleted]        BIT            NOT NULL,
    [SortOrder]        INT            NOT NULL,
    [DateCreated]      DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([SurveyAnswerId] ASC),
    CONSTRAINT [FK_SurveyAnswer_SurveyQuestion] FOREIGN KEY ([SurveyQuestionId]) REFERENCES [dbo].[SurveyQuestion] ([SurveyQuestionId])
);

