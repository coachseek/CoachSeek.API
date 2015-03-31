
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