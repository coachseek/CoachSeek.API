USE [master]
GO
/****** Object:  Database [CoachseekLocal-Test]    Script Date: 24/03/2015 9:38:15 AM ******/
CREATE DATABASE [CoachseekLocal-Test]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Coachseek-Test', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Coachseek-Test.mdf' , SIZE = 13312KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Coachseek-Test_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\DATA\Coachseek-Test_log.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [CoachseekLocal-Test] SET COMPATIBILITY_LEVEL = 110
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CoachseekLocal-Test].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CoachseekLocal-Test] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET ARITHABORT OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET AUTO_CREATE_STATISTICS ON 
GO
ALTER DATABASE [CoachseekLocal-Test] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CoachseekLocal-Test] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CoachseekLocal-Test] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET  DISABLE_BROKER 
GO
ALTER DATABASE [CoachseekLocal-Test] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CoachseekLocal-Test] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET RECOVERY FULL 
GO
ALTER DATABASE [CoachseekLocal-Test] SET  MULTI_USER 
GO
ALTER DATABASE [CoachseekLocal-Test] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CoachseekLocal-Test] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CoachseekLocal-Test] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CoachseekLocal-Test] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
EXEC sys.sp_db_vardecimal_storage_format N'CoachseekLocal-Test', N'ON'
GO
USE [CoachseekLocal-Test]
GO
/****** Object:  StoredProcedure [dbo].[Booking_Create]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Booking_Create]	
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier,
	@sessionGuid uniqueidentifier,
	@customerGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @sessionId int
	DECLARE @customerId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	SELECT
		@sessionId = Id
	FROM
		[dbo].[Session]
	WHERE
		[Guid] = @sessionGuid

	SELECT
		@customerId = Id
	FROM
		[dbo].[Customer]
	WHERE
		[Guid] = @customerGuid

	INSERT INTO [dbo].[Booking]
	(
		[BusinessId],
		[Guid],
		[SessionId],
		[CustomerId]
	)
	VALUES
	(
		@businessId,
		@bookingGuid,
		@sessionId,
		@customerId
	)
	
	SELECT
		bk.[Id],
		b.[Guid] AS BusinessGuid,
		bk.[Guid],
		s.[Guid] AS SessionGuid,
		svc.[Name] + ' at ' + l.[Name] 
				   + ' with ' +  co.[FirstName] + ' ' + co.[LastName] 
				   + ' on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
				   + ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) AS SessionName,
		c.[Guid] AS CustomerGuid,
		c.[FirstName] + ' ' + c.[LastName] As CustomerName
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Booking] bk
			ON b.[Id] = bk.[BusinessId]
		INNER JOIN [dbo].[Session] s
			ON s.[Id] = bk.[SessionId]
		INNER JOIN [dbo].[Customer] c
			ON c.[Id] = bk.[CustomerId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] co
			ON co.[Id] = s.[CoachId]
	WHERE
		bk.[Id] = SCOPE_IDENTITY()

END


GO
/****** Object:  StoredProcedure [dbo].[Booking_DeleteByGuid]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Booking_DeleteByGuid]
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
		bk 
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Booking] bk
			ON b.[Id] = bk.[BusinessId]
	WHERE
		b.[Guid] = @businessGuid
		AND bk.[Guid] = @bookingGuid

END

GO
/****** Object:  StoredProcedure [dbo].[Booking_GetAll]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Booking_GetAll]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		bk.[Id],
		b.[Guid] AS BusinessGuid,
		bk.[Guid],
		s.[Guid] AS SessionGuid,
		svc.[Name] + ' at ' + l.[Name] 
				   + ' with ' +  co.[FirstName] + ' ' + co.[LastName] 
				   + ' on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
				   + ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) AS SessionName,
		c.[Guid] AS CustomerGuid,
		c.[FirstName] + ' ' + c.[LastName] As CustomerName
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Booking] bk
			ON b.[Id] = bk.[BusinessId]
		INNER JOIN [dbo].[Session] s
			ON s.[Id] = bk.[SessionId]
		INNER JOIN [dbo].[Customer] c
			ON c.[Id] = bk.[CustomerId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] co
			ON co.[Id] = s.[CoachId]
	WHERE
		b.[Guid] = @businessGuid

END

GO
/****** Object:  StoredProcedure [dbo].[Booking_GetByGuid]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Booking_GetByGuid]
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		bk.[Id],
		b.[Guid] AS BusinessGuid,
		bk.[Guid],
		s.[Guid] AS SessionGuid,
		svc.[Name] + ' at ' + l.[Name] 
				   + ' with ' +  co.[FirstName] + ' ' + co.[LastName] 
				   + ' on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
				   + ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) AS SessionName,
		c.[Guid] AS CustomerGuid,
		c.[FirstName] + ' ' + c.[LastName] As CustomerName
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Booking] bk
			ON b.[Id] = bk.[BusinessId]
		INNER JOIN [dbo].[Session] s
			ON s.[Id] = bk.[SessionId]
		INNER JOIN [dbo].[Customer] c
			ON c.[Id] = bk.[CustomerId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] co
			ON co.[Id] = s.[CoachId]
	WHERE
		b.[Guid] = @businessGuid
		AND bk.[Guid] = @bookingGuid

END

GO
/****** Object:  StoredProcedure [dbo].[Booking_GetBySessionId]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Booking_GetBySessionId]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		bk.[Id],
		b.[Guid] AS BusinessGuid,
		bk.[Guid],
		s.[Guid] AS SessionGuid,
		svc.[Name] + ' at ' + l.[Name] 
				   + ' with ' +  co.[FirstName] + ' ' + co.[LastName] 
				   + ' on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
				   + ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) AS SessionName,
		c.[Guid] AS CustomerGuid,
		c.[FirstName] + ' ' + c.[LastName] As CustomerName
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Booking] bk
			ON b.[Id] = bk.[BusinessId]
		INNER JOIN [dbo].[Session] s
			ON s.[Id] = bk.[SessionId]
		INNER JOIN [dbo].[Customer] c
			ON c.[Id] = bk.[CustomerId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] co
			ON co.[Id] = s.[CoachId]
	WHERE
		b.[Guid] = @businessGuid
		AND s.[Guid] = @sessionGuid

END


GO
/****** Object:  StoredProcedure [dbo].[Business_Create]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Business_Create](
	@guid [uniqueidentifier],
	@name [nvarchar](100),
	@domain [nvarchar](100))
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Business]
	(
		[Guid],
		[Name],
		[Domain]
	)
	VALUES
	(
		@guid,
		@name,
		@domain
	)

	SELECT
		[Id],
		[Guid],
		[Name],
		[Domain]
	FROM 
		[dbo].[Business]
	WHERE
		[Id] = SCOPE_IDENTITY()

END


GO
/****** Object:  StoredProcedure [dbo].[Business_GetByDomain]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Business_GetByDomain]
	@domain nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		*
	FROM
		[dbo].[Business]
	WHERE
		[Domain] = @domain
END


GO
/****** Object:  StoredProcedure [dbo].[Coach_Create]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Coach_Create]	
	@businessGuid uniqueidentifier,
	@coachGuid uniqueidentifier,
	@firstName [nvarchar](50),
	@lastName [nvarchar](50),
	@email [nvarchar](100),
	@phone [nvarchar](50),
	@mondayIsAvailable bit,
	@mondayStartTime [nchar](5) = NULL,
	@mondayFinishTime [nchar](5) = NULL,
	@tuesdayIsAvailable bit,
	@tuesdayStartTime [nchar](5) = NULL,
	@tuesdayFinishTime [nchar](5) = NULL,
	@wednesdayIsAvailable bit,
	@wednesdayStartTime [nchar](5) = NULL,
	@wednesdayFinishTime [nchar](5) = NULL,
	@thursdayIsAvailable bit,
	@thursdayStartTime [nchar](5) = NULL,
	@thursdayFinishTime [nchar](5) = NULL,
	@fridayIsAvailable bit,
	@fridayStartTime [nchar](5) = NULL,
	@fridayFinishTime [nchar](5) = NULL,
	@saturdayIsAvailable bit,
	@saturdayStartTime [nchar](5) = NULL,
	@saturdayFinishTime [nchar](5) = NULL,
	@sundayIsAvailable bit,
	@sundayStartTime [nchar](5) = NULL,
	@sundayFinishTime [nchar](5) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	INSERT INTO [dbo].[Coach]
	(
		[BusinessId],
		[Guid],
		[FirstName],
		[LastName],
		[Email],
		[Phone],
		[MondayIsAvailable],
		[MondayStartTime],
		[MondayFinishTime],
		[TuesdayIsAvailable],
		[TuesdayStartTime],
		[TuesdayFinishTime],
		[WednesdayIsAvailable],
		[WednesdayStartTime],
		[WednesdayFinishTime],
		[ThursdayIsAvailable],
		[ThursdayStartTime],
		[ThursdayFinishTime],
		[FridayIsAvailable],
		[FridayStartTime],
		[FridayFinishTime],
		[SaturdayIsAvailable],
		[SaturdayStartTime],
		[SaturdayFinishTime],
		[SundayIsAvailable],
		[SundayStartTime],
		[SundayFinishTime]
	)
	VALUES
	(
		@businessId,
		@coachGuid,
		@firstName,
		@lastName,
		@email,
		@phone,
		@mondayIsAvailable,
		@mondayStartTime,
		@mondayFinishTime,
		@tuesdayIsAvailable,
		@tuesdayStartTime,
		@tuesdayFinishTime,
		@wednesdayIsAvailable,
		@wednesdayStartTime,
		@wednesdayFinishTime,
		@thursdayIsAvailable,
		@thursdayStartTime,
		@thursdayFinishTime,
		@fridayIsAvailable,
		@fridayStartTime,
		@fridayFinishTime,
		@saturdayIsAvailable,
		@saturdayStartTime,
		@saturdayFinishTime,
		@sundayIsAvailable,
		@sundayStartTime,
		@sundayFinishTime
	)

	SELECT
		c.[Id],
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone],
		c.[MondayIsAvailable],
		c.[MondayStartTime],
		c.[MondayFinishTime],
		c.[TuesdayIsAvailable],
		c.[TuesdayStartTime],
		c.[TuesdayFinishTime],
		c.[WednesdayIsAvailable],
		c.[WednesdayStartTime],
		c.[WednesdayFinishTime],
		c.[ThursdayIsAvailable],
		c.[ThursdayStartTime],
		c.[ThursdayFinishTime],
		c.[FridayIsAvailable],
		c.[FridayStartTime],
		c.[FridayFinishTime],
		c.[SaturdayIsAvailable],
		c.[SaturdayStartTime],
		c.[SaturdayFinishTime],
		c.[SundayIsAvailable],
		c.[SundayStartTime],
		c.[SundayFinishTime]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Coach] c
			ON b.Id = c.BusinessId
	WHERE
		c.[Id] = SCOPE_IDENTITY()

END



GO
/****** Object:  StoredProcedure [dbo].[Coach_GetAll]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Coach_GetAll]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		c.[Id],
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone],
		c.[MondayIsAvailable],
		c.[MondayStartTime],
		c.[MondayFinishTime],
		c.[TuesdayIsAvailable],
		c.[TuesdayStartTime],
		c.[TuesdayFinishTime],
		c.[WednesdayIsAvailable],
		c.[WednesdayStartTime],
		c.[WednesdayFinishTime],
		c.[ThursdayIsAvailable],
		c.[ThursdayStartTime],
		c.[ThursdayFinishTime],
		c.[FridayIsAvailable],
		c.[FridayStartTime],
		c.[FridayFinishTime],
		c.[SaturdayIsAvailable],
		c.[SaturdayStartTime],
		c.[SaturdayFinishTime],
		c.[SundayIsAvailable],
		c.[SundayStartTime],
		c.[SundayFinishTime]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Coach] c
			ON b.Id = c.BusinessId
	WHERE
		b.[Guid] = @businessGuid
	ORDER BY
		c.[FirstName],
		c.[LastName]

END


GO
/****** Object:  StoredProcedure [dbo].[Coach_GetByGuid]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Coach_GetByGuid]
	@businessGuid uniqueidentifier,
	@coachGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		c.[Id],
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone],
		c.[MondayIsAvailable],
		c.[MondayStartTime],
		c.[MondayFinishTime],
		c.[TuesdayIsAvailable],
		c.[TuesdayStartTime],
		c.[TuesdayFinishTime],
		c.[WednesdayIsAvailable],
		c.[WednesdayStartTime],
		c.[WednesdayFinishTime],
		c.[ThursdayIsAvailable],
		c.[ThursdayStartTime],
		c.[ThursdayFinishTime],
		c.[FridayIsAvailable],
		c.[FridayStartTime],
		c.[FridayFinishTime],
		c.[SaturdayIsAvailable],
		c.[SaturdayStartTime],
		c.[SaturdayFinishTime],
		c.[SundayIsAvailable],
		c.[SundayStartTime],
		c.[SundayFinishTime]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Coach] c
			ON b.Id = c.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND c.[Guid] = @coachGuid
END


GO
/****** Object:  StoredProcedure [dbo].[Coach_Update]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Coach_Update]	
	@businessGuid uniqueidentifier,
	@coachGuid uniqueidentifier,
	@firstName [nvarchar](50),
	@lastName [nvarchar](50),
	@email [nvarchar](100),
	@phone [nvarchar](50),
	@mondayIsAvailable bit,
	@mondayStartTime [nchar](5) = NULL,
	@mondayFinishTime [nchar](5) = NULL,
	@tuesdayIsAvailable bit,
	@tuesdayStartTime [nchar](5) = NULL,
	@tuesdayFinishTime [nchar](5) = NULL,
	@wednesdayIsAvailable bit,
	@wednesdayStartTime [nchar](5) = NULL,
	@wednesdayFinishTime [nchar](5) = NULL,
	@thursdayIsAvailable bit,
	@thursdayStartTime [nchar](5) = NULL,
	@thursdayFinishTime [nchar](5) = NULL,
	@fridayIsAvailable bit,
	@fridayStartTime [nchar](5) = NULL,
	@fridayFinishTime [nchar](5) = NULL,
	@saturdayIsAvailable bit,
	@saturdayStartTime [nchar](5) = NULL,
	@saturdayFinishTime [nchar](5) = NULL,
	@sundayIsAvailable bit,
	@sundayStartTime [nchar](5) = NULL,
	@sundayFinishTime [nchar](5) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	UPDATE
		[dbo].[Coach]
	SET 
		[FirstName] = @firstName,
		[LastName] = @lastName,
		[Email] = @email,
		[Phone] = @phone,
		[MondayIsAvailable] = @mondayIsAvailable,
		[MondayStartTime] = @mondayStartTime,
		[MondayFinishTime] = @mondayFinishTime,
		[TuesdayIsAvailable] = @tuesdayIsAvailable,
		[TuesdayStartTime] = @tuesdayStartTime,
		[TuesdayFinishTime] = @tuesdayFinishTime,
		[WednesdayIsAvailable] = @wednesdayIsAvailable,
		[WednesdayStartTime] = @wednesdayStartTime,
		[WednesdayFinishTime] = @wednesdayFinishTime,
		[ThursdayIsAvailable] = @thursdayIsAvailable,
		[ThursdayStartTime] = @thursdayStartTime,
		[ThursdayFinishTime] = @thursdayFinishTime,
		[FridayIsAvailable] = @fridayIsAvailable,
		[FridayStartTime] = @fridayStartTime,
		[FridayFinishTime] = @fridayFinishTime,
		[SaturdayIsAvailable] = @saturdayIsAvailable,
		[SaturdayStartTime] = @saturdayStartTime,
		[SaturdayFinishTime] = @saturdayFinishTime,
		[SundayIsAvailable] = @sundayIsAvailable,
		[SundayStartTime] = @sundayStartTime,
		[SundayFinishTime] = @sundayFinishTime
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @coachGuid

	SELECT
		*
	FROM 
		[dbo].[Coach]
	WHERE
		[BusinessId] = @businessId
		AND [Guid] = @coachGuid

END


GO
/****** Object:  StoredProcedure [dbo].[Customer_Create]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Customer_Create]	
	@businessGuid uniqueidentifier,
	@customerGuid uniqueidentifier,
	@firstName [nvarchar](50),
	@lastName [nvarchar](50),
	@email [nvarchar](100) = NULL,
	@phone [nvarchar](50) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	INSERT INTO [dbo].[Customer]
	(
		[BusinessId],
		[Guid],
		[FirstName],
		[LastName],
		[Email],
		[Phone]
	)
	VALUES
	(
		@businessId,
		@customerGuid,
		@firstName,
		@lastName,
		@email,
		@phone
	)

	SELECT
		c.[Id],
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Customer] c
			ON b.Id = c.BusinessId
	WHERE
		c.[Id] = SCOPE_IDENTITY()

END



GO
/****** Object:  StoredProcedure [dbo].[Customer_GetAll]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Customer_GetAll]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		c.[Id],
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Customer] c
			ON b.Id = c.BusinessId
	WHERE
		b.[Guid] = @businessGuid
	ORDER BY
		c.[LastName], 
		c.[FirstName]

END


GO
/****** Object:  StoredProcedure [dbo].[Customer_GetByGuid]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Customer_GetByGuid]
	@businessGuid uniqueidentifier,
	@customerGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		c.[Id],
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Customer] c
			ON b.Id = c.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND c.[Guid] = @customerGuid
END


GO
/****** Object:  StoredProcedure [dbo].[Customer_GetCustomerBookingsBySessionId]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Customer_GetCustomerBookingsBySessionId]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		c.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid] AS SessionGuid,
		c.[Guid] AS CustomerGuid,
		bk.[Guid] AS BookingGuid,
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Booking] bk
			ON b.Id = bk.BusinessId
		LEFT JOIN [dbo].[Session] s
			ON bk.[SessionId] = s.[Id]
		LEFT JOIN [dbo].[Customer] c
			ON bk.[CustomerId] = c.[Id]
	WHERE
		b.[Guid] = @businessGuid
		AND s.[Guid] = @sessionGuid

END


GO
/****** Object:  StoredProcedure [dbo].[Customer_Update]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Customer_Update]	
	@businessGuid uniqueidentifier,
	@customerGuid uniqueidentifier,
	@firstName [nvarchar](50),
	@lastName [nvarchar](50),
	@email [nvarchar](100),
	@phone [nvarchar](50)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	UPDATE
		[dbo].[Customer]
	SET 
		[FirstName] = @firstName,
		[LastName] = @lastName,
		[Email] = @email,
		[Phone] = @phone
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @customerGuid

	SELECT
		*
	FROM 
		[dbo].[Customer]
	WHERE
		[BusinessId] = @businessId
		AND [Guid] = @customerGuid

END


GO
/****** Object:  StoredProcedure [dbo].[Location_Create]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Location_Create]	
	@businessGuid uniqueidentifier,
	@locationGuid uniqueidentifier,
	@name [nvarchar](100)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	INSERT INTO [dbo].[Location]
	(
		[BusinessId],
		[Guid],
		[Name]
	)
	VALUES
	(
		@businessId,
		@locationGuid,
		@name
	)

	SELECT
		l.[Id],
		b.[Guid] AS BusinessGuid,
		l.[Guid],
		l.[Name]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Location] l
			ON b.Id = l.BusinessId
	WHERE
		l.[Id] = SCOPE_IDENTITY()

END


GO
/****** Object:  StoredProcedure [dbo].[Location_GetAll]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Location_GetAll]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		l.[Id],
		b.[Guid] AS BusinessGuid,
		l.[Guid],
		l.[Name]	
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Location] l
			ON b.Id = l.BusinessId
	WHERE
		b.[Guid] = @businessGuid
END


GO
/****** Object:  StoredProcedure [dbo].[Location_GetByGuid]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Location_GetByGuid]
	@businessGuid uniqueidentifier,
	@locationGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		l.[Id],
		b.[Guid] AS BusinessGuid,
		l.[Guid],
		l.[Name]	
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Location] l
			ON b.Id = l.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND l.[Guid] = @locationGuid
END


GO
/****** Object:  StoredProcedure [dbo].[Location_Update]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Location_Update]	
	@businessGuid uniqueidentifier,
	@locationGuid uniqueidentifier,
	@name [nvarchar](100)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	UPDATE
		[dbo].[Location]
	SET 
		[Name] = @name
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @locationGuid

	SELECT
		*
	FROM 
		[dbo].[Location]
	WHERE
		[BusinessId] = @businessId
		AND [Guid] = @locationGuid

END


GO
/****** Object:  StoredProcedure [dbo].[Service_Create]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Service_Create]	
	@businessGuid uniqueidentifier,
	@serviceGuid uniqueidentifier,
	@name [nvarchar](100),
	@description [nvarchar](500) = NULL,
	@duration [smallint] = NULL,
	@studentCapacity [tinyint] = NULL,
	@isOnlineBookable [bit] = NULL,
	@sessionCount [tinyint] = NULL,
	@repeatFrequency [char](1) = NULL,
	@sessionPrice [decimal](19, 4) = NULL,
	@coursePrice [decimal](19, 4) = NULL,
	@colour [char](12)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	INSERT INTO [dbo].[Service]
	(
		[BusinessId],
		[Guid],
		[Name],
		[Description],
		[Duration],
		[StudentCapacity],
		[IsOnlineBookable],
		[SessionCount],
		[RepeatFrequency],
		[SessionPrice],
		[CoursePrice],
		[Colour]
	)
	VALUES
	(
		@businessId,
		@serviceGuid,
		@name,
		@description,
		@duration,
		@studentCapacity,
		@isOnlineBookable,
		@sessionCount,
		@repeatFrequency,
		@sessionPrice,
		@coursePrice,
		@colour
	)

	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s.[Name],
		s.[Description],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Service] s
			ON b.Id = s.BusinessId
	WHERE
		s.[Id] = SCOPE_IDENTITY()

END



GO
/****** Object:  StoredProcedure [dbo].[Service_GetAll]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Service_GetAll]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s.[Name],
		s.[Description],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Service] s
			ON b.Id = s.BusinessId
	WHERE
		b.[Guid] = @businessGuid
	ORDER BY
		s.[Name]
END


GO
/****** Object:  StoredProcedure [dbo].[Service_GetByGuid]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Service_GetByGuid]
	@businessGuid uniqueidentifier,
	@serviceGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s.[Name],
		s.[Description],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Service] s
			ON b.Id = s.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND s.[Guid] = @serviceGuid
END


GO
/****** Object:  StoredProcedure [dbo].[Service_Update]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Service_Update]	
	@businessGuid uniqueidentifier,
	@serviceGuid uniqueidentifier,
	@name [nvarchar](100),
	@description [nvarchar](500) = NULL,
	@duration [smallint] = NULL,
	@studentCapacity [tinyint] = NULL,
	@isOnlineBookable [bit] = NULL,
	@sessionCount [tinyint] = NULL,
	@repeatFrequency [char](1) = NULL,
	@sessionPrice [decimal](19, 4) = NULL,
	@coursePrice [decimal](19, 4) = NULL,
	@colour [char](12)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	UPDATE
		[dbo].[Service]
	SET 
		[Name] = @name,
		[Description] = @description,
		[Duration] = @duration,
		[StudentCapacity] = @studentCapacity,
		[IsOnlineBookable] = @isOnlineBookable,
		[SessionCount] = @sessionCount,
		[RepeatFrequency] = @repeatFrequency,
		[SessionPrice] = @sessionPrice,
		[CoursePrice] = @coursePrice,
		[Colour] = @colour
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @serviceGuid

	SELECT
		*
	FROM 
		[dbo].[Service]
	WHERE
		[BusinessId] = @businessId
		AND [Guid] = @serviceGuid

END


GO
/****** Object:  StoredProcedure [dbo].[Session_CreateCourse]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Session_CreateCourse]
	@businessGuid uniqueidentifier,
	@courseGuid uniqueidentifier,
	@locationGuid uniqueidentifier,
	@coachGuid uniqueidentifier,
	@serviceGuid uniqueidentifier,
	@name [nvarchar](100) = NULL,
	@startDate [date],
	@startTime [time](7),
	@duration [smallint],
	@studentCapacity [tinyint],
	@isOnlineBookable [bit],
	@sessionCount [tinyint],
	@repeatFrequency [char](1),
	@sessionPrice [decimal](19, 4) = NULL,
	@coursePrice [decimal](19, 4) = NULL,
	@colour [char](12)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @locationId int
	DECLARE @coachId int
	DECLARE @serviceId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	SELECT
		@locationId = Id
	FROM
		[dbo].[Location]
	WHERE
		[Guid] = @locationGuid

	SELECT
		@coachId = Id
	FROM
		[dbo].[Coach]
	WHERE
		[Guid] = @coachGuid

	SELECT
		@serviceId = Id
	FROM
		[dbo].[Service]
	WHERE
		[Guid] = @serviceGuid

	INSERT INTO [dbo].[Session]
	(
		[BusinessId],
		[Guid],
		[ParentId],
		[LocationId],
		[CoachId],
		[ServiceId],
		[Name],
		[StartDate],
		[StartTime],
		[Duration],
		[StudentCapacity],
		[IsOnlineBookable],
		[SessionCount],
		[RepeatFrequency],
		[SessionPrice],
		[CoursePrice],
		[Colour]
	)
	VALUES
	(
		@businessId,
		@courseGuid,
		NULL,
		@locationId,
		@coachId,
		@serviceId,
		@name,
		@startDate,
		@startTime,
		@duration,
		@studentCapacity,
		@isOnlineBookable,
		@sessionCount,
		@repeatFrequency,
		@sessionPrice,
		@coursePrice,
		@colour
	)

	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.Id = s.BusinessId
		LEFT JOIN [dbo].[Session] s2
			ON s2.Id = s.ParentId
		LEFT JOIN [dbo].[Location] l
			ON l.Id = s.LocationId
		LEFT JOIN [dbo].[Coach] c
			ON c.Id = s.CoachId
		LEFT JOIN [dbo].[Service] svc
			ON svc.Id = s.ServiceId
	WHERE
		s.[Id] = SCOPE_IDENTITY()

END



GO
/****** Object:  StoredProcedure [dbo].[Session_CreateSession]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Session_CreateSession]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier,
	@parentGuid uniqueidentifier = NULL,
	@locationGuid uniqueidentifier,
	@coachGuid uniqueidentifier,
	@serviceGuid uniqueidentifier,
	@name [nvarchar](100) = NULL,
	@startDate [date],
	@startTime [time](7),
	@duration [smallint],
	@studentCapacity [tinyint],
	@isOnlineBookable [bit],
	@sessionCount [tinyint] = NULL,
	@repeatFrequency [char](1) = NULL,
	@sessionPrice [decimal](19, 4) = NULL,
	@coursePrice [decimal](19, 4) = NULL,
	@colour [char](12)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @parentId int
	DECLARE @locationId int
	DECLARE @coachId int
	DECLARE @serviceId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	SELECT
		@parentId = Id
	FROM
		[dbo].[Session]
	WHERE
		[Guid] = @parentGuid

	SELECT
		@locationId = Id
	FROM
		[dbo].[Location]
	WHERE
		[Guid] = @locationGuid

	SELECT
		@coachId = Id
	FROM
		[dbo].[Coach]
	WHERE
		[Guid] = @coachGuid

	SELECT
		@serviceId = Id
	FROM
		[dbo].[Service]
	WHERE
		[Guid] = @serviceGuid

	INSERT INTO [dbo].[Session]
	(
		[BusinessId],
		[Guid],
		[ParentId],
		[LocationId],
		[CoachId],
		[ServiceId],
		[Name],
		[StartDate],
		[StartTime],
		[Duration],
		[StudentCapacity],
		[IsOnlineBookable],
		[SessionCount],
		[RepeatFrequency],
		[SessionPrice],
		[CoursePrice],
		[Colour]
	)
	VALUES
	(
		@businessId,
		@sessionGuid,
		@parentId,
		@locationId,
		@coachId,
		@serviceId,
		@name,
		@startDate,
		@startTime,
		@duration,
		@studentCapacity,
		@isOnlineBookable,
		@sessionCount,
		@repeatFrequency,
		@sessionPrice,
		@coursePrice,
		@colour
	)

	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s2.[Guid] AS ParentGuid,
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.Id = s.BusinessId
		LEFT JOIN [dbo].[Session] s2
			ON s2.Id = s.ParentId
		LEFT JOIN [dbo].[Location] l
			ON l.Id = s.LocationId
		LEFT JOIN [dbo].[Coach] c
			ON c.Id = s.CoachId
		LEFT JOIN [dbo].[Service] svc
			ON svc.Id = s.ServiceId
	WHERE
		s.[Id] = SCOPE_IDENTITY()

END



GO
/****** Object:  StoredProcedure [dbo].[Session_DeleteByGuid]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Session_DeleteByGuid]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;


	WITH Course AS 
	( 
		SELECT 
			s.[ID], 
			s.[ParentId]
		FROM 
			[dbo].[Session] s
			INNER JOIN [dbo].[Business] b
				ON b.[Id] = s.[BusinessId] 
		WHERE
			s.[Guid] = @sessionGuid
			AND b.[Guid] = @businessGuid
		UNION ALL 
		SELECT
			s2.[ID], 
			s2.[ParentId]
		FROM
			[Session] s2
			INNER JOIN Course 
				ON Course.[ID] = s2.[ParentId] 
	)
	
	DELETE 
		s
	FROM 
		[dbo].[Session] s
		INNER JOIN Course c
			ON s.[Id] = c.[Id]

END

GO
/****** Object:  StoredProcedure [dbo].[Session_GetAllCourses]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Session_GetAllCourses]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.[Id] = s.[BusinessId]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] c
			ON c.[Id] = s.[CoachId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
	WHERE
		b.[Guid] = @businessGuid
		AND s.[ParentId] IS NULL
		AND s.[SessionCount] > 1
	ORDER BY
		s.[Name]
END


GO
/****** Object:  StoredProcedure [dbo].[Session_GetAllCourseSessions]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Session_GetAllCourseSessions]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s2.[Guid] AS ParentGuid,
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.[Id] = s.[BusinessId]
		INNER JOIN [dbo].[Session] s2
			ON s.[ParentId] = s2.[Id]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] c
			ON c.[Id] = s.[CoachId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
	WHERE
		b.[Guid] = @businessGuid
		AND s.[SessionCount] = 1
	ORDER BY
		s.[Name]
END


GO
/****** Object:  StoredProcedure [dbo].[Session_GetAllSessions]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Session_GetAllSessions]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s2.[Guid] AS ParentGuid,
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.[Id] = s.[BusinessId]
		LEFT JOIN [dbo].[Session] s2
			ON s.[ParentId] = s2.[Id]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] c
			ON c.[Id] = s.[CoachId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
	WHERE
		b.[Guid] = @businessGuid
		AND s.[SessionCount] = 1
	ORDER BY
		s.[Name]
END


GO
/****** Object:  StoredProcedure [dbo].[Session_GetAllStandaloneSessions]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Session_GetAllStandaloneSessions]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		NULL AS ParentGuid,
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.[Id] = s.[BusinessId]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] c
			ON c.[Id] = s.[CoachId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
	WHERE
		b.[Guid] = @businessGuid
		AND s.[ParentId] IS NULL
		AND s.[SessionCount] = 1
	ORDER BY
		s.[Name]
END


GO
/****** Object:  StoredProcedure [dbo].[Session_GetCourseByGuid]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Session_GetCourseByGuid]
	@businessGuid uniqueidentifier,
	@courseGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	WITH Course AS 
	( 
		SELECT 
			s.[ID], 
			s.[ParentId]
		FROM 
			[dbo].[Session] s
			INNER JOIN [dbo].[Business] b
				ON b.[Id] = s.[BusinessId] 
		WHERE
			s.[Guid] = @courseGuid
			AND b.[Guid] = @businessGuid
		UNION ALL 
		SELECT
			s2.[ID], 
			s2.[ParentId]
		FROM
			[Session] s2
			INNER JOIN Course 
				ON Course.[ID] = s2.[ParentId] 
	)

	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s2.[Guid] AS ParentGuid,
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM
		Course crs
		INNER JOIN [dbo].[Session] s
			ON s.[Id] = crs.[Id]
		INNER JOIN [dbo].[Business] b
			ON b.[Id] = s.[BusinessId]
		LEFT JOIN [dbo].[Session] s2
			ON s.[ParentId] = s2.[Id]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] c
			ON c.[Id] = s.[CoachId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]

	ORDER BY
		s2.[Guid]

END


GO
/****** Object:  StoredProcedure [dbo].[Session_GetSessionByGuid]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Session_GetSessionByGuid]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s2.[Guid] AS ParentGuid,
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.Id = s.BusinessId
		LEFT JOIN [dbo].[Session] s2
			ON s.[ParentId] = s2.[Id]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] c
			ON c.[Id] = s.[CoachId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
	WHERE
		b.[Guid] = @businessGuid
		AND s.[Guid] = @sessionGuid
		AND s.[SessionCount] = 1
END


GO
/****** Object:  StoredProcedure [dbo].[Session_UpdateCourse]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Session_UpdateCourse]
	@businessGuid uniqueidentifier,
	@courseGuid uniqueidentifier,
	@locationGuid uniqueidentifier,
	@coachGuid uniqueidentifier,
	@serviceGuid uniqueidentifier,
	@name [nvarchar](100) = NULL,
	@startDate [date],
	@startTime [time](7),
	@duration [smallint],
	@studentCapacity [tinyint],
	@isOnlineBookable [bit],
	@sessionCount [tinyint] = NULL,
	@repeatFrequency [char](1) = NULL,
	@sessionPrice [decimal](19, 4) = NULL,
	@coursePrice [decimal](19, 4) = NULL,
	@colour [char](12)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @locationId int
	DECLARE @coachId int
	DECLARE @serviceId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	SELECT
		@locationId = Id
	FROM
		[dbo].[Location]
	WHERE
		[Guid] = @locationGuid

	SELECT
		@coachId = Id
	FROM
		[dbo].[Coach]
	WHERE
		[Guid] = @coachGuid

	SELECT
		@serviceId = Id
	FROM
		[dbo].[Service]
	WHERE
		[Guid] = @serviceGuid

	UPDATE
		[dbo].[Session]
	SET 
		[LocationId] = @locationId,
		[CoachId] = @coachId,
		[ServiceId] = @serviceId,
		[Name] = @name,
		[StartDate] = @startDate,
		[StartTime] = @startTime,
		[Duration] = @duration,
		[StudentCapacity] = @studentCapacity,
		[IsOnlineBookable] = @isOnlineBookable,
		[SessionCount] = @sessionCount,
		[RepeatFrequency] = @repeatFrequency,
		[SessionPrice] = @sessionPrice,
		[CoursePrice] = @coursePrice,
		[Colour] = @colour
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @courseGuid

	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s2.[Guid] AS ParentGuid,
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.Id = s.BusinessId
		LEFT JOIN [dbo].[Session] s2
			ON s2.Id = s.ParentId
		LEFT JOIN [dbo].[Location] l
			ON l.Id = s.LocationId
		LEFT JOIN [dbo].[Coach] c
			ON c.Id = s.CoachId
		LEFT JOIN [dbo].[Service] svc
			ON svc.Id = s.ServiceId
	WHERE
		b.[Id] = @businessId
		AND s.[Guid] = @courseGuid

END


GO
/****** Object:  StoredProcedure [dbo].[Session_UpdateSession]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Session_UpdateSession]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier,
	@parentGuid uniqueidentifier = NULL,
	@locationGuid uniqueidentifier,
	@coachGuid uniqueidentifier,
	@serviceGuid uniqueidentifier,
	@name [nvarchar](100) = NULL,
	@startDate [date],
	@startTime [time](7),
	@duration [smallint],
	@studentCapacity [tinyint],
	@isOnlineBookable [bit],
	@sessionCount [tinyint] = NULL,
	@repeatFrequency [char](1) = NULL,
	@sessionPrice [decimal](19, 4) = NULL,
	@coursePrice [decimal](19, 4) = NULL,
	@colour [char](12)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @locationId int
	DECLARE @coachId int
	DECLARE @serviceId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	SELECT
		@locationId = Id
	FROM
		[dbo].[Location]
	WHERE
		[Guid] = @locationGuid

	SELECT
		@coachId = Id
	FROM
		[dbo].[Coach]
	WHERE
		[Guid] = @coachGuid

	SELECT
		@serviceId = Id
	FROM
		[dbo].[Service]
	WHERE
		[Guid] = @serviceGuid

	UPDATE
		[dbo].[Session]
	SET 
		-- ParentId is not updateable yet.
		[LocationId] = @locationId,
		[CoachId] = @coachId,
		[ServiceId] = @serviceId,
		[Name] = @name,
		[StartDate] = @startDate,
		[StartTime] = @startTime,
		[Duration] = @duration,
		[StudentCapacity] = @studentCapacity,
		[IsOnlineBookable] = @isOnlineBookable,
		[SessionCount] = @sessionCount,
		[RepeatFrequency] = @repeatFrequency,
		[SessionPrice] = @sessionPrice,
		[CoursePrice] = @coursePrice,
		[Colour] = @colour
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @sessionGuid

	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s2.[Guid] AS ParentGuid,
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.Id = s.BusinessId
		LEFT JOIN [dbo].[Session] s2
			ON s2.Id = s.ParentId
		LEFT JOIN [dbo].[Location] l
			ON l.Id = s.LocationId
		LEFT JOIN [dbo].[Coach] c
			ON c.Id = s.CoachId
		LEFT JOIN [dbo].[Service] svc
			ON svc.Id = s.ServiceId
	WHERE
		b.[Id] = @businessId
		AND s.[Guid] = @sessionGuid

END


GO
/****** Object:  Table [dbo].[Booking]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Booking](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Booking_Guid]  DEFAULT (newid()),
	[CustomerId] [int] NOT NULL,
	[SessionId] [int] NOT NULL,
 CONSTRAINT [PK_Booking] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Business]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Business](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Business_Guid]  DEFAULT (newid()),
	[Name] [nvarchar](100) NOT NULL,
	[Domain] [nvarchar](100) NOT NULL CONSTRAINT [DF_Business_Domain]  DEFAULT (''),
 CONSTRAINT [PK_Business] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BusinessUser]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BusinessUser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_BusinessUser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Coach]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coach](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NOT NULL,
	[Phone] [nvarchar](50) NOT NULL,
	[MondayIsAvailable] [bit] NOT NULL CONSTRAINT [DF_Coach_MondayIsAvailable]  DEFAULT ((0)),
	[MondayStartTime] [nchar](5) NULL,
	[MondayFinishTime] [nchar](5) NULL,
	[TuesdayIsAvailable] [bit] NOT NULL CONSTRAINT [DF_Coach_TuesdayIsAvailable]  DEFAULT ((0)),
	[TuesdayStartTime] [nchar](5) NULL,
	[TuesdayFinishTime] [nchar](5) NULL,
	[WednesdayIsAvailable] [bit] NOT NULL CONSTRAINT [DF_Coach_WednesdayIsAvailable]  DEFAULT ((0)),
	[WednesdayStartTime] [nchar](5) NULL,
	[WednesdayFinishTime] [nchar](5) NULL,
	[ThursdayIsAvailable] [bit] NOT NULL CONSTRAINT [DF_Coach_ThursdayIsAvailable]  DEFAULT ((0)),
	[ThursdayStartTime] [nchar](5) NULL,
	[ThursdayFinishTime] [nchar](5) NULL,
	[FridayIsAvailable] [bit] NOT NULL CONSTRAINT [DF_Coach_FridayIsAvailable]  DEFAULT ((0)),
	[FridayStartTime] [nchar](5) NULL,
	[FridayFinishTime] [nchar](5) NULL,
	[SaturdayIsAvailable] [bit] NOT NULL CONSTRAINT [DF_Coach_SaturdayIsAvailable]  DEFAULT ((0)),
	[SaturdayStartTime] [nchar](5) NULL,
	[SaturdayFinishTime] [nchar](5) NULL,
	[SundayIsAvailable] [bit] NOT NULL CONSTRAINT [DF_Coach_SundayIsAvailable]  DEFAULT ((0)),
	[SundayStartTime] [nchar](5) NULL,
	[SundayFinishTime] [nchar](5) NULL,
 CONSTRAINT [PK_Coach] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Colour]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Colour](
	[Id] [tinyint] NOT NULL,
	[Name] [char](20) NOT NULL,
 CONSTRAINT [PK_Colour] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Customer](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Customer_Guid]  DEFAULT (newid()),
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100) NULL,
	[Phone] [nvarchar](50) NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Location]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Location](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Location_Guid]  DEFAULT (newid()),
	[Name] [nvarchar](100) NOT NULL,
 CONSTRAINT [PK_Location] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Service]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Service](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Service_Guid]  DEFAULT (newid()),
	[Name] [nvarchar](100) NOT NULL,
	[Description] [nvarchar](500) NULL,
	[Duration] [smallint] NULL,
	[StudentCapacity] [tinyint] NULL,
	[IsOnlineBookable] [bit] NULL,
	[SessionCount] [tinyint] NOT NULL CONSTRAINT [DF_Service_SessionCount]  DEFAULT ((1)),
	[RepeatFrequency] [char](1) NULL,
	[SessionPrice] [decimal](19, 4) NULL,
	[CoursePrice] [decimal](19, 4) NULL,
	[Colour] [char](12) NOT NULL CONSTRAINT [DF_Service_Colour]  DEFAULT ('Green'),
 CONSTRAINT [PK_Service] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Session]    Script Date: 24/03/2015 9:38:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Session](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[BusinessId] [int] NOT NULL,
	[Guid] [uniqueidentifier] NOT NULL CONSTRAINT [DF_Session_Guid]  DEFAULT (newid()),
	[ParentId] [int] NULL,
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
 CONSTRAINT [PK_Session] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Index [UX_Business_Guid]    Script Date: 24/03/2015 9:38:15 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Business_Guid] ON [dbo].[Business]
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UX_Coach_Guid]    Script Date: 24/03/2015 9:38:15 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Coach_Guid] ON [dbo].[Coach]
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UX_Customer_Guid]    Script Date: 24/03/2015 9:38:15 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Customer_Guid] ON [dbo].[Customer]
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UX_Location_Guid]    Script Date: 24/03/2015 9:38:15 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Location_Guid] ON [dbo].[Location]
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UX_Service_Guid]    Script Date: 24/03/2015 9:38:15 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Service_Guid] ON [dbo].[Service]
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [UX_Session_Guid]    Script Date: 24/03/2015 9:38:15 AM ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_Session_Guid] ON [dbo].[Session]
(
	[Guid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[BusinessUser] ADD  CONSTRAINT [DF_BusinessUser_Guid]  DEFAULT (newid()) FOR [Guid]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Customer] FOREIGN KEY([CustomerId])
REFERENCES [dbo].[Customer] ([Id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Customer]
GO
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Session] FOREIGN KEY([SessionId])
REFERENCES [dbo].[Session] ([Id])
GO
ALTER TABLE [dbo].[Booking] CHECK CONSTRAINT [FK_Booking_Session]
GO
ALTER TABLE [dbo].[Coach]  WITH CHECK ADD  CONSTRAINT [FK_Coach_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[Coach] CHECK CONSTRAINT [FK_Coach_Business]
GO
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD  CONSTRAINT [FK_Customer_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[Customer] CHECK CONSTRAINT [FK_Customer_Business]
GO
ALTER TABLE [dbo].[Location]  WITH CHECK ADD  CONSTRAINT [FK_Location_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[Location] CHECK CONSTRAINT [FK_Location_Business]
GO
ALTER TABLE [dbo].[Service]  WITH CHECK ADD  CONSTRAINT [FK_Service_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[Service] CHECK CONSTRAINT [FK_Service_Business]
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_Business]
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Coach] FOREIGN KEY([CoachId])
REFERENCES [dbo].[Coach] ([Id])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_Coach]
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Location] FOREIGN KEY([LocationId])
REFERENCES [dbo].[Location] ([Id])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_Location]
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Service] FOREIGN KEY([ServiceId])
REFERENCES [dbo].[Service] ([Id])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_Service]
GO
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Session] FOREIGN KEY([ParentId])
REFERENCES [dbo].[Session] ([Id])
GO
ALTER TABLE [dbo].[Session] CHECK CONSTRAINT [FK_Session_Session]
GO
USE [master]
GO
ALTER DATABASE [CoachseekLocal-Test] SET  READ_WRITE 
GO
