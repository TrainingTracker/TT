CREATE TABLE [dbo].[NotificationType] (
    [NotificationTypeId] INT            IDENTITY (1, 1) NOT NULL,
    [TypeDescription]    NVARCHAR (100) NOT NULL,
    PRIMARY KEY CLUSTERED ([NotificationTypeId] ASC)
);

