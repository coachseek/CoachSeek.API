
CREATE PROCEDURE [dbo].[Business_Update]	
	@guid uniqueidentifier,
	@name [nvarchar](100),
	@currency [nchar](3),
	@paymentProvider [nvarchar](50) = NULL,
	@merchantAccountIdentifier [nvarchar](100) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE
		[dbo].[Business]
	SET 
		[Name] = @name,
		[Currency] = @currency,
		[PaymentProvider] = @paymentProvider,
		[MerchantAccountIdentifier] = @merchantAccountIdentifier
	WHERE 
		[Guid] = @guid


	SELECT
		[Id],
		[Guid],
		[Name],
		[Domain],
		[Currency],
		[PaymentProvider],
		[MerchantAccountIdentifier]
	FROM 
		[dbo].[Business]
	WHERE 
		[Guid] = @guid

END