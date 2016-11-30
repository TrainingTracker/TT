CREATE TABLE [dbo].[LearningSource] (
    [SourceId]    INT           IDENTITY (1, 1) NOT NULL,
    [SkillId]     INT           NULL,
    [Title]       VARCHAR (200) NULL,
    [Description] VARCHAR (MAX) NULL,
    [Url]         VARCHAR (500) NULL,
    [Sequence]    INT           NULL,
    [AddedOn]     DATETIME      CONSTRAINT [DF_LearningSource_AddedOn] DEFAULT (getdate()) NULL,
    [AddedBy]     INT           NULL,
    CONSTRAINT [PK_LearningSource] PRIMARY KEY CLUSTERED ([SourceId] ASC)
);

