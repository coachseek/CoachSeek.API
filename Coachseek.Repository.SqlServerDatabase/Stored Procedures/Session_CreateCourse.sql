

CREATE PROCEDURE [dbo].[Session_CreateCourse]
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
	@sessionCount [tinyint],
	@repeatFrequency [char](1),
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
		[Colour]
	)
	VALUES
	(
		@businessId,
		@courseGuid,
		NULL,
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
		@colour
	)

	SELECT
		s.[Id],
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		l.[Guid] AS LocationGuid,
		l.[Name] AS LocationName,
		c.[Guid] AS CoachGuid,
		c.[FirstName] AS CoachFirstName,
		c.[LastName] AS CoachLastName,
		svc.[Guid] AS ServiceGuid,
		svc.[Name] AS ServiceName,
		s.[Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour]
	FROM 
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.Id = s.BusinessId
		LEFT JOIN [dbo].[Session] s2
			ON s2.Id = s.ParentId
		LEFT JOIN [dbo].[Location] l
			ON l.Id = s.LocationId
		LEFT JOIN [dbo].[Coach] c
			ON c.Id = s.CoachId
		LEFT JOIN [dbo].[Service] svc
			ON svc.Id = s.ServiceId
	WHERE
		s.[Id] = SCOPE_IDENTITY()

END



