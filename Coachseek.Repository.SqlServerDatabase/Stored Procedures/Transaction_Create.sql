
CREATE PROCEDURE [dbo].[Transaction_Create]	
	@id nvarchar(50),
	@paymentProvider nvarchar(50),
	@type nvarchar(50),
	@status nvarchar(50),
	@transactionDate datetime2(7),
	@processedDate datetime2(7),
	@payerFirstName nvarchar(50),
	@payerLastName nvarchar(50),
	@payerEmail nvarchar(100),
	@merchantId uniqueidentifier,
	@merchantName nvarchar(50),
	@merchantEmail nvarchar(100),
	@itemId uniqueidentifier,
	@itemName nvarchar(200),
	@itemCurrency nchar(3),
	@itemAmount decimal(19, 4),
	@isTesting bit,
	@originalMessage nvarchar(MAX)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Transaction]
	(
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
		[IsTesting],
		[OriginalMessage]
	)
	VALUES
	(
		@id,
		@paymentProvider,
		@type,
		@status,
		@transactionDate,
		@processedDate,
		@payerFirstName,
		@payerLastName,
		@payerEmail,
		@merchantId,
		@merchantName,
		@merchantEmail,
		@itemId,
		@itemName,
		@itemCurrency,
		@itemAmount,
		@isTesting,
		@originalMessage
	)

END