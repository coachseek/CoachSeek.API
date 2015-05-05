
CREATE PROCEDURE [dbo].[Business_GetByGuid]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		*
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid
END