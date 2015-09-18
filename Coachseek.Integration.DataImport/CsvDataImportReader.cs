using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Coachseek.Integration.DataImport
{
    public class CsvDataImportReader : DataImportReader
    {
        public CsvDataImportReader(string filePath, bool hasHeaderRow)
            : base(filePath, hasHeaderRow, ',')
        { }

        public List<CsvData> ReadDataRows()
        {
            return GetDataIterator().Skip(1).ToList();
        }

        public IEnumerable<CsvData> GetDataIterator()
        {
            using (var readFile = new StreamReader(FilePath))
            {
                string line;
                while ((line = readFile.ReadLine()) != null)
                {
                    var row = line.Split(Separator);
                    var list = CovertRowToList(row);
                    yield return list;
                }
            }
        }

        public CsvData CovertRowToList(string[] row)
        {
            return new CsvData(row); ;
        }

        public char Separator { get; private set; }

        public bool SkipTheFirstColumn { get; private set; }

        public string FilePath { get; private set; }
    }
}
