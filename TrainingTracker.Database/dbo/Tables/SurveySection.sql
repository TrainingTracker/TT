CREATE TABLE [dbo].[SurveySection] (
    [SurveySectionId] INT           IDENTITY (1, 1) NOT NULL,
    [SectionHeader]   NVARCHAR (50) NOT NULL,
    [SortOrder]       INT           NOT NULL,
    [SurveyId]        INT           NOT NULL,
    [DateCreated]     DATETIME      DEFAULT (getdate()) NOT NULL,
    [IsDeleted]       BIT           NOT NULL,
    PRIMARY KEY CLUSTERED ([SurveySectionId] ASC),
    CONSTRAINT [FK_SurveySection_Survey] FOREIGN KEY ([SurveyId]) REFERENCES [dbo].[Survey] ([SurveyId])
);

