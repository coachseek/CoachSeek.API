

CREATE PROCEDURE [dbo].[EmailTemplate_GetAll]
	@businessGuid uniqueidentifier
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
	ORDER BY
		et.[Type]

END