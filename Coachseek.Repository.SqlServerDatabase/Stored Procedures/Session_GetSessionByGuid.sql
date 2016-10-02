

CREATE PROCEDURE [dbo].[Session_GetSessionByGuid]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier
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
		svc.[Name] + ' at ' + l.[Name] 
				   + ' with ' +  c.[FirstName] + ' ' + c.[LastName] 
				   + CASE 
						WHEN s.SessionCount = 1 THEN
							  ' on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
							+ ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) 
						ELSE
						      ' starting on ' + CONVERT(NVARCHAR(24), s.[StartDate]) 
						    + ' at ' + CONVERT(NVARCHAR(5), s.[StartTime], 108) 
						    + ' for ' + CONVERT(NVARCHAR(4), s.[SessionCount])
						    + CASE s.RepeatFrequency
								WHEN 'd' THEN ' days'
								WHEN 'w' THEN ' weeks'
							 END 
						END AS [Name],
		s.[StartDate],
		s.[StartTime],
		s.[Duration],
		s.[StudentCapacity],
		s.[IsOnlineBookable],
		s.[SessionCount],
		s.[RepeatFrequency],
		s.[SessionPrice],
		s.[CoursePrice],
		s.[Colour],
		s.[BookUntil]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Session] s
			ON b.Id = s.BusinessId
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
		AND s.[Guid] = @sessionGuid
		AND s.[SessionCount] = 1
END


