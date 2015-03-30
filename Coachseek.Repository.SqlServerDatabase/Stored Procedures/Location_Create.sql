
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

	SELECT
		l.[Id],
		b.[Guid] AS BusinessGuid,
		l.[Guid],
		l.[Name]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Location] l
			ON b.Id = l.BusinessId
	WHERE
		l.[Id] = SCOPE_IDENTITY()

END


