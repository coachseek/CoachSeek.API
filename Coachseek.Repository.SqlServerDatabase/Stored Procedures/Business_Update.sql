
CREATE PROCEDURE [dbo].[Business_Update]	
	@guid uniqueidentifier,
	@name [nvarchar](100),
	@currency [nchar](3),
	@isOnlinePaymentEnabled [bit],
	@forceOnlinePayment [bit],
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
		[IsOnlinePaymentEnabled] = @isOnlinePaymentEnabled,
		[ForceOnlinePayment] = @forceOnlinePayment,
		[PaymentProvider] = @paymentProvider,
		[MerchantAccountIdentifier] = @merchantAccountIdentifier
	WHERE 
		[Guid] = @guid


	SELECT
		[Guid],
		[Name],
		[Domain],
		[Currency],
		[IsOnlinePaymentEnabled],
		[ForceOnlinePayment],
		[PaymentProvider],
		[MerchantAccountIdentifier]
	FROM 
		[dbo].[Business]
	WHERE 
		[Guid] = @guid

END