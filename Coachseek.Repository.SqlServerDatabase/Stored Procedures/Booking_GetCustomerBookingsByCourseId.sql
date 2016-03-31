
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
				ON Course.[Id] = s2.[ParentId] 
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
		c.[Phone],
		bk.[PaymentStatus],
		bk.[HasAttended],
		bk.[IsOnlineBooking],
		bk.[DiscountPercent]
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