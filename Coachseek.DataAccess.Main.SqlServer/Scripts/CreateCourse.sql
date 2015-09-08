USE [CoachseekLocal-Test]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[Course](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[LocationId] [int] NOT NULL,
	[CoachId] [int] NOT NULL,
	[ServiceId] [int] NOT NULL,
	[Name] [nvarchar](100) NULL,
	[StartDate] [date] NOT NULL,
	[StartTime] [time](7) NOT NULL,
	[Duration] [smallint] NOT NULL,
	[StudentCapacity] [tinyint] NOT NULL,
	[IsOnlineBookable] [bit] NOT NULL,
	[SessionCount] [tinyint] NULL,
	[RepeatFrequency] [char](1) NULL,
	[SessionPrice] [decimal](19, 4) NULL,
	[CoursePrice] [decimal](19, 4) NULL,
	[Colour] [char](12) NOT NULL,
 CONSTRAINT [PK_Course] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING ON
GO

ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO

ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Business]
GO

ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Coach] FOREIGN KEY([CoachId])
REFERENCES [dbo].[Coach] ([Id])
GO

ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Coach]
GO

ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO

ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Location]
GO

ALTER TABLE [dbo].[Course]  WITH CHECK ADD  CONSTRAINT [FK_Course_Service] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([Id])
GO

ALTER TABLE [dbo].[Course] CHECK CONSTRAINT [FK_Course_Service]
GO

