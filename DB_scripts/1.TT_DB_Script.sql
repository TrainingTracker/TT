USE [master]
GO
/****** Object:  Database [TrainingTracker]    Script Date: 12/9/2016 8:47:44 PM ******/
CREATE DATABASE [TrainingTracker]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'TrainingTracker', FILENAME = N'E:\Program Files\Microsoft SQL Server\MSSQL12.SQL2014\MSSQL\DATA\TrainingTracker.mdf' , SIZE = 4096KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'TrainingTracker_log', FILENAME = N'E:\Program Files\Microsoft SQL Server\MSSQL12.SQL2014\MSSQL\DATA\TrainingTracker_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [TrainingTracker] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [TrainingTracker].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [TrainingTracker] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [TrainingTracker] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [TrainingTracker] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [TrainingTracker] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [TrainingTracker] SET ARITHABORT OFF 
GO
ALTER DATABASE [TrainingTracker] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [TrainingTracker] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [TrainingTracker] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [TrainingTracker] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [TrainingTracker] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [TrainingTracker] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [TrainingTracker] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [TrainingTracker] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [TrainingTracker] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [TrainingTracker] SET  DISABLE_BROKER 
GO
ALTER DATABASE [TrainingTracker] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [TrainingTracker] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [TrainingTracker] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [TrainingTracker] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [TrainingTracker] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [TrainingTracker] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [TrainingTracker] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [TrainingTracker] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [TrainingTracker] SET  MULTI_USER 
GO
ALTER DATABASE [TrainingTracker] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [TrainingTracker] SET DB_CHAINING OFF 
GO
ALTER DATABASE [TrainingTracker] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [TrainingTracker] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [TrainingTracker] SET DELAYED_DURABILITY = DISABLED 
GO
USE [TrainingTracker]
GO
/****** Object:  UserDefinedFunction [dbo].[ufn_CSVToTable]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[ufn_CSVToTable] ( @StringInput NVARCHAR(4000), @Delimiter nvarchar(1))
RETURNS @OutputTable TABLE ( [Id] INT Identity(1,1), [Value] NVARCHAR(10) )
AS
BEGIN

    DECLARE @String    VARCHAR(10)

    WHILE LEN(@StringInput) > 0
    BEGIN
        SET @String      = LEFT(@StringInput, 
                                ISNULL(NULLIF(CHARINDEX(@Delimiter, @StringInput) - 1, -1),
                                LEN(@StringInput)))
        SET @StringInput = SUBSTRING(@StringInput,
                                     ISNULL(NULLIF(CHARINDEX(@Delimiter, @StringInput), 0),
                                     LEN(@StringInput)) + 1, LEN(@StringInput))

        INSERT INTO @OutputTable ( [Value] )
        VALUES ( @String )
    END

    RETURN
END

GO
/****** Object:  Table [dbo].[Assignment]    Script Date: 12/9/2016 8:47:48 PM ******/
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
/****** Object:  Table [dbo].[AssignmentSubtopicMap]    Script Date: 12/9/2016 8:47:48 PM ******/
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
/****** Object:  Table [dbo].[AssignmentUserMap]    Script Date: 12/9/2016 8:47:48 PM ******/
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
/****** Object:  Table [dbo].[Comments]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Comments](
	[CommentId] [int] IDENTITY(1,1) NOT NULL,
	[Comment] [varchar](max) NULL,
	[AddedOn] [datetime] NULL CONSTRAINT [DF_Comments_AddedOn]  DEFAULT (getdate()),
	[AddedBy] [int] NULL,
	[AddedFor] [int] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[CommentId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Course]    Script Date: 12/9/2016 8:47:48 PM ******/
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
/****** Object:  Table [dbo].[CourseSubtopic]    Script Date: 12/9/2016 8:47:48 PM ******/
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
/****** Object:  Table [dbo].[CourseSubtopicDiscussion]    Script Date: 12/9/2016 8:47:48 PM ******/
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
/****** Object:  Table [dbo].[Feedback]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Feedback](
	[FeedbackId] [int] IDENTITY(1,1) NOT NULL,
	[FeedbackText] [varchar](max) NULL,
	[Title] [varchar](100) NULL,
	[FeedbackType] [int] NULL,
	[ProjectId] [int] NULL,
	[SkillId] [int] NULL,
	[Rating] [smallint] NULL,
	[AddedBy] [int] NULL,
	[AddedFor] [int] NULL,
	[AddedOn] [datetime] NULL CONSTRAINT [DF_Feedback_New_AddedOn]  DEFAULT (getdate()),
	[StartDate] [date] NULL,
	[EndDate] [date] NULL,
 CONSTRAINT [PK_Feedback_New] PRIMARY KEY CLUSTERED 
(
	[FeedbackId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FeedbackThread]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FeedbackThread](
	[ThreadId] [int] IDENTITY(1,1) NOT NULL,
	[Comments] [nvarchar](500) NOT NULL,
	[FeedbackId] [int] NOT NULL,
	[AddedBy] [int] NOT NULL,
	[DateTimeInserted] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ThreadId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[FeedbackType]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FeedbackType](
	[FeedbackTypeId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [varchar](max) NULL,
	[Sequence] [int] NULL,
 CONSTRAINT [PK_FedbackType] PRIMARY KEY CLUSTERED 
(
	[FeedbackTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[LearningSource]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LearningSource](
	[SourceId] [int] IDENTITY(1,1) NOT NULL,
	[SkillId] [int] NULL,
	[Title] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[Url] [varchar](500) NULL,
	[Sequence] [int] NULL,
	[AddedOn] [datetime] NULL CONSTRAINT [DF_LearningSource_AddedOn]  DEFAULT (getdate()),
	[AddedBy] [int] NULL,
 CONSTRAINT [PK_LearningSource] PRIMARY KEY CLUSTERED 
(
	[SourceId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Notification]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Notification](
	[NotificationId] [int] IDENTITY(1,1) NOT NULL,
	[NotificationTitle] [nvarchar](200) NOT NULL,
	[Description] [nvarchar](500) NOT NULL,
	[Link] [nvarchar](200) NOT NULL,
	[NotificationType] [int] NOT NULL,
	[AddedBy] [int] NOT NULL,
	[AddedOn] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[NotificationType]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[NotificationType](
	[NotificationTypeId] [int] IDENTITY(1,1) NOT NULL,
	[TypeDescription] [nvarchar](100) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[NotificationTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Plans]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Plans](
	[PlanId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](200) NULL,
	[Sequqence] [int] NULL,
	[AddedOn] [datetime] NULL CONSTRAINT [DF_Plans_AddedOn]  DEFAULT (getdate()),
	[AddedBy] [int] NULL,
	[Description] [varchar](max) NULL,
	[ParentId] [int] NULL,
 CONSTRAINT [PK_Plans] PRIMARY KEY CLUSTERED 
(
	[PlanId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[PlanSkillMapping]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PlanSkillMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[PlanId] [int] NULL,
	[SkillId] [int] NULL,
 CONSTRAINT [PK_PlanSkillMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Project]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[CreatedBy] [int] NULL,
	[ProjectType] [varchar](50) NULL,
	[ClientName] [varchar](200) NULL,
	[AddedOn] [datetime] NULL CONSTRAINT [DF_Project_AddedOn]  DEFAULT (getdate()),
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ProjectPlanMapping]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectPlanMapping](
	[Id] [int] NOT NULL,
	[PlanId] [int] NULL,
	[ProjectId] [int] NULL,
 CONSTRAINT [PK_ProjectPlanMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[QuestionLevelMapping]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[QuestionLevelMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionId] [int] NOT NULL,
	[Level] [smallint] NOT NULL,
	[ExperienceStartRange] [smallint] NOT NULL,
	[ExperienceEndRange] [smallint] NOT NULL,
 CONSTRAINT [PK_QuestionLevelMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Questions]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Questions](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QuestionText] [varchar](1000) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[SkillId] [int] NOT NULL,
	[AddedBy] [int] NOT NULL,
	[AddedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Questions] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Release]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Release](
	[ReleaseId] [int] IDENTITY(1,1) NOT NULL,
	[Major] [int] NOT NULL,
	[Minor] [int] NOT NULL,
	[Patch] [int] NOT NULL,
	[Title] [nvarchar](200) NOT NULL CONSTRAINT [DF__Release__Title__3FD07829]  DEFAULT ('NEW RELEASE'),
	[Description] [nvarchar](max) NOT NULL,
	[IsPublished] [bit] NOT NULL CONSTRAINT [DF__Release__IsPubli__40C49C62]  DEFAULT ((0)),
	[ReleaseDate] [datetime] NULL CONSTRAINT [DF__ReleaseDe__Relea__40058253]  DEFAULT (getdate()),
	[AddedBy] [int] NOT NULL,
 CONSTRAINT [PK__ReleaseD__5D7A69CD22DCE8E3] PRIMARY KEY CLUSTERED 
(
	[ReleaseId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Session]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Session](
	[SessionId] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](200) NULL,
	[Description] [varchar](max) NULL,
	[Presenter] [int] NULL,
	[AddedOn] [date] NULL CONSTRAINT [DF_Session_AddedOn]  DEFAULT (getdate()),
	[SessionDate] [date] NULL,
	[VideoFileName] [nvarchar](200) NULL,
	[SlideName] [nvarchar](200) NULL,
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[SessionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Skills]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Skills](
	[SkillId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NULL,
	[Description] [varchar](500) NULL,
	[AddedOn] [date] NULL CONSTRAINT [DF_Skills_AddedOn]  DEFAULT (getdate()),
	[AddedBy] [int] NULL,
 CONSTRAINT [PK_Skills] PRIMARY KEY CLUSTERED 
(
	[SkillId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[SubtopicContent]    Script Date: 12/9/2016 8:47:48 PM ******/
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
/****** Object:  Table [dbo].[SubtopicContentUserMap]    Script Date: 12/9/2016 8:47:48 PM ******/
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
/****** Object:  Table [dbo].[Survey]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Survey](
	[SurveyId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[IsOpen] [bit] NOT NULL,
	[DateCreated] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[SurveyId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveyAnswer]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyAnswer](
	[SurveyAnswerId] [int] IDENTITY(1,1) NOT NULL,
	[SurveyQuestionId] [int] NOT NULL,
	[OptionText] [nvarchar](200) NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[SurveyAnswerId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveyCompletedMetaData]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyCompletedMetaData](
	[SurveyCompletedMetaDataId] [int] IDENTITY(1,1) NOT NULL,
	[SurveyId] [int] NOT NULL,
	[DateCompleted] [datetime] NOT NULL DEFAULT (getdate()),
	[SurveyTakenBy] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SurveyCompletedMetaDataId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveyQuestion]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyQuestion](
	[SurveyQuestionId] [int] IDENTITY(1,1) NOT NULL,
	[SurveySectionId] [int] NOT NULL,
	[QuestionText] [nvarchar](1000) NOT NULL,
	[HelpText] [nvarchar](200) NULL,
	[IsMandatory] [bit] NOT NULL,
	[AdditionalNoteRequired] [bit] NOT NULL,
	[ResponseTypeId] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[SortOrder] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[SurveyQuestionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveyQuestionResponseType]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyQuestionResponseType](
	[TypeId] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[TypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveyResponse]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveyResponse](
	[SurveyResponse] [int] IDENTITY(1,1) NOT NULL,
	[SurveyQuestionId] [int] NOT NULL,
	[SurveyAnswerId] [int] NULL,
	[AdditionalNote] [nvarchar](max) NULL,
	[SurveyCompletedMetaDataId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL DEFAULT (getdate()),
PRIMARY KEY CLUSTERED 
(
	[SurveyResponse] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[SurveySection]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SurveySection](
	[SurveySectionId] [int] IDENTITY(1,1) NOT NULL,
	[SectionHeader] [nvarchar](50) NOT NULL,
	[SortOrder] [int] NOT NULL,
	[SurveyId] [int] NOT NULL,
	[DateCreated] [datetime] NOT NULL DEFAULT (getdate()),
	[IsDeleted] [bit] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[SurveySectionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Team]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Team](
	[TeamId] [int] IDENTITY(1,1) NOT NULL,
	[TeamName] [nvarchar](50) NOT NULL,
	[TeamManager] [int] NOT NULL,
	[IsDeleted] [bit] NOT NULL,
	[DateInserted] [datetime] NOT NULL,
	[WeeklySurveyId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[TeamId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[User]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[FirstName] [varchar](100) NULL,
	[LastName] [varchar](100) NULL,
	[UserName] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Email] [varchar](100) NULL,
	[Designation] [varchar](100) NULL,
	[ProfilePictureName] [varchar](100) NULL,
	[IsFemale] [bit] NULL,
	[IsAdministrator] [bit] NULL,
	[IsTrainer] [bit] NULL,
	[IsTrainee] [bit] NULL,
	[IsManager] [bit] NULL,
	[DateAddedToSystem] [datetime] NULL DEFAULT (getdate()),
	[IsActive] [bit] NULL DEFAULT ((1)),
	[TeamId] [int] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[UserNotificationMapping]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserNotificationMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[NotificationId] [int] NOT NULL,
	[Seen] [bit] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserProjectMapping]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserProjectMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[ProjectId] [int] NULL,
	[StartedOn] [datetime] NULL CONSTRAINT [DF_AddedBy_StartedOn]  DEFAULT (getdate()),
	[EndedOn] [datetime] NULL,
	[AddedBy] [int] NULL,
 CONSTRAINT [PK_AddedBy] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserSessionMapping]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSessionMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NULL,
	[SessionId] [int] NULL,
	[AddedOn] [datetime] NULL CONSTRAINT [DF_UserSessionMapping_AddedOn]  DEFAULT (getdate()),
	[AddedBy] [int] NULL,
 CONSTRAINT [PK_UserSessionMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserSkillMapping]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserSkillMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[SkillId] [int] NULL,
	[UserId] [int] NULL,
	[AddedOn] [datetime] NULL CONSTRAINT [DF_UserSkillMapping_AddedDate]  DEFAULT (getdate()),
	[AddedBy] [int] NULL,
 CONSTRAINT [PK_UserSkillMapping] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[WeeklyFeedbackSurveyMapping]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WeeklyFeedbackSurveyMapping](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[FeedbackId] [int] NOT NULL,
	[SurveyCompletedMetaDataId] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
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
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [fk_Feedback_AddedBy] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [fk_Feedback_AddedBy]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [fk_Feedback_AddedFor] FOREIGN KEY([AddedFor])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [fk_Feedback_AddedFor]
GO
ALTER TABLE [dbo].[Feedback]  WITH CHECK ADD  CONSTRAINT [fk_Feedback_FeedbackType] FOREIGN KEY([FeedbackType])
REFERENCES [dbo].[FeedbackType] ([FeedbackTypeId])
GO
ALTER TABLE [dbo].[Feedback] CHECK CONSTRAINT [fk_Feedback_FeedbackType]
GO
ALTER TABLE [dbo].[FeedbackThread]  WITH CHECK ADD  CONSTRAINT [FK_Feedback_Threads] FOREIGN KEY([FeedbackId])
REFERENCES [dbo].[Feedback] ([FeedbackId])
GO
ALTER TABLE [dbo].[FeedbackThread] CHECK CONSTRAINT [FK_Feedback_Threads]
GO
ALTER TABLE [dbo].[FeedbackThread]  WITH CHECK ADD  CONSTRAINT [FK_User_Threads] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[FeedbackThread] CHECK CONSTRAINT [FK_User_Threads]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [fk_Notification_NotificationType] FOREIGN KEY([NotificationType])
REFERENCES [dbo].[NotificationType] ([NotificationTypeId])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [fk_Notification_NotificationType]
GO
ALTER TABLE [dbo].[Notification]  WITH CHECK ADD  CONSTRAINT [fk_Notification_User] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Notification] CHECK CONSTRAINT [fk_Notification_User]
GO
ALTER TABLE [dbo].[QuestionLevelMapping]  WITH CHECK ADD  CONSTRAINT [FK_QuestionLevelMapping_QuestionId] FOREIGN KEY([QuestionId])
REFERENCES [dbo].[Questions] ([Id])
GO
ALTER TABLE [dbo].[QuestionLevelMapping] CHECK CONSTRAINT [FK_QuestionLevelMapping_QuestionId]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_SkillId] FOREIGN KEY([SkillId])
REFERENCES [dbo].[Skills] ([SkillId])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_SkillId]
GO
ALTER TABLE [dbo].[Questions]  WITH CHECK ADD  CONSTRAINT [FK_Questions_UserId] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Questions] CHECK CONSTRAINT [FK_Questions_UserId]
GO
ALTER TABLE [dbo].[Release]  WITH CHECK ADD  CONSTRAINT [FK_Release_UserId] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Release] CHECK CONSTRAINT [FK_Release_UserId]
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [fk_Session_User] FOREIGN KEY([Presenter])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [fk_Session_User]
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
ALTER TABLE [dbo].[SurveyAnswer]  WITH CHECK ADD  CONSTRAINT [FK_SurveyAnswer_SurveyQuestion] FOREIGN KEY([SurveyQuestionId])
REFERENCES [dbo].[SurveyQuestion] ([SurveyQuestionId])
GO
ALTER TABLE [dbo].[SurveyAnswer] CHECK CONSTRAINT [FK_SurveyAnswer_SurveyQuestion]
GO
ALTER TABLE [dbo].[SurveyCompletedMetaData]  WITH CHECK ADD  CONSTRAINT [FK_SurveyCompletedMetaData_Survey] FOREIGN KEY([SurveyId])
REFERENCES [dbo].[Survey] ([SurveyId])
GO
ALTER TABLE [dbo].[SurveyCompletedMetaData] CHECK CONSTRAINT [FK_SurveyCompletedMetaData_Survey]
GO
ALTER TABLE [dbo].[SurveyCompletedMetaData]  WITH CHECK ADD  CONSTRAINT [FK_SurveyCompletedMetaData_User] FOREIGN KEY([SurveyTakenBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[SurveyCompletedMetaData] CHECK CONSTRAINT [FK_SurveyCompletedMetaData_User]
GO
ALTER TABLE [dbo].[SurveyQuestion]  WITH CHECK ADD  CONSTRAINT [FK_SurveyQuestion_SurveyQuestionResponseType] FOREIGN KEY([ResponseTypeId])
REFERENCES [dbo].[SurveyQuestionResponseType] ([TypeId])
GO
ALTER TABLE [dbo].[SurveyQuestion] CHECK CONSTRAINT [FK_SurveyQuestion_SurveyQuestionResponseType]
GO
ALTER TABLE [dbo].[SurveyQuestion]  WITH CHECK ADD  CONSTRAINT [FK_SurveyQuestion_SurveySection] FOREIGN KEY([SurveySectionId])
REFERENCES [dbo].[SurveySection] ([SurveySectionId])
GO
ALTER TABLE [dbo].[SurveyQuestion] CHECK CONSTRAINT [FK_SurveyQuestion_SurveySection]
GO
ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_SurveyResponse_SurveyAnswer] FOREIGN KEY([SurveyAnswerId])
REFERENCES [dbo].[SurveyAnswer] ([SurveyAnswerId])
GO
ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_SurveyResponse_SurveyAnswer]
GO
ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_SurveyResponse_SurveyCompletedMetaData] FOREIGN KEY([SurveyCompletedMetaDataId])
REFERENCES [dbo].[SurveyCompletedMetaData] ([SurveyCompletedMetaDataId])
GO
ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_SurveyResponse_SurveyCompletedMetaData]
GO
ALTER TABLE [dbo].[SurveyResponse]  WITH CHECK ADD  CONSTRAINT [FK_SurveyResponse_SurveyQuestion] FOREIGN KEY([SurveyQuestionId])
REFERENCES [dbo].[SurveyQuestion] ([SurveyQuestionId])
GO
ALTER TABLE [dbo].[SurveyResponse] CHECK CONSTRAINT [FK_SurveyResponse_SurveyQuestion]
GO
ALTER TABLE [dbo].[SurveySection]  WITH CHECK ADD  CONSTRAINT [FK_SurveySection_Survey] FOREIGN KEY([SurveyId])
REFERENCES [dbo].[Survey] ([SurveyId])
GO
ALTER TABLE [dbo].[SurveySection] CHECK CONSTRAINT [FK_SurveySection_Survey]
GO
ALTER TABLE [dbo].[Team]  WITH CHECK ADD  CONSTRAINT [FK_Team_Survey] FOREIGN KEY([WeeklySurveyId])
REFERENCES [dbo].[Survey] ([SurveyId])
GO
ALTER TABLE [dbo].[Team] CHECK CONSTRAINT [FK_Team_Survey]
GO
ALTER TABLE [dbo].[Team]  WITH CHECK ADD  CONSTRAINT [Fk_UserId_TeamManager] FOREIGN KEY([TeamManager])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[Team] CHECK CONSTRAINT [Fk_UserId_TeamManager]
GO
ALTER TABLE [dbo].[User]  WITH CHECK ADD  CONSTRAINT [Fk_User_Team] FOREIGN KEY([TeamId])
REFERENCES [dbo].[Team] ([TeamId])
GO
ALTER TABLE [dbo].[User] CHECK CONSTRAINT [Fk_User_Team]
GO
ALTER TABLE [dbo].[UserNotificationMapping]  WITH CHECK ADD FOREIGN KEY([NotificationId])
REFERENCES [dbo].[Notification] ([NotificationId])
GO
ALTER TABLE [dbo].[UserNotificationMapping]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserSessionMapping]  WITH CHECK ADD  CONSTRAINT [fk_UserSessionMapping_AddedBy] FOREIGN KEY([AddedBy])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserSessionMapping] CHECK CONSTRAINT [fk_UserSessionMapping_AddedBy]
GO
ALTER TABLE [dbo].[UserSessionMapping]  WITH CHECK ADD  CONSTRAINT [fk_UserSessionMapping_Session] FOREIGN KEY([SessionId])
REFERENCES [dbo].[Session] ([SessionId])
GO
ALTER TABLE [dbo].[UserSessionMapping] CHECK CONSTRAINT [fk_UserSessionMapping_Session]
GO
ALTER TABLE [dbo].[UserSessionMapping]  WITH CHECK ADD  CONSTRAINT [fk_UserSessionMapping_User] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
GO
ALTER TABLE [dbo].[UserSessionMapping] CHECK CONSTRAINT [fk_UserSessionMapping_User]
GO
ALTER TABLE [dbo].[WeeklyFeedbackSurveyMapping]  WITH CHECK ADD  CONSTRAINT [FK_WeeklyFeedbackSurveyMapping_Feedback] FOREIGN KEY([FeedbackId])
REFERENCES [dbo].[Feedback] ([FeedbackId])
GO
ALTER TABLE [dbo].[WeeklyFeedbackSurveyMapping] CHECK CONSTRAINT [FK_WeeklyFeedbackSurveyMapping_Feedback]
GO
ALTER TABLE [dbo].[WeeklyFeedbackSurveyMapping]  WITH CHECK ADD  CONSTRAINT [FK_WeeklyFeedbackSurveyMapping_SurveyCompletedMetaData] FOREIGN KEY([SurveyCompletedMetaDataId])
REFERENCES [dbo].[SurveyCompletedMetaData] ([SurveyCompletedMetaDataId])
GO
ALTER TABLE [dbo].[WeeklyFeedbackSurveyMapping] CHECK CONSTRAINT [FK_WeeklyFeedbackSurveyMapping_SurveyCompletedMetaData]
GO
/****** Object:  StoredProcedure [dbo].[ADD_FEEDBACK]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		HARSH WARDHAN
-- Create date: 21st July, 2016
-- Description:	Adds feedback.
-- =============================================
CREATE PROCEDURE [dbo].[ADD_FEEDBACK]
	@FeedbackText VARCHAR(MAX),
	@Title VARCHAR(100),
	@FeedbackType  int,
	@ProjectId  int = null,
	@SkillId int = null,
	@Rating int = null,
	@AddedBy int,
	@AddedFor int,
	@StartDate date = null,
	@EndDate date = null,
	@AddedOn DateTime = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;

	DECLARE @InsertedId INT;

	INSERT INTO [dbo].[Feedback]
           ([FeedbackText]
           ,[Title]
           ,[FeedbackType]
           ,[ProjectId]
           ,[SkillId]
           ,[Rating]
           ,[AddedBy]
           ,[AddedFor]
		   ,[StartDate]
		   ,[EndDate]
		   ,[AddedOn])
     VALUES
           (@FeedbackText
		   ,@Title
		   ,@FeedbackType
		   ,@ProjectId
		   ,@SkillId
		   ,@Rating
		   ,@AddedBy
		   ,@AddedFor
		   ,@StartDate
		   ,@EndDate
		   ,ISNULL(@AddedOn,GETDATE()))

SELECT @InsertedId= SCOPE_IDENTITY()  

IF(@FeedbackType=2)
BEGIN
	IF NOT EXISTS(SELECT Count(Id) FROM UserSkillMapping WHERE UserId=@AddedFor AND SkillId = @SkillId)
	BEGIN
	INSERT INTO UserSkillMapping
	VALUES
	(
		 @SkillId
		,@AddedFor
		,GETDATE()
		,@AddedBy	
	)
	END
END

SELECT @InsertedId
END



GO
/****** Object:  StoredProcedure [dbo].[ADD_UPDATE_SESSION_DETAILS]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ADD_UPDATE_SESSION_DETAILS]
(
 @Presenter INT,
 @Title NVARCHAR(MAX)='',
 @Description NVARCHAR(MAX)='',
 @Date DATETIME,
 @SessionId INT,
 @AttendeeCsv NVARCHAR(MAX)=''

)
AS 
BEGIN

IF(@SessionId=0)
BEGIN
	INSERT INTO [Session](Title,[Description],Presenter,AddedOn,SessionDate)
	VALUES(@Title,@Description,@Presenter,GETDATE(),@Date)

	SELECT @SessionId= IDENT_CURRENT('Session');
END
ELSE
	BEGIN
	 UPDATE [Session] 
	 SET Title=@Title,[Description]=@Description,SessionDate=@Date
	 WHERE SessionId=@SessionId

	 DELETE FROM UserSessionMapping
	 WHERE SessionId=@SessionId

	 
	END

IF EXISTS(SELECT Id From [dbo].[ufn_CSVToTable](@AttendeeCsv,','))
	BEGIN
	INSERT INTO UserSessionMapping(UserId,SessionId,AddedBy)
	SELECT Value,@SessionId,@Presenter From [dbo].[ufn_CSVToTable](@AttendeeCsv,',')
	END

SELECT @SessionId
END



GO
/****** Object:  StoredProcedure [dbo].[ADD_USER]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ADD_USER]
	@FirstName varchar(100),
	@LastName varchar(100),
	@Username  VARCHAR(200),
	@Password  VARCHAR(200),
	@Email varchar(100),
	@Designation varchar(100),
	@ProfilePictureName varchar(100),
	@IsFemale bit,
	@IsAdministrator bit,
	@IsTrainer bit,
	@IsTrainee bit,
	@IsManager bit,
	@IsActive bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[User]
           ([FirstName]
           ,[LastName]
           ,[UserName]
           ,[Password]
           ,[Email]
           ,[Designation]
           ,[ProfilePictureName]
           ,[IsFemale]
           ,[IsAdministrator]
           ,[IsTrainer]
           ,[IsTrainee]
           ,[IsManager]
           ,[IsActive])
OUTPUT Inserted.UserId
     VALUES
           (@FirstName
           ,@LastName
           ,@Username
           ,@Password
           ,@Email
           ,@Designation
           ,@ProfilePictureName
           ,@IsFemale
           ,@IsAdministrator
           ,@IsTrainer
           ,@IsTrainee
           ,@IsManager
		   ,@IsActive)
END
GO
/****** Object:  StoredProcedure [dbo].[GET_ALL_USERS]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GET_ALL_USERS]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [UserId]
      ,[FirstName]
      ,[LastName]
      ,[UserName]
      ,[Password]
      ,[Email]
      ,[Designation]
      ,[ProfilePictureName]
      ,[IsFemale]
      ,[IsAdministrator]
      ,[IsTrainer]
      ,[IsTrainee]
      ,[IsManager]
      ,[IsActive]
  FROM [dbo].[User]
  ORDER BY [FirstName]
  
END
GO
/****** Object:  StoredProcedure [dbo].[GET_APPLICATION_SKILLS]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	Fetches All the Available Skills for the App.
-- Unit Test : EXEC [GET_APPLICATION_SKILLS]
-- =============================================

CREATE PROCEDURE [dbo].[GET_APPLICATION_SKILLS]
AS
BEGIN

	SELECT SkillId
		  ,Name	 
	FROM [SKILLS]
	ORDER BY Name ASC
	
END
GO
/****** Object:  StoredProcedure [dbo].[GET_DASHBOARD_DATA]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Shekhar Pankaj>
-- Create date: <29 July 2016>
-- Description:	<Fetches Data for Dashboard>
-- =============================================
CREATE PROCEDURE [dbo].[GET_DASHBOARD_DATA]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
 SET NOCOUNT ON;
 
	SELECT 
	   usr.[UserId]
      ,usr.[FirstName] + ' ' + usr.[LastName] AS FullName
	  ,usr.DateAddedToSystem As DateAddedToSystem
	  ,fb.FeedbackType
	  ,fb.FeedbackText
	  ,fb.Title
	  ,CASE WHEN fb.FeedbackType = 2 THEN fb.SkillId 
			ELSE  NULL
	   END AS [SkillId]
	  ,CASE WHEN fb.FeedbackType = 2 THEN s.Name 
			ELSE  NULL
	   END AS [SkillName]
	  ,CASE 
			WHEN fb.FeedbackType = 2  THEN ( SELECT ROUND(AVG(CAST(fbTemp.[Rating] AS FLOAT)),0)	
										   FROM Feedback fbTemp 
										   INNER JOIN [Skills] sTemp ON fbTemp.SkillId = sTemp.SkillId
										   WHERE  fbTemp.SkillId = fb.SkillId
										   GROUP BY fbTemp.SkillId,sTemp.Name )	
			ELSE fb.Rating  
	   END AS [Rating]
	  ,au.[FirstName] + ' ' + au.[LastName] AS AddedBy
	  ,CASE 
			WHEN fb.FeedbackType = 2 THEN NULL
			ELSE fb.AddedOn  
	   END AS [AddedOn]
	  ,CASE 
			WHEN fb.FeedbackType = 5 THEN fb.StartDate
			ELSE NULL 
	   END AS [StartDate]
	   ,CASE 
			WHEN fb.FeedbackType = 5 THEN fb.EndDate
			ELSE NULL 
	   END AS [EndDate]
	  	  
    FROM [dbo].[User] usr
	LEFT JOIN Feedback fb on fb.AddedFor =usr.UserId
	LEFT JOIN [User] au on au.UserId = fb.AddedBy
	LEFT JOIN Skills s on fb.SkillId =s.SkillId	

    WHERE usr.[IsTrainee] = 1 AND usr.[IsActive] = 1
    ORDER BY usr.[FirstName],fb.FeedbackType,fb.SkillId ,fb.AddedOn DESC
	
END







GO
/****** Object:  StoredProcedure [dbo].[GET_PROJECTS_BY_USER_ID]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GET_PROJECTS_BY_USER_ID]
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT p.[ProjectId]
		,[Title]
		,m.[Id] AS UserProjectId
	FROM [dbo].[Project] p
	JOIN [UserProjectMapping] m 
	ON p.ProjectId = m.ProjectId
	WHERE m.UserId = @UserId
				
END


GO
/****** Object:  StoredProcedure [dbo].[GET_SESSIONS_BY_USER_ID]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[GET_SESSIONS_BY_USER_ID]
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT s.[SessionId]
		,[Title]
	FROM [dbo].[Session] s
	JOIN [UserSessionMapping] m
	ON s.[SessionId] = m.[SessionId]
	WHERE m.UserId = @UserId
				
END
GO
/****** Object:  StoredProcedure [dbo].[GET_SESSIONS_ON_FILTERS]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[GET_SESSIONS_ON_FILTERS] --100,'',0,0,26
(
	@PageSize INT = 5,
	@KeyWord NVARCHAR='',
	@SessionType INT=0, 
	@Offset INT = 0,
	@TeamId Int = 0
)

AS
BEGIN
DECLARE @Today DATE = GETDATE()

	SELECT 
		  s.SessionId
		 ,s.Title
		 ,s.[Description]
		 ,s.SessionDate
		 ,usr.FirstName + ' ' + usr.LastName AS PresenterFullName
		 ,s.presenter
		 ,s.VideoFileName
		 ,s.SlideName
		 ,ausr.UserId
		 ,ausr.FirstName
		 ,ausr.LastName
FROM
	(SELECT * 
	 FROM [Session]s
	  ORDER BY SessionDate DESC
		OFFSET  @Offset ROWS 
		FETCH NEXT @PageSize ROWS ONLY ) AS s
	LEFT JOIN [USER] usr on s.Presenter=usr.UserId
	LEFT JOIN  [UserSessionMapping] usm on usm.SessionId=s.SessionId
	LEFT JOIN [User] ausr on ausr.UserId = usm.UserId
	  WHERE  (
			 (
				   (@SessionType = 1  AND SessionDate >=  @Today)
				OR (@SessionType = 2 AND SessionDate <  @Today)
				OR (@SessionType =0 AND SessionDate=SessionDate)
			 ))

			 AND 
			 (
			    usr.TeamId = CASE @TeamId WHEN 0 THEN usr.TeamId ELSE @TeamId END
			 )
		 
END


GO
/****** Object:  StoredProcedure [dbo].[GET_SKILLS_BY_USER_ID]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	
-- Test  - EXEC GET_SKILLS_BY_USER_ID 20
-- =============================================
CREATE PROCEDURE [dbo].[GET_SKILLS_BY_USER_ID]
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--SELECT s.[SkillId]
	--	,[Name]
	--	,(SELECT ROUND(AVG(CAST([Rating] AS FLOAT)),0)
	--		FROM [dbo].[Feedback] f
	--		//JOIN [FeedbackSkillMapping] sk
	--		ON sk.[FeedbackId] = f.[FeedbackId] 
	--		AND sk.[UserId] = @UserId 
	--		AND sk.[SkillId] = s.[SkillId]) AS Rating
	--FROM [dbo].[Skills] s
	--JOIN [UserSkillMapping] m
	--ON s.SkillId = m.SkillId
	--WHERE m.UserId = @UserId

	SELECT  f.SkillId
		  , s.Name
		  , ROUND(AVG(CAST(f.[Rating] AS FLOAT)),0) AS [Rating]
	FROM [Feedback] f
	INNER JOIN [Skills] s ON f.SkillId = s.SkillId
	WHERE f.AddedFor = @UserId
	GROUP BY f.SkillId,s.Name 
				
END


	


GO
/****** Object:  StoredProcedure [dbo].[GET_USER]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GET_USER]
@UserId INT = 0,
@UserName VARCHAR(50) = NULL,
@Scenario INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@Scenario = 2)
	BEGIN
		SET @UserId = (SELECT [UserId]
						FROM [dbo].[User]
						WHERE [UserName] = @UserName)
	END
	
	DECLARE @UserRating AS INT
		--EXEC [GET_USER_RATING] @UserId, @UserRating OUTPUT
	
		SELECT [UserId]
			,[FirstName]
			,[LastName]
			,[UserName]
			,[Password]
			,[Email]
			,[Designation]
			,[ProfilePictureName]
			,[IsFemale]
			,[IsAdministrator]
			,[IsTrainer]
			,[IsTrainee]
			,[IsManager]
			,[IsActive]
			,0 AS UserRating
			--,@UserRating AS UserRating
		FROM [dbo].[User]
		WHERE [UserId] = @UserId
END

GO
/****** Object:  StoredProcedure [dbo].[GET_USER_FEEDBACKS]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	Scenario:
--				1 = Get feedbacks on user id.
--				2 = Gets user feedbacks with pagination.
-- EXEC [GET_USER_FEEDBACKS] @UserId=20,@PageSize=15,@FeedbackId=1
-- =============================================
CREATE PROCEDURE [dbo].[GET_USER_FEEDBACKS]
@UserId INT,
@StartDate DATE = null,
@EndDate DATE = null,
@FeedbackId INT = null,
@PageSize INT = 5,
@Offset INT = 0,
@Scenario INT=0,
@StartAddedOn DATE = null,
@EndAddedOn DATE = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF(@FeedbackId=0)
	BEGIN
		SET @FeedbackId=null
	END

	BEGIN 
		SELECT DISTINCT [FeedbackId]
		  ,[FeedbackText]
		  ,[Title]
		  ,[FeedbackType]
		  ,[Description] AS [FeedbackTypeName]
		  ,[ProjectId]
		  ,[SkillId]
		  ,[Rating]
		  ,[AddedBy]
		  ,u.[FirstName] + ' ' + u.[LastName] AS AddedByUser
		  ,u.ProfilePictureName AS AddedByUserImage
		  ,[AddedFor]
		  ,[AddedOn]
		  ,[StartDate]
		  ,[EndDate]
		  , (SELECT COUNT(ThreadId) From FeedbackThread ft WHERE ft.FeedbackId= f.FeedbackId ) As ThreadCount
		FROM [dbo].[Feedback] f
		JOIN [User] u
		ON f.[AddedBy] = u.[UserId]
		JOIN [FeedbackType] t
		ON f.[FeedbackType] = t.[FeedbackTypeId]
		WHERE f.[AddedFor] = @UserId AND 
			  f.[FeedbackType] = ISNULL(@FeedbackId,f.[FeedbackType]) AND 
			  f.[StartDate] >= ISNULL(@StartDate,f.[StartDate]) AND 
			  f.[EndDate] >= ISNULL(@EndDate,f.[EndDate])  AND 
			  ( CAST( f.[AddedOn] AS DATE) between CAST( ISNULL(@StartAddedOn,f.[AddedOn]) AS DATE) 
			  AND CAST( ISNULL(@EndAddedOn,f.[AddedOn]) AS DATE) )

		ORDER BY [AddedOn] DESC
		OFFSET  @Offset ROWS 
		FETCH NEXT @PageSize ROWS ONLY 
	END

END



GO
/****** Object:  StoredProcedure [dbo].[UPDATE_USER]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UPDATE_USER]
	@UserId INT,
	@FirstName varchar(100),
	@LastName varchar(100),
	@Username  VARCHAR(200),
	@Password  VARCHAR(200),
	@Email varchar(100),
	@Designation varchar(100),
	@ProfilePictureName varchar(100),
	@IsFemale bit,
	@IsAdministrator bit,
	@IsTrainer bit,
	@IsTrainee bit,
	@IsManager bit,
	@IsActive bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	--SET NOCOUNT ON;
	IF (ISNULL(@Password,'') = '') and (ISNULL(@UserId,0)!=0)
	BEGIN
	UPDATE [dbo].[User]
	SET		[FirstName] = @FirstName 
			,[LastName] = @LastName
           ,[UserName] =@Username
           ,[Email]=@Email
           ,[Designation]=@Designation
           ,[ProfilePictureName]=@ProfilePictureName
           ,[IsFemale]=@IsFemale
           ,[IsAdministrator]=@IsAdministrator
           ,[IsTrainer]=@IsTrainer
           ,[IsTrainee]=@IsTrainee
           ,[IsManager]=@IsManager
           ,[IsActive]=@IsActive
     WHERE UserId=@UserId;
	 END
	 ELSE IF (ISNULL(@UserId,0)!=0)
	 BEGIN
	 UPDATE [dbo].[User]
		SET		[FirstName] = @FirstName 
			,[LastName] = @LastName
           ,[UserName] =@Username
           ,[Password]=@Password
           ,[Email]=@Email
           ,[Designation]=@Designation
           ,[ProfilePictureName]=@ProfilePictureName
           ,[IsFemale]=@IsFemale
           ,[IsAdministrator]=@IsAdministrator
           ,[IsTrainer]=@IsTrainer
           ,[IsTrainee]=@IsTrainee
           ,[IsManager]=@IsManager
           ,[IsActive]=@IsActive
		WHERE UserId=@UserId;
	 END

END

GO
/****** Object:  StoredProcedure [dbo].[VALIDATE_USER]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[VALIDATE_USER]
	@Username  VARCHAR(50),
	@Password  VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @RecordCount int

	BEGIN
		SELECT @RecordCount = COUNT(*) 
							FROM [User] 
							WHERE [UserName] = @Username 
							AND [Password] = @Password
	END

	BEGIN
		IF @RecordCount > 0 
			SELECT 1 AS IsValid
		ELSE
			SELECT 0 AS IsValid
	END 

END


GO
/****** Object:  StoredProcedure [dbo].[X_ADD_COMMENT]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[X_ADD_COMMENT]
	@Comment varchar(MAX),
	@AddedBy  int,
	@AddedFor int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Comments]
           ([Comment]
           ,[AddedBy]
           ,[AddedFor])
     VALUES
           (@Comment
           ,@AddedBy
           ,@AddedFor)

END


GO
/****** Object:  StoredProcedure [dbo].[X_ADD_SKILL_FEEDBACK]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[X_ADD_SKILL_FEEDBACK]
	@Feedback varchar(MAX),
	@Rating smallint,
	@AddedBy  int,
	@SkillId  int,
	@UserId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Feedback]
           ([Feedback]
           ,[Rating]
           ,[AddedBy])
     VALUES
           (@Feedback
           ,@Rating
           ,@AddedBy)

	INSERT INTO [dbo].[FeedbackSkillMapping]
           ([SkillId]
           ,[FeedbackId]
           ,[UserId])
     VALUES
           (@SkillId
           ,@@IDENTITY
           ,@UserId)
END


GO
/****** Object:  StoredProcedure [dbo].[X_ADD_USER_PROJECT_FEEDBACK]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[X_ADD_USER_PROJECT_FEEDBACK]
	@Feedback varchar(MAX),
	@Rating smallint,
	@AddedBy  int,
	@UserProjectId  int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Feedback]
           ([Feedback]
           ,[Rating]
           ,[AddedBy])
     VALUES
           (@Feedback
           ,@Rating
           ,@AddedBy)

	INSERT INTO [dbo].[FeedbackUserProjectMapping]
           ([UserProjectId]
           ,[FeedbackId])
     VALUES
           (@UserProjectId
           ,@@IDENTITY)
END


GO
/****** Object:  StoredProcedure [dbo].[X_GET_FEEDBACKS_BY_USER_ID]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[X_GET_FEEDBACKS_BY_USER_ID]
@UserId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT [Feedback] AS FeedbackText
		,'Skill: ' + [Name] AS Title
		,[Rating]
		,f.[AddedOn]
		,u.[FirstName] + ' ' + u.[LastName] AS AddedByUser
		,f.AddedBy
		,u.ProfilePictureName AS AddedByUserImage
		,'Skill Feedback' AS FeedbackType
	FROM [dbo].[Feedback] f			
	JOIN [FeedbackSkillMapping] m
	ON f.[FeedbackId] = m.[FeedbackId]	
	JOIN [Skills] s
	ON s.[SkillId] = m.[SkillId]
	JOIN [User] u
	ON f.[AddedBy] = u.[UserId]
	WHERE m.[UserId] = @UserId
	UNION
	SELECT [Feedback] AS FeedbackText
		,p.[ProjectType] + ': ' + [Title] AS Title
		,[Rating]
		,f.[AddedOn]
		,u.[FirstName] + ' ' + u.[LastName] AS AddedByUser
		,f.AddedBy
		,u.ProfilePictureName AS AddedByUserImage
		,'Project Feedback' AS FeedbackType
	FROM [dbo].[Feedback] f			
	JOIN [FeedbackUserProjectMapping] m
	ON f.[FeedbackId] = m.[FeedbackId]	
	JOIN [UserProjectMapping] pm
	ON pm.[Id] = m.[UserProjectId]
	JOIN [Project] p
	ON pm.[ProjectId] = p.[ProjectId]
	JOIN [User] u
	ON f.[AddedBy] = u.[UserId]
	WHERE pm.[UserId] = @UserId
	UNION
	SELECT c.[Comment] AS FeedbackText
		,'Comment' AS Title
		,0 AS Rating
		,c.[AddedOn]
		,u.[FirstName] + ' ' + u.[LastName] AS AddedByUser
		,c.AddedBy
		,u.ProfilePictureName AS AddedByUserImage
		,'Comment' AS FeedbackType
	FROM [dbo].[Comments] c	
	JOIN [User] u
	ON c.[AddedBy] = u.[UserId]
	WHERE c.[AddedFor] = @UserId
	ORDER BY AddedOn DESC

END
GO
/****** Object:  StoredProcedure [dbo].[X_GET_USER_RATING]    Script Date: 12/9/2016 8:47:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Harsh Wardhan
-- Create date: 13th July, 2016
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[X_GET_USER_RATING]
@UserId INT,
@UserRating INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

		SELECT @UserRating = ROUND(AVG(CAST([Rating] AS FLOAT)),0)
		FROM (SELECT [Rating]
				FROM [dbo].[Feedback] f
				JOIN [FeedbackSkillMapping] s
				ON s.[FeedbackId] = f.[FeedbackId] AND s.[UserId] = @UserId
				UNION ALL
				SELECT [Rating]
				FROM [dbo].[Feedback] f
				JOIN [FeedbackUserProjectMapping] p
				ON p.[FeedbackId] = f.[FeedbackId]
				JOIN [UserProjectMapping] u
				ON u.Id = p.[UserProjectId] AND  u.UserId = @UserId) AS AllRatings

END


GO
USE [master]
GO
ALTER DATABASE [TrainingTracker] SET  READ_WRITE 
GO
