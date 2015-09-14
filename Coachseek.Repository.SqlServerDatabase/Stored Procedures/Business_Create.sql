
CREATE PROCEDURE [dbo].[Business_Create](
	@guid [uniqueidentifier],
	@name [nvarchar](100),
	@domain [nvarchar](100),
	@sport [nvarchar](100) = NULL,
	@currency [nchar](3),
	@isOnlinePaymentEnabled [bit],
	@forceOnlinePayment [bit],
	@paymentProvider [nvarchar](50) = NULL,
	@merchantAccountIdentifier [nvarchar](100) = NULL,
	@createdOn [datetime2],
	@authorisedUntil [datetime2],
	@isTesting [bit]
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
		[MerchantAccountIdentifier],
		[CreatedOn],
		[IsTesting],
		[Sport],
		[AuthorisedUntil]
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
		@merchantAccountIdentifier,
		@createdOn,
		@isTesting,
		@sport,
		@authorisedUntil
	)

END


