CREATE TABLE [dbo].[Skills] (
    [SkillId]     INT           IDENTITY (1, 1) NOT NULL,
    [Name]        VARCHAR (100) NULL,
    [Description] VARCHAR (500) NULL,
    [AddedOn]     DATE          CONSTRAINT [DF_Skills_AddedOn] DEFAULT (getdate()) NULL,
    [AddedBy]     INT           NULL,
    CONSTRAINT [PK_Skills] PRIMARY KEY CLUSTERED ([SkillId] ASC),
    CONSTRAINT [fk_Skills_User] FOREIGN KEY ([AddedBy]) REFERENCES [dbo].[User] ([UserId])
);

