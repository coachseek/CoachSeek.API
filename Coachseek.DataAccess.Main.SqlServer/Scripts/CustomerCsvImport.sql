USE [CsvImport]
GO

BULK
INSERT CustomerData
FROM 'C:\DataImport\CPSTennis.csv'
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

