
CREATE PROCEDURE [dbo].[CustomFieldTemplate_Update]
	@businessGuid uniqueidentifier,
	@templateGuid uniqueidentifier,
	@type [nvarchar](50),
	@key [nvarchar](50),
	@name [nvarchar](50),
	@isRequired [bit],
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
		[Type] = @type,
		[Key] = @key,
		[Name] = @name,
		[IsRequired] = @isRequired,
		[IsActive] = @isActive
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @templateGuid
END