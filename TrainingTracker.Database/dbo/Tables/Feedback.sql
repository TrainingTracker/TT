CREATE TABLE [dbo].[Feedback] (
    [FeedbackId]   INT           IDENTITY (1, 1) NOT NULL,
    [FeedbackText] VARCHAR (MAX) NULL,
    [Title]        VARCHAR (100) NULL,
    [FeedbackType] INT           NULL,
    [ProjectId]    INT           NULL,
    [SkillId]      INT           NULL,
    [Rating]       SMALLINT      NULL,
    [AddedBy]      INT           NULL,
    [AddedFor]     INT           NULL,
    [AddedOn]      DATETIME      CONSTRAINT [DF_Feedback_New_AddedOn] DEFAULT (getdate()) NULL,
    [StartDate]    DATE          NULL,
    [EndDate]      DATE          NULL,
    CONSTRAINT [PK_Feedback_New] PRIMARY KEY CLUSTERED ([FeedbackId] ASC),
    CONSTRAINT [fk_Feedback_AddedBy] FOREIGN KEY ([AddedBy]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [fk_Feedback_AddedFor] FOREIGN KEY ([AddedFor]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [fk_Feedback_FeedbackType] FOREIGN KEY ([FeedbackType]) REFERENCES [dbo].[FeedbackType] ([FeedbackTypeId])
);

