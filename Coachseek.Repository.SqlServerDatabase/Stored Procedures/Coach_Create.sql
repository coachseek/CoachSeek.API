

CREATE PROCEDURE [dbo].[Coach_Create]	
	@businessGuid uniqueidentifier,
	@coachGuid uniqueidentifier,
	@firstName [nvarchar](50),
	@lastName [nvarchar](50),
	@email [nvarchar](100),
	@phone [nvarchar](50),
	@mondayIsAvailable bit,
	@mondayStartTime [nchar](5) = NULL,
	@mondayFinishTime [nchar](5) = NULL,
	@tuesdayIsAvailable bit,
	@tuesdayStartTime [nchar](5) = NULL,
	@tuesdayFinishTime [nchar](5) = NULL,
	@wednesdayIsAvailable bit,
	@wednesdayStartTime [nchar](5) = NULL,
	@wednesdayFinishTime [nchar](5) = NULL,
	@thursdayIsAvailable bit,
	@thursdayStartTime [nchar](5) = NULL,
	@thursdayFinishTime [nchar](5) = NULL,
	@fridayIsAvailable bit,
	@fridayStartTime [nchar](5) = NULL,
	@fridayFinishTime [nchar](5) = NULL,
	@saturdayIsAvailable bit,
	@saturdayStartTime [nchar](5) = NULL,
	@saturdayFinishTime [nchar](5) = NULL,
	@sundayIsAvailable bit,
	@sundayStartTime [nchar](5) = NULL,
	@sundayFinishTime [nchar](5) = NULL
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	INSERT INTO [dbo].[Coach]
	(
		[BusinessId],
		[Guid],
		[FirstName],
		[LastName],
		[Email],
		[Phone],
		[MondayIsAvailable],
		[MondayStartTime],
		[MondayFinishTime],
		[TuesdayIsAvailable],
		[TuesdayStartTime],
		[TuesdayFinishTime],
		[WednesdayIsAvailable],
		[WednesdayStartTime],
		[WednesdayFinishTime],
		[ThursdayIsAvailable],
		[ThursdayStartTime],
		[ThursdayFinishTime],
		[FridayIsAvailable],
		[FridayStartTime],
		[FridayFinishTime],
		[SaturdayIsAvailable],
		[SaturdayStartTime],
		[SaturdayFinishTime],
		[SundayIsAvailable],
		[SundayStartTime],
		[SundayFinishTime]
	)
	VALUES
	(
		@businessId,
		@coachGuid,
		@firstName,
		@lastName,
		@email,
		@phone,
		@mondayIsAvailable,
		@mondayStartTime,
		@mondayFinishTime,
		@tuesdayIsAvailable,
		@tuesdayStartTime,
		@tuesdayFinishTime,
		@wednesdayIsAvailable,
		@wednesdayStartTime,
		@wednesdayFinishTime,
		@thursdayIsAvailable,
		@thursdayStartTime,
		@thursdayFinishTime,
		@fridayIsAvailable,
		@fridayStartTime,
		@fridayFinishTime,
		@saturdayIsAvailable,
		@saturdayStartTime,
		@saturdayFinishTime,
		@sundayIsAvailable,
		@sundayStartTime,
		@sundayFinishTime
	)

	SELECT
		c.[Id],
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
		c.[Id] = SCOPE_IDENTITY()

END



