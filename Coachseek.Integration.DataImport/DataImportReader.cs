﻿using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace Coachseek.Integration.DataImport
{
    public class DataImportReader : IDataImportReader
    {
        public bool HasHeaderRow { get; private set; }
        public char Separator { get; private set; }
        public string FilePath { get; private set; }
        

        public DataImportReader(string filePath, bool hasHeaderRow, char separator)
        {
            HasHeaderRow = hasHeaderRow;
            Separator = separator;
            FilePath = filePath;
        }

        public List<CsvData> ReadDataRows()
        {
            return HasHeaderRow ? GetDataIterator().Skip(1).ToList() : GetDataIterator().ToList();
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
            return new CsvData(row);
        }
    }
}
