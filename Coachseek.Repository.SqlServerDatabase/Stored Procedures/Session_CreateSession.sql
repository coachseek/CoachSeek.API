﻿
CREATE PROCEDURE [dbo].[Session_CreateSession]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier,
	@parentGuid uniqueidentifier = NULL,
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
	@colour [char](12),
	@bookUntil [tinyint]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @parentId int
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
		@parentId = Id
	FROM
		[dbo].[Session]
	WHERE
		[Guid] = @parentGuid

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

	INSERT INTO [dbo].[Session]
	(
		[BusinessId],
		[Guid],
		[ParentId],
		[LocationId],
		[CoachId],
		[ServiceId],
		[Name],
		[StartDate],
		[StartTime],
		[Duration],
		[StudentCapacity],
		[IsOnlineBookable],
		[SessionCount],
		[RepeatFrequency],
		[SessionPrice],
		[CoursePrice],
		[Colour],
		[BookUntil]
	)
	VALUES
	(
		@businessId,
		@sessionGuid,
		@parentId,
		@locationId,
		@coachId,
		@serviceId,
		@name,
		@startDate,
		@startTime,
		@duration,
		@studentCapacity,
		@isOnlineBookable,
		@sessionCount,
		@repeatFrequency,
		@sessionPrice,
		@coursePrice,
		@colour,
		@bookUntil
	)

END



