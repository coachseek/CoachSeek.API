
CREATE PROCEDURE [dbo].[Location_Create]	
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

	INSERT INTO [dbo].[Location]
	(
		[BusinessId],
		[Guid],
		[Name]
	)
	VALUES
	(
		@businessId,
		@locationGuid,
		@name
	)

END


