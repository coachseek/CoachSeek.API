
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
			bk2.[Id], 
			bk2.[ParentId]
		FROM
			[Booking] bk2
			INNER JOIN CourseBooking 
				ON CourseBooking.[Id] = bk2.[ParentId] 
	)

	--select * from CourseBooking

	SELECT
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
		c.[FirstName] + ' ' + c.[LastName] As CustomerName,
		bk.PaymentStatus,
		bk.HasAttended
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