

CREATE PROCEDURE [dbo].[Session_GetAllCourses]
	@businessGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s2.[Guid] AS ParentGuid,
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
			ON b.[Id] = s.[BusinessId]
		LEFT JOIN [dbo].[Session] s2
			ON s.[ParentId] = s2.[Id]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] c
			ON c.[Id] = s.[CoachId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
	WHERE
		b.[Guid] = @businessGuid
		AND s.[ParentId] IS NULL
		AND s.[SessionCount] > 1

END


