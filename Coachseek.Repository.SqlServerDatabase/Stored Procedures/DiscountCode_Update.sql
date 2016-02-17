
CREATE PROCEDURE [dbo].[DiscountCode_Update]
	@businessGuid uniqueidentifier,
	@discountCodeGuid uniqueidentifier,
	@code [nchar](10),
	@discountPercent [int],
	@isActive [bit]
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

	UPDATE
		[dbo].[DiscountCode]
	SET 
		[Code] = @code,
		[DiscountPercent] = @discountPercent,
		[IsActive] = @isActive
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @discountCodeGuid

END