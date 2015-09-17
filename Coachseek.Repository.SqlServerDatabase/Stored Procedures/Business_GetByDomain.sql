
CREATE PROCEDURE [dbo].[Business_GetByDomain]
	@domain nvarchar(100)
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
		[Domain] = @domain
END


