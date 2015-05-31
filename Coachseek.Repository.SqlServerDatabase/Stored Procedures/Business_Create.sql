
CREATE PROCEDURE [dbo].[Business_Create](
	@guid [uniqueidentifier],
	@name [nvarchar](100),
	@domain [nvarchar](100),
	@currency [nchar](3))
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Business]
	(
		[Guid],
		[Name],
		[Domain],
		[Currency]
	)
	VALUES
	(
		@guid,
		@name,
		@domain,
		@currency
	)

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
		[Id] = SCOPE_IDENTITY()

END


