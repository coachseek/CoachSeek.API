
CREATE PROCEDURE [dbo].[Business_GetByGuid]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	DECLARE @totalSessions int
	SELECT
		@totalSessions = COUNT(*)
	FROM
		[dbo].[Session]
	WHERE
		[BusinessId] = @businessId
		AND [SessionCount] = 1

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
		[Subscription],
		@totalSessions AS TotalNumberOfSessions
	FROM
		[dbo].[Business]
	WHERE
		[Id] = @businessId
END