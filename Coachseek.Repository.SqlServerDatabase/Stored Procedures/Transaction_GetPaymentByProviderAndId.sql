
CREATE PROCEDURE [dbo].[Transaction_GetPaymentByProviderAndId]
	@paymentProvider nvarchar(50),
	@id nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[Id],
		[PaymentProvider],
		[Type],
		[Status],
		[TransactionDate],
		[ProcessedDate],
		[PayerFirstName],
		[PayerLastName],
		[PayerEmail],
		[MerchantId],
		[MerchantName],
		[MerchantEmail],
		[ItemId],
		[ItemName],
		[ItemCurrency],
		[ItemAmount],
		[IsTesting]
	FROM
		[dbo].[Transaction]
	WHERE
		[PaymentProvider] = @paymentProvider
		AND [Id] = @id
END