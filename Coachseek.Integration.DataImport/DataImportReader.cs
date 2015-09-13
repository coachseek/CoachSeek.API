using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Coachseek.Integration.DataImport
{
    public class DataImportReader : IDataImportReader
    {
        public DataImportReader(string filePath, bool skipTheFirstColumn, char seperator)
        {
            SkipTheFirstColumn = skipTheFirstColumn;
            Seperator = seperator;
            FilePath = filePath;
        }

        public List<CsvData> ReadDataRows()
        {
            return SkipTheFirstColumn ? GetDataIterator().Skip(1).ToList() : GetDataIterator().ToList();
        }

        public IEnumerable<CsvData> GetDataIterator()
        {
            using (var readFile = new StreamReader(FilePath))
            {
                string line;
                while ((line = readFile.ReadLine()) != null)
                {
                    var row = line.Split(Seperator);
                    var list = CovertRowToList(row);
                    yield return list;
                }
            }
        }

        public CsvData CovertRowToList(string[] row)
        {
            return new CsvData(row); ;
        }

        public char Seperator { get; private set; }

        public bool SkipTheFirstColumn { get; private set; }

        public string FilePath { get; private set; }
    }
}
