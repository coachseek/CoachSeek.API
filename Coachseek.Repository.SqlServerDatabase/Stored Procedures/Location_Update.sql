
CREATE PROCEDURE [dbo].[Location_Update]	
	@businessGuid uniqueidentifier,
	@locationGuid uniqueidentifier,
	@name [nvarchar](100)
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
		[dbo].[Location]
	SET 
		[Name] = @name
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @locationGuid

	SELECT
		*
	FROM 
		[dbo].[Location]
	WHERE
		[BusinessId] = @businessId
		AND [Guid] = @locationGuid

END


