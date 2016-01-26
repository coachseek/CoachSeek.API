
CREATE PROCEDURE [dbo].[CustomFieldTemplate_UpdateIsActive]
	@businessGuid uniqueidentifier,
	@templateGuid uniqueidentifier,
	@isActive [bit]
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
		[dbo].[CustomFieldTemplate]
	SET 
		[IsActive] = @isActive
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @templateGuid

END