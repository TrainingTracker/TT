CREATE TABLE [dbo].[UserSkillMapping] (
    [Id]      INT      IDENTITY (1, 1) NOT NULL,
    [SkillId] INT      NULL,
    [UserId]  INT      NULL,
    [AddedOn] DATETIME CONSTRAINT [DF_UserSkillMapping_AddedDate] DEFAULT (getdate()) NULL,
    [AddedBy] INT      NULL,
    CONSTRAINT [PK_UserSkillMapping] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [fk_UserSkillMapping_SkillId] FOREIGN KEY ([SkillId]) REFERENCES [dbo].[Skills] ([SkillId]),
    CONSTRAINT [fk_UserSkillMapping_USER] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [fk_UserSkillMapping_UserAddedBy] FOREIGN KEY ([AddedBy]) REFERENCES [dbo].[User] ([UserId])
);

