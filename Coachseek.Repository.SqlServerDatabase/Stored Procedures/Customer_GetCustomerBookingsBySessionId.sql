

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


