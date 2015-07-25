
CREATE PROCEDURE [dbo].[Service_GetByGuid]
	@businessGuid uniqueidentifier,
	@serviceGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		b.[Guid] AS BusinessGuid,
		s.[Guid],
		s.[Name],
		s.[Description],
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
		INNER JOIN [dbo].[Service] s
			ON b.Id = s.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND s.[Guid] = @serviceGuid

END


