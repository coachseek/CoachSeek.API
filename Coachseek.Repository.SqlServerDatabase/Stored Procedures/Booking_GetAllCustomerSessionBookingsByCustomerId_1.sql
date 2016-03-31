
CREATE PROCEDURE [dbo].[Booking_GetAllCustomerSessionBookingsByCustomerId]
	@businessGuid uniqueidentifier,
	@customerGuid uniqueidentifier
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
		c.[Phone],
		bk.[PaymentStatus],
		bk.[HasAttended],
		bk.[IsOnlineBooking],
		bk.[DiscountPercent]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Booking] bk
			ON b.[Id] = bk.[BusinessId]
		LEFT JOIN [dbo].[Booking] bk2
			ON bk.[ParentId] = bk2.[Id]
		LEFT JOIN [dbo].[Session] s
			ON bk.[SessionId] = s.[Id]
		LEFT JOIN [dbo].[Customer] c
			ON bk.[CustomerId] = c.[Id]
	WHERE
		b.[Guid] = @businessGuid
		AND c.[Guid] = @customerGuid
		AND bk2.[Guid] IS NOT NULL
END