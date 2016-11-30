CREATE TABLE [dbo].[Project] (
    [ProjectId]   INT           IDENTITY (1, 1) NOT NULL,
    [Title]       VARCHAR (200) NULL,
    [Description] VARCHAR (MAX) NULL,
    [CreatedBy]   INT           NULL,
    [ProjectType] VARCHAR (50)  NULL,
    [ClientName]  VARCHAR (200) NULL,
    [AddedOn]     DATETIME      CONSTRAINT [DF_Project_AddedOn] DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED ([ProjectId] ASC)
);

