CREATE TABLE [dbo].[UserSessionMapping] (
    [Id]        INT      IDENTITY (1, 1) NOT NULL,
    [UserId]    INT      NULL,
    [SessionId] INT      NULL,
    [AddedOn]   DATETIME CONSTRAINT [DF_UserSessionMapping_AddedOn] DEFAULT (getdate()) NULL,
    [AddedBy]   INT      NULL,
    CONSTRAINT [PK_UserSessionMapping] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [fk_UserSessionMapping_AddedBy] FOREIGN KEY ([AddedBy]) REFERENCES [dbo].[User] ([UserId]),
    CONSTRAINT [fk_UserSessionMapping_Session] FOREIGN KEY ([SessionId]) REFERENCES [dbo].[Session] ([SessionId]),
    CONSTRAINT [fk_UserSessionMapping_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([UserId])
);

