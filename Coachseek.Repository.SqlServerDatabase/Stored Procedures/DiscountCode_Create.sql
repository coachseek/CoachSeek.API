
CREATE PROCEDURE [dbo].[DiscountCode_Create]
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

	INSERT INTO [dbo].[DiscountCode]
	(
		[BusinessId],
		[Guid],
		[Code],
		[DiscountPercent],
		[IsActive]
	)
	VALUES
	(
		@businessId,
		@discountCodeGuid,
		@code,
		@discountPercent,
		@isActive
	)

END