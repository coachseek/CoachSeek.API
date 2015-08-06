

CREATE PROCEDURE [dbo].[EmailTemplate_Create]	
	@businessGuid uniqueidentifier,
	@type [nvarchar](50),
	@subject [nvarchar](200),
	@body [nvarchar](MAX) = NULL
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

	INSERT INTO [dbo].[EmailTemplate]
	(
		[BusinessId],
		[Type],
		[Subject],
		[Body]
	)
	VALUES
	(
		@businessId,
		@type,
		@subject,
		@body
	)

END