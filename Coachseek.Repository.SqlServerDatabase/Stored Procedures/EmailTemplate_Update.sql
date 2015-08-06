
CREATE PROCEDURE [dbo].[EmailTemplate_Update]
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

	UPDATE
		[dbo].[EmailTemplate]
	SET 
		[Subject] = @subject,
		[Body] = @body
	WHERE 
		[BusinessId] = @businessId
		AND [Type] = @type

END