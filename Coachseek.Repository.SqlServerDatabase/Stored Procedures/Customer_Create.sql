

CREATE PROCEDURE [dbo].[Customer_Create]	
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

	INSERT INTO [dbo].[Customer]
	(
		[BusinessId],
		[Guid],
		[FirstName],
		[LastName],
		[Email],
		[Phone]
	)
	VALUES
	(
		@businessId,
		@customerGuid,
		@firstName,
		@lastName,
		@email,
		@phone
	)

END



