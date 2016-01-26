
CREATE PROCEDURE [dbo].[CustomFieldTemplate_GetByGuid]
	@businessGuid uniqueidentifier,
	@templateGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		b.[Guid] AS BusinessGuid,
		cft.[Guid],
		cft.[Type],
		cft.[Key],
		cft.[Name],
		cft.[IsRequired],
		cft.[IsActive]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[CustomFieldTemplate] cft
			ON b.Id = cft.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND cft.[Guid] = @templateGuid
END