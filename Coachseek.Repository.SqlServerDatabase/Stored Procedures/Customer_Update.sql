﻿
CREATE PROCEDURE [dbo].[Customer_Update]	
	@businessGuid uniqueidentifier,
	@customerGuid uniqueidentifier,
	@firstName [nvarchar](50),
	@lastName [nvarchar](50),
	@email [nvarchar](100),
	@phone [nvarchar](50)
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
		*
	FROM 
		[dbo].[Customer]
	WHERE
		[BusinessId] = @businessId
		AND [Guid] = @customerGuid

END


