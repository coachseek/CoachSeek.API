
CREATE PROCEDURE [dbo].[CustomFieldValue_Create]
	@businessGuid uniqueidentifier,
	@type [nvarchar](50),
	@typeGuid uniqueidentifier,
	@key [nvarchar](50),
	@value [nvarchar](MAX)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @typeId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	-- Type can only be customer at this stage.
	SELECT
		@typeId = Id
	FROM
		[dbo].[Customer]
	WHERE
		[Guid] = @typeGuid


	INSERT INTO [dbo].[CustomFieldValue]
	(
		[BusinessId],
		[Type],
		[TypeId],
		[Key],
        [Value]
	)
	VALUES
	(
		@businessId,
		@type,
		@typeId,
		@key,
		@value
	)

END