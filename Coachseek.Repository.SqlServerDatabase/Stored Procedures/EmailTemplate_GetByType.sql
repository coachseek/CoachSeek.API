
CREATE PROCEDURE [dbo].[EmailTemplate_GetByType]
	@businessGuid uniqueidentifier,
	@templateType nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		b.[Guid] AS BusinessGuid,
		et.[Type],
		et.[Subject],
		et.[Body]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[EmailTemplate] et
			ON b.Id = et.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND et.[Type] = @templateType
END