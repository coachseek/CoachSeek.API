
CREATE PROCEDURE [dbo].[CustomFieldTemplate_DeleteByTypeAndKey]
	@businessGuid uniqueidentifier,
	@type [nvarchar](50),
	@key [nvarchar](50)
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE
		cft
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[CustomFieldTemplate] cft
			ON b.Id = cft.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND cft.[Type] = @type
		AND cft.[Key] = @key
END