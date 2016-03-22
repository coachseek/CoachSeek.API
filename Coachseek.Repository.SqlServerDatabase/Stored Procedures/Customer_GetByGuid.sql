
CREATE PROCEDURE [dbo].[Customer_GetByGuid]
	@businessGuid uniqueidentifier,
	@customerGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone],
		c.[Sex],
		c.[DateOfBirth]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Customer] c
			ON b.Id = c.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND c.[Guid] = @customerGuid
END


