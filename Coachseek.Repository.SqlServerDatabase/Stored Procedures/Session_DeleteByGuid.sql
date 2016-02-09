

CREATE PROCEDURE [dbo].[Session_DeleteByGuid]
	@businessGuid uniqueidentifier,
	@sessionGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @parentId int

	SELECT
		@parentId = ParentId
	FROM
		[dbo].[Session] s
		INNER JOIN [dbo].[Business] b
			ON b.[Id] = s.[BusinessId] 
	WHERE
		b.[Guid] = @businessGuid
		AND s.[Guid] = @sessionGuid;

	WITH Course AS 
	( 
		SELECT 
			s.[ID], 
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
			s2.[ID], 
			s2.[ParentId]
		FROM
			[Session] s2
			INNER JOIN Course 
				ON Course.[ID] = s2.[ParentId] 
	)

	DELETE 
		s
	FROM 
		[dbo].[Session] s
		INNER JOIN Course c
			ON s.[Id] = c.[Id]

	IF @parentId IS NOT NULL -- it's a session
	BEGIN
		DECLARE @courseSessionCount int

		SELECT
			@courseSessionCount = COUNT(*)
		FROM
			[dbo].[Session] s
		WHERE
			s.[ParentId] = @parentId
		
		IF @courseSessionCount = 0 -- no sessions left in course
		BEGIN
			DELETE 
				s
			FROM 
				[dbo].[Session] s
			WHERE 
				s.[Id] = @parentId
		END
	END
END

