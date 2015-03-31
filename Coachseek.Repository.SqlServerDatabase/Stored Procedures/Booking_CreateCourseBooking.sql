
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
	
	--SELECT
	--	bk.[Id],
	--	b.[Guid] AS BusinessGuid,
	--	bk.[Guid],
	--	NULL,
	--	s.[Guid] AS CourseGuid,
	--	svc.[Name] + ' at ' + l.[Name] 
	--			   + ' with ' +  co.[FirstName] + ' ' + co.[LastName] 
	--			   + ' starting on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
	--			   + ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) 
	--			   + ' for ' + s.[SessionCount] 
	--			   + CASE s.RepeatFrequency
	--					WHEN 'd' THEN ' days'
	--					WHEN 'w' THEN ' weeks'
	--				 END AS CourseName,
	--	c.[Guid] AS CustomerGuid,
	--	c.[FirstName] + ' ' + c.[LastName] As CustomerName
	--FROM
	--	[dbo].[Business] b
	--	INNER JOIN [dbo].[Booking] bk
	--		ON b.[Id] = bk.[BusinessId]
	--	INNER JOIN [dbo].[Session] s
	--		ON s.[Id] = bk.[SessionId]
	--	INNER JOIN [dbo].[Customer] c
	--		ON c.[Id] = bk.[CustomerId]
	--	LEFT JOIN [dbo].[Service] svc
	--		ON svc.[Id] = s.[ServiceId]
	--	LEFT JOIN [dbo].[Location] l
	--		ON l.[Id] = s.[LocationId]
	--	LEFT JOIN [dbo].[Coach] co
	--		ON co.[Id] = s.[CoachId]
	--WHERE
	--	bk.[Id] = SCOPE_IDENTITY()

END