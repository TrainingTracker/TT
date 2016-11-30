CREATE TABLE [dbo].[Comments] (
    [CommentId] INT           IDENTITY (1, 1) NOT NULL,
    [Comment]   VARCHAR (MAX) NULL,
    [AddedOn]   DATETIME      CONSTRAINT [DF_Comments_AddedOn] DEFAULT (getdate()) NULL,
    [AddedBy]   INT           NULL,
    [AddedFor]  INT           NULL,
    CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED ([CommentId] ASC)
);

