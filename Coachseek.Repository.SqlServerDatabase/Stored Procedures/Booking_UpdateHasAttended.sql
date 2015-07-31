
CREATE PROCEDURE [dbo].[Booking_UpdateHasAttended]	
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier,
	@hasAttended bit = NULL
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
		[HasAttended] = @hasAttended
	WHERE
		[BusinessId] = @businessId
		AND [Guid] = @bookingGuid
 
END