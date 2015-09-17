
CREATE PROCEDURE [dbo].[Business_GetByGuid]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[Guid],
		[Name],
		[Domain],
		[Sport],
		[Currency],
		[IsOnlinePaymentEnabled],
		[ForceOnlinePayment],
		[PaymentProvider],
		[MerchantAccountIdentifier],
		[AuthorisedUntil],
		[Subscription]
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid
END