CREATE TABLE [dbo].[Release] (
    [ReleaseId]   INT            IDENTITY (1, 1) NOT NULL,
    [Major]       INT            NOT NULL,
    [Minor]       INT            NOT NULL,
    [Patch]       INT            NOT NULL,
    [Title]       NVARCHAR (200) CONSTRAINT [DF__Release__Title__3FD07829] DEFAULT ('NEW RELEASE') NOT NULL,
    [Description] NVARCHAR (MAX) NOT NULL,
    [IsPublished] BIT            CONSTRAINT [DF__Release__IsPubli__40C49C62] DEFAULT ((0)) NOT NULL,
    [ReleaseDate] DATETIME       CONSTRAINT [DF__ReleaseDe__Relea__40058253] DEFAULT (getdate()) NULL,
    [AddedBy]     INT            NOT NULL,
    CONSTRAINT [PK__ReleaseD__5D7A69CD22DCE8E3] PRIMARY KEY CLUSTERED ([ReleaseId] ASC),
    CONSTRAINT [FK_Release_UserId] FOREIGN KEY ([AddedBy]) REFERENCES [dbo].[User] ([UserId])
);

