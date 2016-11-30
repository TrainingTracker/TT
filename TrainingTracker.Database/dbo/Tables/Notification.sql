CREATE TABLE [dbo].[Notification] (
    [NotificationId]    INT            IDENTITY (1, 1) NOT NULL,
    [NotificationTitle] NVARCHAR (200) NOT NULL,
    [Description]       NVARCHAR (500) NOT NULL,
    [Link]              NVARCHAR (200) NOT NULL,
    [NotificationType]  INT            NOT NULL,
    [AddedBy]           INT            NOT NULL,
    [AddedOn]           DATETIME       NOT NULL,
    PRIMARY KEY CLUSTERED ([NotificationId] ASC),
    CONSTRAINT [fk_Notification_NotificationType] FOREIGN KEY ([NotificationType]) REFERENCES [dbo].[NotificationType] ([NotificationTypeId]),
    CONSTRAINT [fk_Notification_User] FOREIGN KEY ([AddedBy]) REFERENCES [dbo].[User] ([UserId])
);

