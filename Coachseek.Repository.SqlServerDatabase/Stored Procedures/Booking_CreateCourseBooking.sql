﻿
CREATE PROCEDURE [dbo].[Booking_CreateCourseBooking]	
	@businessGuid uniqueidentifier,
	@bookingGuid uniqueidentifier,
	@courseGuid uniqueidentifier,
	@customerGuid uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @businessId int
	DECLARE @courseId int
	DECLARE @customerId int

	SELECT
		@businessId = Id
	FROM
		[dbo].[Business]
	WHERE
		[Guid] = @businessGuid

	SELECT
		@courseId = Id
	FROM
		[dbo].[Session]
	WHERE
		[Guid] = @courseGuid

	SELECT
		@customerId = Id
	FROM
		[dbo].[Customer]
	WHERE
		[Guid] = @customerGuid

	INSERT INTO [dbo].[Booking]
	(
		[BusinessId],
		[Guid],
		[SessionId],
		[CustomerId],
		[ParentId],
		[PaymentStatus],
		[HasAttended]
	)
	VALUES
	(
		@businessId,
		@bookingGuid,
		@courseId,
		@customerId,
		NULL,
		NULL,
		NULL
	)
	
END