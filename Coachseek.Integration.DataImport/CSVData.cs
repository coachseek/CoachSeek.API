using System.Collections.Generic;

namespace Coachseek.Integration.DataImport
{
    public class CsvData
    {
        public CsvData(string[] row)
        {
            Data = new List<string>(row);
        }

        public CsvData()
        {
            Data = new List<string>();
        }

        public List<string> Data { get; private set; }
    }
}