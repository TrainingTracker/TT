USE [TrainingTracker]
GO
/****** Object:  Table [dbo].[Assignment]    Script Date: 12/9/2016 9:34:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Assignment](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[AddedBy] [int] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_Assignment] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AssignmentSubtopicMap]    Script Date: 12/9/2016 9:34:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssignmentSubtopicMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssignmentId] [int] NOT NULL,
	[SubtopicId] [int] NOT NULL,
 CONSTRAINT [PK_AssignmentSubtopicMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[AssignmentUserMap]    Script Date: 12/9/2016 9:34:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AssignmentUserMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AssignmentId] [int] NOT NULL,
	[TraineeId] [int] NOT NULL,
	[StartedOn] [datetime] NOT NULL,
	[CompletedOn] [datetime] NULL,
	[ApprovedBy] [int] NULL,
 CONSTRAINT [PK_AssignmentUserMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Course]    Script Date: 12/9/2016 9:34:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Course](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Icon] [nvarchar](200) NOT NULL,
	[AddedBy] [int] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getdate()),
	[IsPublished] [bit] NOT NULL DEFAULT ((0)),
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CourseSubtopic]    Script Date: 12/9/2016 9:34:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseSubtopic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[AddedBy] [int] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_CourseSubtopic] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[CourseSubtopicDiscussion]    Script Date: 12/9/2016 9:34:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CourseSubtopicDiscussion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseSubtopicId] [int] NOT NULL,
	[Text] [nvarchar](max) NOT NULL,
	[IsHidden] [bit] NOT NULL,
	[AddedBy] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_CourseSubtopicDiscussion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SubtopicContent]    Script Date: 12/9/2016 9:34:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubtopicContent](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[CourseSubtopicId] [int] NOT NULL,
	[Name] [nvarchar](250) NOT NULL,
	[Description] [nvarchar](max) NOT NULL,
	[Url] [nvarchar](max) NULL,
	[AddedBy] [int] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[CreatedOn] [datetime] NOT NULL DEFAULT (getdate()),
 CONSTRAINT [PK_SubtopicContent] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SubtopicContentUserMap]    Script Date: 12/9/2016 9:34:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SubtopicContentUserMap](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SubtopicContentId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[Seen] [bit] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
 CONSTRAINT [PK_SubtopicContentUserMap] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
ALTER TABLE [dbo].[AssignmentUserMap] ADD  DEFAULT (getdate()) FOR [StartedOn]
GO
ALTER TABLE [dbo].[CourseSubtopicDiscussion] ADD  DEFAULT ((0)) FOR [IsHidden]
GO
ALTER TABLE [dbo].[CourseSubtopicDiscussion] ADD  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[SubtopicContentUserMap] ADD  DEFAULT ((0)) FOR [Seen]
GO
ALTER TABLE [dbo].[Assignment]  WITH CHECK ADD  CONSTRAINT [FK_Assignment_User] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Assignment] CHECK CONSTRAINT [FK_Assignment_User]
GO
ALTER TABLE [dbo].[AssignmentSubtopicMap]  WITH CHECK ADD  CONSTRAINT [FK_AssignmentSubtopicMap_Assignment] FOREIGN KEY([AssignmentId])
REFERENCES [dbo].[Assignment] ([Id])
GO
ALTER TABLE [dbo].[AssignmentSubtopicMap] CHECK CONSTRAINT [FK_AssignmentSubtopicMap_Assignment]
GO
ALTER TABLE [dbo].[AssignmentSubtopicMap]  WITH CHECK ADD  CONSTRAINT [FK_AssignmentSubtopicMap_CourseSubtopic] FOREIGN KEY([SubtopicId])
REFERENCES [dbo].[CourseSubtopic] ([Id])
GO
ALTER TABLE [dbo].[AssignmentSubtopicMap] CHECK CONSTRAINT [FK_AssignmentSubtopicMap_CourseSubtopic]
GO
ALTER TABLE [dbo].[AssignmentUserMap]  WITH CHECK ADD  CONSTRAINT [FK_AssignmentUseraMap_User] FOREIGN KEY([ApprovedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[AssignmentUserMap] CHECK CONSTRAINT [FK_AssignmentUseraMap_User]
GO
ALTER TABLE [dbo].[AssignmentUserMap]  WITH CHECK ADD  CONSTRAINT [FK_AssignmentUserMap_User] FOREIGN KEY([AssignmentId])
REFERENCES [dbo].[Assignment] ([Id])
GO
ALTER TABLE [dbo].[AssignmentUserMap] CHECK CONSTRAINT [FK_AssignmentUserMap_User]
GO
ALTER TABLE [dbo].[AssignmentUserMap]  WITH CHECK ADD  CONSTRAINT [FK_AssignmentUserMap_UserTrainee] FOREIGN KEY([TraineeId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[AssignmentUserMap] CHECK CONSTRAINT [FK_AssignmentUserMap_UserTrainee]
GO
ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_User] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_User]
GO
ALTER TABLE [dbo].[CourseSubtopic]  WITH CHECK ADD  CONSTRAINT [FK_CourseSubtopic_Course] FOREIGN KEY([CourseId])
REFERENCES [dbo].[Course] ([Id])
GO
ALTER TABLE [dbo].[CourseSubtopic] CHECK CONSTRAINT [FK_CourseSubtopic_Course]
GO
ALTER TABLE [dbo].[CourseSubtopic]  WITH CHECK ADD  CONSTRAINT [FK_CourseSubtopic_User] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[CourseSubtopic] CHECK CONSTRAINT [FK_CourseSubtopic_User]
GO
ALTER TABLE [dbo].[CourseSubtopicDiscussion]  WITH CHECK ADD  CONSTRAINT [FK_CourseSubtopicDiscussion_CourseSubtopic] FOREIGN KEY([CourseSubtopicId])
REFERENCES [dbo].[CourseSubtopic] ([Id])
GO
ALTER TABLE [dbo].[CourseSubtopicDiscussion] CHECK CONSTRAINT [FK_CourseSubtopicDiscussion_CourseSubtopic]
GO
ALTER TABLE [dbo].[CourseSubtopicDiscussion]  WITH CHECK ADD  CONSTRAINT [FK_CourseSubtopicDiscussion_User] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[CourseSubtopicDiscussion] CHECK CONSTRAINT [FK_CourseSubtopicDiscussion_User]
GO
ALTER TABLE [dbo].[SubtopicContent]  WITH CHECK ADD  CONSTRAINT [FK_SubtopicContent_CourseSubtopic] FOREIGN KEY([CourseSubtopicId])
REFERENCES [dbo].[CourseSubtopic] ([Id])
GO
ALTER TABLE [dbo].[SubtopicContent] CHECK CONSTRAINT [FK_SubtopicContent_CourseSubtopic]
GO
ALTER TABLE [dbo].[SubtopicContent]  WITH CHECK ADD  CONSTRAINT [FK_SubtopicContent_User] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[SubtopicContent] CHECK CONSTRAINT [FK_SubtopicContent_User]
GO
ALTER TABLE [dbo].[SubtopicContentUserMap]  WITH CHECK ADD  CONSTRAINT [FK_SubtopicContentUserMap_SubtopicContent] FOREIGN KEY([SubtopicContentId])
REFERENCES [dbo].[SubtopicContent] ([Id])
GO
ALTER TABLE [dbo].[SubtopicContentUserMap] CHECK CONSTRAINT [FK_SubtopicContentUserMap_SubtopicContent]
GO
ALTER TABLE [dbo].[SubtopicContentUserMap]  WITH CHECK ADD  CONSTRAINT [FK_SubtopicContentUserMap_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[SubtopicContentUserMap] CHECK CONSTRAINT [FK_SubtopicContentUserMap_User]
GO
