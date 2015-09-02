
CREATE PROCEDURE [dbo].[Business_Create](
	@guid [uniqueidentifier],
	@name [nvarchar](100),
	@domain [nvarchar](100),
	@sport [nvarchar](100) = NULL,
	@currency [nchar](3),
	@isOnlinePaymentEnabled [bit] = 0,
	@forceOnlinePayment [bit] = 0,
	@paymentProvider [nvarchar](50) = NULL,
	@merchantAccountIdentifier [nvarchar](100) = NULL,
	@createdOn [datetime2] = NULL,
	@isTesting [bit] = 0
	)
AS
BEGIN
	SET NOCOUNT ON;

	IF @createdOn IS NULL
        SET @createdOn = GETUTCDATE()

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
		[Sport]
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
		@sport
	)

END


