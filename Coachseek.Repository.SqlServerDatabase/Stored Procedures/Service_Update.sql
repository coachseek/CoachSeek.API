
CREATE PROCEDURE [dbo].[Service_Update]	
	@businessGuid uniqueidentifier,
	@serviceGuid uniqueidentifier,
	@name [nvarchar](100),
	@description [nvarchar](500) = NULL,
	@duration [smallint] = NULL,
	@studentCapacity [tinyint] = NULL,
	@isOnlineBookable [bit] = NULL,
	@sessionCount [tinyint] = NULL,
	@repeatFrequency [char](1) = NULL,
	@sessionPrice [decimal](19, 4) = NULL,
	@coursePrice [decimal](19, 4) = NULL,
	@colour [char](12)
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
		[dbo].[Service]
	SET 
		[Name] = @name,
		[Description] = @description,
		[Duration] = @duration,
		[StudentCapacity] = @studentCapacity,
		[IsOnlineBookable] = @isOnlineBookable,
		[SessionCount] = @sessionCount,
		[RepeatFrequency] = @repeatFrequency,
		[SessionPrice] = @sessionPrice,
		[CoursePrice] = @coursePrice,
		[Colour] = @colour
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @serviceGuid

	EXEC [dbo].[Service_GetByGuid] @businessGuid, @serviceGuid

END


