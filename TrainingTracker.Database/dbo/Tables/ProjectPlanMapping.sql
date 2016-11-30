CREATE TABLE [dbo].[ProjectPlanMapping] (
    [Id]        INT NOT NULL,
    [PlanId]    INT NULL,
    [ProjectId] INT NULL,
    CONSTRAINT [PK_ProjectPlanMapping] PRIMARY KEY CLUSTERED ([Id] ASC)
);

