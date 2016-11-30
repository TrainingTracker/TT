CREATE TABLE [dbo].[QuestionLevelMapping] (
    [Id]                   INT      IDENTITY (1, 1) NOT NULL,
    [QuestionId]           INT      NOT NULL,
    [Level]                SMALLINT NOT NULL,
    [ExperienceStartRange] SMALLINT NOT NULL,
    [ExperienceEndRange]   SMALLINT NOT NULL,
    CONSTRAINT [PK_QuestionLevelMapping] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_QuestionLevelMapping_QuestionId] FOREIGN KEY ([QuestionId]) REFERENCES [dbo].[Questions] ([Id])
);

