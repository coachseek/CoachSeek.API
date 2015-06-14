
CREATE PROCEDURE [dbo].[Customer_Update]	
	@businessGuid uniqueidentifier,
	@customerGuid uniqueidentifier,
	@firstName [nvarchar](50),
	@lastName [nvarchar](50),
	@email [nvarchar](100) = NULL,
	@phone [nvarchar](50) = NULL
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

	UPDATE
		[dbo].[Customer]
	SET 
		[FirstName] = @firstName,
		[LastName] = @lastName,
		[Email] = @email,
		[Phone] = @phone
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @customerGuid

	SELECT
		c.[Id],
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Customer] c
			ON b.Id = c.BusinessId
	WHERE
		c.[BusinessId] = @businessId
		AND c.[Guid] = @customerGuid

END


