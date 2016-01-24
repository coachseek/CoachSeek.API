﻿
CREATE PROCEDURE [dbo].[CustomFieldValue_GetByTypeGuid]
	@businessGuid uniqueidentifier,
	@type nvarchar(50),
	@typeGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @typeId int

	-- Type can only be customer at this stage.
	SELECT
		@typeId = Id
	FROM
		[dbo].[Customer]
	WHERE
		[Guid] = @typeGuid

	SELECT
		b.[Guid] AS BusinessGuid,
		cfv.[Type],
		@typeGuid,
		cfv.[Key],
		cfv.[Value]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[CustomFieldValue] cfv
			ON b.Id = cfv.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND cfv.[Type] = @type
		AND cfv.[TypeId] = @typeId
END