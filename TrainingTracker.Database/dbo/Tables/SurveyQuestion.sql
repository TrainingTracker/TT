CREATE TABLE [dbo].[SurveyQuestion] (
    [SurveyQuestionId]       INT             IDENTITY (1, 1) NOT NULL,
    [SurveySectionId]        INT             NOT NULL,
    [QuestionText]           NVARCHAR (1000) NOT NULL,
    [HelpText]               NVARCHAR (200)  NULL,
    [IsMandatory]            BIT             NOT NULL,
    [AdditionalNoteRequired] BIT             NOT NULL,
    [ResponseTypeId]         INT             NOT NULL,
    [IsDeleted]              BIT             NOT NULL,
    [SortOrder]              INT             NOT NULL,
    [DateCreated]            DATETIME        DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([SurveyQuestionId] ASC),
    CONSTRAINT [FK_SurveyQuestion_SurveyQuestionResponseType] FOREIGN KEY ([ResponseTypeId]) REFERENCES [dbo].[SurveyQuestionResponseType] ([TypeId]),
    CONSTRAINT [FK_SurveyQuestion_SurveySection] FOREIGN KEY ([SurveySectionId]) REFERENCES [dbo].[SurveySection] ([SurveySectionId])
);

