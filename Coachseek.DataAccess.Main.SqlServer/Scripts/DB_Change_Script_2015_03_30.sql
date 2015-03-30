

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[Booking_CreateCourseBooking]	
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier,
	@courseGuid uniqueidentifier,
	@customerGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @courseId int
	DECLARE @customerId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	SELECT
		@courseId = Id
	FROM
		[dbo].[Session]
	WHERE
		[Guid] = @courseGuid

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
		[CustomerId],
		[ParentId]
	)
	VALUES
	(
		@businessId,
		@bookingGuid,
		@courseId,
		@customerId,
		NULL
	)
END
GO


CREATE PROCEDURE [dbo].[Booking_CreateSessionBooking]	
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier,
	@parentGuid uniqueidentifier = NULL,
	@sessionGuid uniqueidentifier,
	@customerGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @parentId int
	DECLARE @sessionId int
	DECLARE @customerId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	SELECT
		@parentId = Id
	FROM
		[dbo].[Booking]
	WHERE
		[Guid] = @parentGuid

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
		[CustomerId],
		[ParentId]
	)
	VALUES
	(
		@businessId,
		@bookingGuid,
		@sessionId,
		@customerId,
		@parentId
	)
	
	SELECT
		bk.[Id],
		b.[Guid] AS BusinessGuid,
		bk.[Guid],
		bk2.[Guid] AS ParentGuid,
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
		LEFT JOIN [dbo].[Booking] bk2
			ON bk2.[Id] = bk.[ParentId]
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


CREATE PROCEDURE [dbo].[Booking_GetCourseBookingByGuid]
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	WITH CourseBooking AS 
	( 
		SELECT 
			bk.[Id], 
			bk.[ParentId]
		FROM 
			[dbo].[Booking] bk
			INNER JOIN [dbo].[Business] b
				ON b.[Id] = bk.[BusinessId] 
		WHERE
			bk.[Guid] = @bookingGuid
			AND b.[Guid] = @businessGuid
		UNION ALL 
		SELECT
			bk2.[ID], 
			bk2.[ParentId]
		FROM
			[Booking] bk2
			INNER JOIN CourseBooking 
				ON CourseBooking.[ID] = bk2.[ParentId] 
	)

	SELECT
		bk.[Id],
		b.[Guid] AS BusinessGuid,
		bk.[Guid],
		bk2.[Guid] AS ParentGuid,
		s.[Guid] AS SessionGuid,
		svc.[Name] + ' at ' + l.[Name] 
				   + ' with ' +  co.[FirstName] + ' ' + co.[LastName] 
				   + CASE 
						WHEN s.SessionCount = 1 THEN
							  ' on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
							+ ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) 
						ELSE
						      ' starting on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
						    + ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) 
						    + ' for ' + CONVERT(NVARCHAR(4), s.[SessionCount])
						    + CASE s.RepeatFrequency
								WHEN 'd' THEN ' days'
								WHEN 'w' THEN ' weeks'
							 END 
						END AS SessionName,
		c.[Guid] AS CustomerGuid,
		c.[FirstName] + ' ' + c.[LastName] As CustomerName
	FROM
		CourseBooking cbk
		INNER JOIN [dbo].[Booking] bk
			ON bk.[Id] = cbk.[Id]
		LEFT JOIN [dbo].[Business] b
			ON b.[Id] = bk.[BusinessId]
		LEFT JOIN [dbo].[Booking] bk2
			ON bk2.[Id] = cbk.[ParentId]
		LEFT JOIN [dbo].[Session] s
			ON s.[Id] = bk.[SessionId]
		LEFT JOIN [dbo].[Customer] c
			ON c.[Id] = bk.[CustomerId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] co
			ON co.[Id] = s.[CoachId]
	ORDER BY
		bk.[Id]
END
GO


CREATE PROCEDURE [dbo].[Booking_GetCustomerBookingsByCourseId]
	@businessGuid uniqueidentifier,
	@courseGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	WITH Course AS 
	( 
		SELECT 
			s.[Id], 
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
			s2.[Id], 
			s2.[ParentId]
		FROM
			[Session] s2
			INNER JOIN Course 
				ON Course.[ID] = s2.[ParentId] 
	)

	SELECT
		b.[Guid] AS BusinessGuid,
		s.[Guid] AS SessionGuid,
		c.[Guid] AS CustomerGuid,
		bk.[Guid] AS BookingGuid,
		bk2.[Guid] AS ParentBookingGuid,
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone]
	FROM
		Course crs
		INNER JOIN [dbo].[Booking] bk
			ON crs.[Id] = bk.[SessionId]
		LEFT JOIN [dbo].[Booking] bk2
			ON bk.[ParentId] = bk2.[Id]
		LEFT JOIN [dbo].[Business] b
			ON b.[Id] = bk.[BusinessId]
		LEFT JOIN [dbo].[Session] s
			ON bk.[SessionId] = s.[Id]
		LEFT JOIN [dbo].[Customer] c
			ON bk.[CustomerId] = c.[Id]
END
GO


CREATE PROCEDURE [dbo].[Booking_GetCustomerBookingsBySessionId]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		b.[Guid] AS BusinessGuid,
		s.[Guid] AS SessionGuid,
		c.[Guid] AS CustomerGuid,
		bk.[Guid] AS BookingGuid,
		bk2.[Guid] AS ParentBookingGuid,
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Booking] bk
			ON b.Id = bk.BusinessId
		LEFT JOIN [dbo].[Booking] bk2
			ON bk.[ParentId] = bk2.[Id]
		LEFT JOIN [dbo].[Session] s
			ON bk.[SessionId] = s.[Id]
		LEFT JOIN [dbo].[Customer] c
			ON bk.[CustomerId] = c.[Id]
	WHERE
		b.[Guid] = @businessGuid
		AND s.[Guid] = @sessionGuid
END
GO


-- Drop old stored procs
DROP PROCEDURE [dbo].[Booking_Create]
GO

DROP PROCEDURE [dbo].[Booking_GetByGuid]
GO

DROP PROCEDURE [dbo].[Customer_GetCustomerBookingsBySessionId]
GO


