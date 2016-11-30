CREATE TABLE [dbo].[Session] (
    [SessionId]     INT            IDENTITY (1, 1) NOT NULL,
    [Title]         VARCHAR (200)  NULL,
    [Description]   VARCHAR (MAX)  NULL,
    [Presenter]     INT            NULL,
    [AddedOn]       DATE           CONSTRAINT [DF_Session_AddedOn] DEFAULT (getdate()) NULL,
    [SessionDate]   DATE           NULL,
    [VideoFileName] NVARCHAR (200) NULL,
    [SlideName]     NVARCHAR (200) NULL,
    CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED ([SessionId] ASC),
    CONSTRAINT [fk_Session_User] FOREIGN KEY ([Presenter]) REFERENCES [dbo].[User] ([UserId])
);

