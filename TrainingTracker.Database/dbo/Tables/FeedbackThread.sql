CREATE TABLE [dbo].[FeedbackThread] (
    [ThreadId]         INT            IDENTITY (1, 1) NOT NULL,
    [Comments]         NVARCHAR (500) NOT NULL,
    [FeedbackId]       INT            NOT NULL,
    [AddedBy]          INT            NOT NULL,
    [DateTimeInserted] DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([ThreadId] ASC),
    CONSTRAINT [FK_Feedback_Threads] FOREIGN KEY ([FeedbackId]) REFERENCES [dbo].[Feedback] ([FeedbackId]),
    CONSTRAINT [FK_User_Threads] FOREIGN KEY ([AddedBy]) REFERENCES [dbo].[User] ([UserId])
);

