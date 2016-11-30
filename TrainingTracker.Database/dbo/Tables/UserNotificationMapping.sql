CREATE TABLE [dbo].[UserNotificationMapping] (
    [Id]             INT IDENTITY (1, 1) NOT NULL,
    [UserId]         INT NOT NULL,
    [NotificationId] INT NOT NULL,
    [Seen]           BIT DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC),
    FOREIGN KEY ([NotificationId]) REFERENCES [dbo].[Notification] ([NotificationId]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

