
CREATE PROCEDURE [dbo].[Business_GetByGuid]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[Id],
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
		[Guid] = @businessGuid
END