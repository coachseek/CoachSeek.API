﻿
CREATE PROCEDURE [dbo].[Business_Update]	
	@guid uniqueidentifier,
	@name [nvarchar](100),
	@domain [nvarchar](100),
	@sport [nvarchar](100) = NULL,
	@currency [nchar](3),
	@isOnlinePaymentEnabled [bit],
	@forceOnlinePayment [bit],
	@paymentProvider [nvarchar](50) = NULL,
	@merchantAccountIdentifier [nvarchar](100) = NULL,
	@useProRataPricing [bit]
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE
		[dbo].[Business]
	SET 
		[Name] = @name,
		[Domain] = @domain,
		[Sport] = @sport,
		[Currency] = @currency,
		[IsOnlinePaymentEnabled] = @isOnlinePaymentEnabled,
		[ForceOnlinePayment] = @forceOnlinePayment,
		[PaymentProvider] = @paymentProvider,
		[MerchantAccountIdentifier] = @merchantAccountIdentifier,
		[UseProRataPricing] = @useProRataPricing
	WHERE 
		[Guid] = @guid

END