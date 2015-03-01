USE Coachseek

-- Drop Constraints
ALTER TABLE [dbo].[Location] DROP CONSTRAINT [FK_Location_Business]
ALTER TABLE [dbo].[Coach] DROP CONSTRAINT [FK_Coach_Business]
ALTER TABLE [dbo].[Service] DROP CONSTRAINT [FK_Service_Business]
ALTER TABLE [dbo].[Customer] DROP CONSTRAINT [FK_Customer_Business]
ALTER TABLE [dbo].[Session] DROP CONSTRAINT [FK_Session_Business]
GO

-- Truncate Tables
TRUNCATE TABLE [dbo].[Location]
TRUNCATE TABLE [dbo].[Coach]
TRUNCATE TABLE [dbo].[Service]
TRUNCATE TABLE [dbo].[Customer]
TRUNCATE TABLE [dbo].[Session]
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
GO
