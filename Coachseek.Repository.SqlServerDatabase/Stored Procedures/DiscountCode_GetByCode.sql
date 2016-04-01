
CREATE PROCEDURE [dbo].[DiscountCode_GetByCode]
	@businessGuid uniqueidentifier,
	@code nchar(10)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		b.[Guid] AS BusinessGuid,
		dc.[Guid],
		dc.[Code],
		dc.[DiscountPercent],
		dc.[IsActive]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[DiscountCode] dc
			ON b.Id = dc.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND dc.[Code] = @code
END