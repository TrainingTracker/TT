CREATE TABLE [dbo].[Questions] (
    [Id]           INT            IDENTITY (1, 1) NOT NULL,
    [QuestionText] VARCHAR (1000) NOT NULL,
    [Description]  VARCHAR (MAX)  NOT NULL,
    [SkillId]      INT            NOT NULL,
    [AddedBy]      INT            NOT NULL,
    [AddedDate]    DATETIME       NOT NULL,
    CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Questions_SkillId] FOREIGN KEY ([SkillId]) REFERENCES [dbo].[Skills] ([SkillId]),
    CONSTRAINT [FK_Questions_UserId] FOREIGN KEY ([AddedBy]) REFERENCES [dbo].[User] ([UserId])
);

