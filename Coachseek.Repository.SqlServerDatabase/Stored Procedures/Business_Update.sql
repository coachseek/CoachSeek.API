
CREATE PROCEDURE [dbo].[Business_Update]	
	@guid uniqueidentifier,
	@name [nvarchar](100),
	@currency [nchar](3)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE
		[dbo].[Business]
	SET 
		[Name] = @name,
		[Currency] = @currency
	WHERE 
		[Guid] = @guid


	SELECT
		[Id],
		[Guid],
		[Name],
		[Domain],
		[Currency]
	FROM 
		[dbo].[Business]
	WHERE 
		[Guid] = @guid

END