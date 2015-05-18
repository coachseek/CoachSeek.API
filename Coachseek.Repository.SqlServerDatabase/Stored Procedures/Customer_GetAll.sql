

CREATE PROCEDURE [dbo].[Customer_GetAll]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		c.[Id],
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone],
		c.[IsEmailUnsubscribed]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Customer] c
			ON b.Id = c.BusinessId
	WHERE
		b.[Guid] = @businessGuid
	ORDER BY
		c.[LastName], 
		c.[FirstName]

END


