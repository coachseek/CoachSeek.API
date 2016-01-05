
CREATE PROCEDURE [dbo].[CustomFieldTemplate_Create]	
	@businessGuid uniqueidentifier,
	@type [nvarchar](50),
	@key [nvarchar](50),
	@name [nvarchar](50),
	@isRequired [bit]
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

	INSERT INTO [dbo].[CustomFieldTemplate]
	(
		[BusinessId],
		[Type],
		[Key],
		[Name],
		[IsRequired]
	)
	VALUES
	(
		@businessId,
		@type,
		@key,
		@name,
		@isRequired
	)

END