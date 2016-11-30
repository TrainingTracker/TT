CREATE TABLE [dbo].[FeedbackType] (
    [FeedbackTypeId] INT           IDENTITY (1, 1) NOT NULL,
    [Description]    VARCHAR (MAX) NULL,
    [Sequence]       INT           NULL,
    CONSTRAINT [PK_FedbackType] PRIMARY KEY CLUSTERED ([FeedbackTypeId] ASC)
);

