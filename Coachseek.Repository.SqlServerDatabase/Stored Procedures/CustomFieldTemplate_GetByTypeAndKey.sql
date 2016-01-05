
CREATE PROCEDURE [dbo].[CustomFieldTemplate_GetByTypeAndKey]
	@businessGuid uniqueidentifier,
	@type [nvarchar](50) = NULL,
	@key [nvarchar](50) = NULL
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		b.[Guid] AS BusinessGuid,
		cft.[Type],
		cft.[Key],
		cft.[Name],
		cft.[IsRequired]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[CustomFieldTemplate] cft
			ON b.Id = cft.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND (@type IS NULL OR cft.[Type] = @type)
		AND (@key IS NULL OR cft.[Key] = @key)
END