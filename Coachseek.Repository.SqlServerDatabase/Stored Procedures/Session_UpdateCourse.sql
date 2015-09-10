﻿
CREATE PROCEDURE [dbo].[Session_UpdateCourse]
	@businessGuid uniqueidentifier,
	@courseGuid uniqueidentifier,
	@locationGuid uniqueidentifier,
	@coachGuid uniqueidentifier,
	@serviceGuid uniqueidentifier,
	@name [nvarchar](100) = NULL,
	@startDate [date],
	@startTime [time](7),
	@duration [smallint],
	@studentCapacity [tinyint],
	@isOnlineBookable [bit],
	@sessionCount [tinyint] = NULL,
	@repeatFrequency [char](1) = NULL,
	@sessionPrice [decimal](19, 4) = NULL,
	@coursePrice [decimal](19, 4) = NULL,
	@colour [char](12)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @locationId int
	DECLARE @coachId int
	DECLARE @serviceId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	SELECT
		@locationId = Id
	FROM
		[dbo].[Location]
	WHERE
		[Guid] = @locationGuid

	SELECT
		@coachId = Id
	FROM
		[dbo].[Coach]
	WHERE
		[Guid] = @coachGuid

	SELECT
		@serviceId = Id
	FROM
		[dbo].[Service]
	WHERE
		[Guid] = @serviceGuid

	UPDATE
		[dbo].[Session]
	SET 
		[LocationId] = @locationId,
		[CoachId] = @coachId,
		[ServiceId] = @serviceId,
		[Name] = @name,
		[StartDate] = @startDate,
		[StartTime] = @startTime,
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
		AND [Guid] = @courseGuid

END


