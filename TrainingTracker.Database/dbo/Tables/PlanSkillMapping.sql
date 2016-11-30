CREATE TABLE [dbo].[PlanSkillMapping] (
    [Id]      INT IDENTITY (1, 1) NOT NULL,
    [PlanId]  INT NULL,
    [SkillId] INT NULL,
    CONSTRAINT [PK_PlanSkillMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

