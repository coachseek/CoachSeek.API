﻿

CREATE PROCEDURE [dbo].[Service_Create]	
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

	INSERT INTO [dbo].[Service]
	(
		[BusinessId],
		[Guid],
		[Name],
		[Description],
		[Duration],
		[StudentCapacity],
		[IsOnlineBookable],
		[SessionCount],
		[RepeatFrequency],
		[SessionPrice],
		[CoursePrice],
		[Colour]
	)
	VALUES
	(
		@businessId,
		@serviceGuid,
		@name,
		@description,
		@duration,
		@studentCapacity,
		@isOnlineBookable,
		@sessionCount,
		@repeatFrequency,
		@sessionPrice,
		@coursePrice,
		@colour
	)

END



