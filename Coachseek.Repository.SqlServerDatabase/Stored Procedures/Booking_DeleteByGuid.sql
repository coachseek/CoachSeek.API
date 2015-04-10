
CREATE PROCEDURE [dbo].[Booking_DeleteByGuid]
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	WITH CourseBooking AS 
	( 
		SELECT 
			bk.[Id], 
			bk.[ParentId]
		FROM 
			[dbo].[Booking] bk
			INNER JOIN [dbo].[Business] b
				ON b.[Id] = bk.[BusinessId] 
		WHERE
			bk.[Guid] = @bookingGuid
			AND b.[Guid] = @businessGuid
		UNION ALL 
		SELECT
			bk2.[Id], 
			bk2.[ParentId]
		FROM
			[Booking] bk2
			INNER JOIN CourseBooking 
				ON CourseBooking.[Id] = bk2.[ParentId] 
	)
	
	DELETE 
		bk
	FROM 
		[dbo].[Booking] bk
		INNER JOIN CourseBooking cb
			ON bk.[Id] = cb.[Id]
	
END

