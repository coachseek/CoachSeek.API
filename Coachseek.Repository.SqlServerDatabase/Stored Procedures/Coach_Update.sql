
CREATE PROCEDURE [dbo].[Coach_Update]	
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

	UPDATE
		[dbo].[Coach]
	SET 
		[FirstName] = @firstName,
		[LastName] = @lastName,
		[Email] = @email,
		[Phone] = @phone,
		[MondayIsAvailable] = @mondayIsAvailable,
		[MondayStartTime] = @mondayStartTime,
		[MondayFinishTime] = @mondayFinishTime,
		[TuesdayIsAvailable] = @tuesdayIsAvailable,
		[TuesdayStartTime] = @tuesdayStartTime,
		[TuesdayFinishTime] = @tuesdayFinishTime,
		[WednesdayIsAvailable] = @wednesdayIsAvailable,
		[WednesdayStartTime] = @wednesdayStartTime,
		[WednesdayFinishTime] = @wednesdayFinishTime,
		[ThursdayIsAvailable] = @thursdayIsAvailable,
		[ThursdayStartTime] = @thursdayStartTime,
		[ThursdayFinishTime] = @thursdayFinishTime,
		[FridayIsAvailable] = @fridayIsAvailable,
		[FridayStartTime] = @fridayStartTime,
		[FridayFinishTime] = @fridayFinishTime,
		[SaturdayIsAvailable] = @saturdayIsAvailable,
		[SaturdayStartTime] = @saturdayStartTime,
		[SaturdayFinishTime] = @saturdayFinishTime,
		[SundayIsAvailable] = @sundayIsAvailable,
		[SundayStartTime] = @sundayStartTime,
		[SundayFinishTime] = @sundayFinishTime
	WHERE 
		[BusinessId] = @businessId
		AND [Guid] = @coachGuid

	EXEC [dbo].[Coach_GetByGuid] @businessGuid, @coachGuid

END


