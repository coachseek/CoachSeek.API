using System;
using System.Collections.Generic;

namespace Coachseek.Integration.DataImport
{
    public interface IDataImportReader
    {
        Char Seperator { get; }
        string FilePath { get; }
        bool SkipTheFirstColumn { get; }
        List<CsvData> ReadDataRows();
        IEnumerable<CsvData> GetDataIterator();
        CsvData CovertRowToList(string[] row);
    }
}
