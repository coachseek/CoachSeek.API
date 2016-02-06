USE [CsvImport]
GO

BULK
INSERT CustomerData
FROM 'C:\Data\Import\Jim Wid Contacts 2a.csv'
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

