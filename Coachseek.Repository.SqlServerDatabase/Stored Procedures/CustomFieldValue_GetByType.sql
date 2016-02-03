
CREATE PROCEDURE [dbo].[CustomFieldValue_GetByType]
	@businessGuid uniqueidentifier,
	@type nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	-- Type can only be customer at this stage.

	SELECT
		b.[Guid] AS BusinessGuid,
		cfv.[Type],
		c.[Guid],
		cfv.[Key],
		cfv.[Value]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[CustomFieldValue] cfv
			ON b.[Id] = cfv.[BusinessId]
		INNER JOIN [dbo].[Customer] c
			ON cfv.[TypeId] = c.[Id]
	WHERE
		b.[Guid] = @businessGuid
		AND cfv.[Type] = @type
END