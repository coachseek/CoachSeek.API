

CREATE PROCEDURE [dbo].[Session_GetCourseByGuid]
	@businessGuid uniqueidentifier,
	@courseGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	WITH Course AS 
	( 
		SELECT 
			s.[Id], 
			s.[ParentId]
		FROM 
			[dbo].[Session] s
			INNER JOIN [dbo].[Business] b
				ON b.[Id] = s.[BusinessId] 
		WHERE
			s.[Guid] = @courseGuid
			AND b.[Guid] = @businessGuid
		UNION ALL 
		SELECT
			s2.[Id], 
			s2.[ParentId]
		FROM
			[Session] s2
			INNER JOIN Course 
				ON Course.[Id] = s2.[ParentId] 
	)

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
		Course crs
		INNER JOIN [dbo].[Session] s
			ON s.[Id] = crs.[Id]
		INNER JOIN [dbo].[Business] b
			ON b.[Id] = s.[BusinessId]
		LEFT JOIN [dbo].[Session] s2
			ON s.[ParentId] = s2.[Id]
		LEFT JOIN [dbo].[Location] l
			ON l.[Id] = s.[LocationId]
		LEFT JOIN [dbo].[Coach] c
			ON c.[Id] = s.[CoachId]
		LEFT JOIN [dbo].[Service] svc
			ON svc.[Id] = s.[ServiceId]
	ORDER BY
		s2.[Guid]

END


