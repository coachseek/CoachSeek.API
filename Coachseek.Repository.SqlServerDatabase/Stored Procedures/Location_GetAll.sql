
CREATE PROCEDURE [dbo].[Location_GetAll]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
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
		b.[Guid] = @businessGuid
END


