
CREATE PROCEDURE [dbo].[Business_UpdateAuthorisedUntil]	
	@businessGuid uniqueidentifier,
	@authorisedUntil datetime2
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE
		[dbo].[Business]
	SET 
		[AuthorisedUntil] = @authorisedUntil
	WHERE
		[Guid] = @businessGuid
 
END