
CREATE PROCEDURE [dbo].[Business_Create](
	@guid [uniqueidentifier],
	@name [nvarchar](100),
	@domain [nvarchar](100),
	@currency [nchar](3),
	@isOnlinePaymentEnabled [bit] = 0,
	@forceOnlinePayment [bit] = 0,
	@paymentProvider [nvarchar](50) = NULL,
	@merchantAccountIdentifier [nvarchar](100) = NULL	
	)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Business]
	(
		[Guid],
		[Name],
		[Domain],
		[Currency],
		[IsOnlinePaymentEnabled],
		[ForceOnlinePayment],
		[PaymentProvider],
		[MerchantAccountIdentifier]
	)
	VALUES
	(
		@guid,
		@name,
		@domain,
		@currency,
		@isOnlinePaymentEnabled,
		@forceOnlinePayment,
		@paymentProvider,
		@merchantAccountIdentifier	
	)

END


