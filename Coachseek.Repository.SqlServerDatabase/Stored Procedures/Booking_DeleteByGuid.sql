
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

