
CREATE PROCEDURE [dbo].[Booking_GetSessionBookingByGuid]
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @count int;

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
			bk2.[Id], 
			bk2.[ParentId]
		FROM
			[Booking] bk2
			INNER JOIN CourseBooking 
				ON CourseBooking.[Id] = bk2.[ParentId] 
	)

	SELECT
		@count = COUNT(*)
	FROM
		CourseBooking

	IF @count > 1
	BEGIN
		-- It's a Course. Return no records.
		SELECT
			b.[Guid] AS BusinessGuid,
			bk.[Guid],
			bk2.[Guid] AS ParentGuid,
			s.[Guid] AS SessionGuid,
			svc.[Name] + ' at ' + l.[Name] 
					   + ' with ' +  co.[FirstName] + ' ' + co.[LastName] 
					   + ' on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
					   + ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) AS SessionName,
			c.[Guid] AS CustomerGuid,
			c.[FirstName] + ' ' + c.[LastName] As CustomerName,
			bk.[PaymentStatus],
			bk.[HasAttended],
			bk.[IsOnlineBooking]
		FROM
			[dbo].[Business] b
			INNER JOIN [dbo].[Booking] bk
				ON b.[Id] = bk.[BusinessId]
			LEFT JOIN [dbo].[Booking] bk2
				ON bk.[ParentId] = bk2.[Id]
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
			1 = 0
	END
	ELSE
	BEGIN
		-- It's a Session.
		SELECT
			b.[Guid] AS BusinessGuid,
			bk.[Guid],
			bk2.[Guid] AS ParentGuid,
			s.[Guid] AS SessionGuid,
			svc.[Name] + ' at ' + l.[Name] 
					   + ' with ' +  co.[FirstName] + ' ' + co.[LastName] 
					   + ' on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
					   + ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) AS SessionName,
			c.[Guid] AS CustomerGuid,
			c.[FirstName] + ' ' + c.[LastName] As CustomerName,
			bk.[PaymentStatus],
			bk.[HasAttended],
			bk.[IsOnlineBooking]
		FROM
			[dbo].[Business] b
			INNER JOIN [dbo].[Booking] bk
				ON b.[Id] = bk.[BusinessId]
			LEFT JOIN [dbo].[Booking] bk2
				ON bk.[ParentId] = bk2.[Id]
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

END