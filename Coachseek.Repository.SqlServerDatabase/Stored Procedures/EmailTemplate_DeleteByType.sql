
CREATE PROCEDURE [dbo].[EmailTemplate_DeleteByType]
	@businessGuid uniqueidentifier,
	@type nvarchar(50)
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
	
	DELETE FROM 
		[dbo].[EmailTemplate]
	WHERE
		[BusinessId] = @businessId
		AND [Type] = @type
	
END