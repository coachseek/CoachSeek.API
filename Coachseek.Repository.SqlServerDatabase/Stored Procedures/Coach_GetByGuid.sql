
CREATE PROCEDURE [dbo].[Coach_GetByGuid]
	@businessGuid uniqueidentifier,
	@coachGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT
		b.[Guid] AS BusinessGuid,
		c.[Guid],
		c.[FirstName],
		c.[LastName],
		c.[Email],
		c.[Phone],
		c.[MondayIsAvailable],
		c.[MondayStartTime],
		c.[MondayFinishTime],
		c.[TuesdayIsAvailable],
		c.[TuesdayStartTime],
		c.[TuesdayFinishTime],
		c.[WednesdayIsAvailable],
		c.[WednesdayStartTime],
		c.[WednesdayFinishTime],
		c.[ThursdayIsAvailable],
		c.[ThursdayStartTime],
		c.[ThursdayFinishTime],
		c.[FridayIsAvailable],
		c.[FridayStartTime],
		c.[FridayFinishTime],
		c.[SaturdayIsAvailable],
		c.[SaturdayStartTime],
		c.[SaturdayFinishTime],
		c.[SundayIsAvailable],
		c.[SundayStartTime],
		c.[SundayFinishTime]
	FROM
		[dbo].[Business] b
		INNER JOIN [dbo].[Coach] c
			ON b.Id = c.BusinessId
	WHERE
		b.[Guid] = @businessGuid
		AND c.[Guid] = @coachGuid
END


