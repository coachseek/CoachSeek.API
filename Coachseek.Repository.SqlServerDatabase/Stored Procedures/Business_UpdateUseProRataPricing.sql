
CREATE PROCEDURE [dbo].[Business_UpdateUseProRataPricing]	
	@businessGuid uniqueidentifier,
	@useProRataPricing bit
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE
		[dbo].[Business]
	SET 
		[UseProRataPricing] = @useProRataPricing
	WHERE
		[Guid] = @businessGuid
 
END