USE Coachseek

-- Drop Constraints
ALTER TABLE [dbo].[Location] DROP CONSTRAINT [FK_Location_Business]
ALTER TABLE [dbo].[Coach] DROP CONSTRAINT [FK_Coach_Business]
GO

-- Truncate Tables
TRUNCATE TABLE [dbo].[Location]
TRUNCATE TABLE [dbo].[Coach]
TRUNCATE TABLE [dbo].[Business]

-- Recreate Constraints
ALTER TABLE [dbo].[Location]  WITH CHECK ADD CONSTRAINT [FK_Location_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
ALTER TABLE [dbo].[Coach]  WITH CHECK ADD CONSTRAINT [FK_Coach_Business] FOREIGN KEY([BusinessId])
REFERENCES [dbo].[Business] ([Id])
GO
