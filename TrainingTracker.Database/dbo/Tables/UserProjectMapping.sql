CREATE TABLE [dbo].[UserProjectMapping] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [UserId]    INT      NULL,
    [ProjectId] INT      NULL,
    [StartedOn] DATETIME CONSTRAINT [DF_AddedBy_StartedOn] DEFAULT (getdate()) NULL,
    [EndedOn]   DATETIME NULL,
    [AddedBy]   INT      NULL,
    CONSTRAINT [PK_AddedBy] PRIMARY KEY CLUSTERED ([Id] ASC)
);

