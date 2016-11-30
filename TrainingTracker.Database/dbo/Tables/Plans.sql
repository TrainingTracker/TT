CREATE TABLE [dbo].[Plans] (
    [PlanId]      INT           IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (200) NULL,
    [Sequqence]   INT           NULL,
    [AddedOn]     DATETIME      CONSTRAINT [DF_Plans_AddedOn] DEFAULT (getdate()) NULL,
    [AddedBy]     INT           NULL,
    [Description] VARCHAR (MAX) NULL,
    [ParentId]    INT           NULL,
    CONSTRAINT [PK_Plans] PRIMARY KEY CLUSTERED ([PlanId] ASC)
);

