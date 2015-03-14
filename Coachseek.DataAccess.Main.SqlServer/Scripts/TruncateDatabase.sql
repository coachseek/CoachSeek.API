
-- Drop Constraints
ALTER TABLE [dbo].[Location] DROP CONSTRAINT [FK_Location_Business]
ALTER TABLE [dbo].[Coach] DROP CONSTRAINT [FK_Coach_Business]
ALTER TABLE [dbo].[Service] DROP CONSTRAINT [FK_Service_Business]
ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [FK_Customer_Business]
ALTER TABLE [dbo].[Session] DROP CONSTRAINT [FK_Session_Business]
ALTER TABLE [dbo].[Session] DROP CONSTRAINT [FK_Session_Location]
ALTER TABLE [dbo].[Session] DROP CONSTRAINT [FK_Session_Coach]
ALTER TABLE [dbo].[Session] DROP CONSTRAINT [FK_Session_Service]
ALTER TABLE [dbo].[Session] DROP CONSTRAINT [FK_Session_Session]
ALTER TABLE [dbo].[Booking] DROP CONSTRAINT [FK_Booking_Customer]
ALTER TABLE [dbo].[Booking] DROP CONSTRAINT [FK_Booking_Session]
GO

-- Truncate Tables
TRUNCATE TABLE [dbo].[Location]
TRUNCATE TABLE [dbo].[Coach]
TRUNCATE TABLE [dbo].[Service]
TRUNCATE TABLE [dbo].[Customer]
TRUNCATE TABLE [dbo].[Session]
TRUNCATE TABLE [dbo].[Booking]
TRUNCATE TABLE [dbo].[Business]
GO

-- Recreate Constraints
ALTER TABLE [dbo].[Location]  WITH CHECK ADD CONSTRAINT [FK_Location_Business] FOREIGN KEY([BusinessId])
	REFERENCES [dbo].[Business] ([Id])
ALTER TABLE [dbo].[Coach]  WITH CHECK ADD CONSTRAINT [FK_Coach_Business] FOREIGN KEY([BusinessId])
	REFERENCES [dbo].[Business] ([Id])
ALTER TABLE [dbo].[Service]  WITH CHECK ADD CONSTRAINT [FK_Service_Business] FOREIGN KEY([BusinessId])
	REFERENCES [dbo].[Business] ([Id])
ALTER TABLE [dbo].[Customer]  WITH CHECK ADD CONSTRAINT [FK_Customer_Business] FOREIGN KEY([BusinessId])
	REFERENCES [dbo].[Business] ([Id])
ALTER TABLE [dbo].[Session]  WITH CHECK ADD CONSTRAINT [FK_Session_Business] FOREIGN KEY([BusinessId])
	REFERENCES [dbo].[Business] ([Id])
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Location] FOREIGN KEY([LocationId])
	REFERENCES [dbo].[Location] ([Id])
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Coach] FOREIGN KEY([CoachId])
	REFERENCES [dbo].[Coach] ([Id])
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Service] FOREIGN KEY([ServiceId])
	REFERENCES [dbo].[Service] ([Id])
ALTER TABLE [dbo].[Session]  WITH CHECK ADD  CONSTRAINT [FK_Session_Session] FOREIGN KEY([ParentId])
	REFERENCES [dbo].[Session] ([Id])	
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Customer] FOREIGN KEY([CustomerId])
	REFERENCES [dbo].[Customer] ([Id])
ALTER TABLE [dbo].[Booking]  WITH CHECK ADD  CONSTRAINT [FK_Booking_Session] FOREIGN KEY([SessionId])
	REFERENCES [dbo].[Session] ([Id])
GO
