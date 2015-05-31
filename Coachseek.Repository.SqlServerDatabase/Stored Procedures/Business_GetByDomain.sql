
CREATE PROCEDURE [dbo].[Business_GetByDomain]
	@domain nvarchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		[Id],
		[Guid],
		[Name],
		[Domain],
		[Currency],
		[PaymentProvider],
		[MerchantAccountIdentifier]
	FROM
		[dbo].[Business]
	WHERE
		[Domain] = @domain
END


