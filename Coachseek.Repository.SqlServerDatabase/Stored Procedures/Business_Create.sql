﻿
CREATE PROCEDURE [dbo].[Business_Create](
	@guid [uniqueidentifier],
	@name [nvarchar](100),
	@domain [nvarchar](100))
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO [dbo].[Business]
	(
		[Guid],
		[Name],
		[Domain]
	)
	VALUES
	(
		@guid,
		@name,
		@domain
	)

	SELECT
		[Id],
		[Guid],
		[Name],
		[Domain]
	FROM 
		[dbo].[Business]
	WHERE
		[Id] = SCOPE_IDENTITY()

END

