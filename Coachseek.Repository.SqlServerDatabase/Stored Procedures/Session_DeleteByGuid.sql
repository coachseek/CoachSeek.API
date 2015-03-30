
CREATE PROCEDURE [dbo].[Session_DeleteByGuid]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier
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
			s.[Guid] = @sessionGuid
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
	
	DELETE 
		s
	FROM 
		[dbo].[Session] s
		INNER JOIN Course c
			ON s.[Id] = c.[Id]

END

