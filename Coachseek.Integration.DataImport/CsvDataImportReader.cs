namespace Coachseek.Integration.DataImport
{
    public class CsvDataImportReader : DataImportReader
    {
        public CsvDataImportReader(string filePath, bool hasHeaderRow)
            : base(filePath, hasHeaderRow, ',')
        { }
    }
}
