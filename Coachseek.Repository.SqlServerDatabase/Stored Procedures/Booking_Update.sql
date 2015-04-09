
CREATE PROCEDURE [dbo].[Booking_Update]	
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier,
	@paymentStatus nvarchar(50),
	@hasAttended bit
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
		[dbo].[Booking]
	SET 
		[PaymentStatus] = @paymentStatus,
		[HasAttended] = @hasAttended
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @bookingGuid

	--SELECT
	--	*
	--FROM 
	--	[dbo].[Booking]
	--WHERE
	--	[BusinessId] = @businessId
	--	AND [Guid] = @bookingGuid

	EXEC Booking_GetByGuid @businessGuid, @bookingGuid;

END


