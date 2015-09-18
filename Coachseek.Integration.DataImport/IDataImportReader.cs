using System;
using System.Collections.Generic;

namespace Coachseek.Integration.DataImport
{
    public interface IDataImportReader
    {
        bool HasHeaderRow { get; }
        Char Separator { get; }
        string FilePath { get; }

        List<CsvData> ReadDataRows();
        IEnumerable<CsvData> GetDataIterator();
        CsvData CovertRowToList(string[] row);
    }
}
