USE [CsvImport]
GO

BULK
INSERT CustomerData
FROM 'C:\Data\Import\Rahman Smiley Contacts 2.csv'
WITH
(
FIELDTERMINATOR = ',',
ROWTERMINATOR = '\n'
)
GO
--Check the content of the table.
SELECT *
FROM CustomerData
GO

