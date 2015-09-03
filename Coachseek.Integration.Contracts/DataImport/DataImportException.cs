using System;

namespace Coachseek.Integration.Contracts.DataImport
{
    public class DataImportException : Exception
    {
        public DataImportException()
        { }

        public DataImportException(string message) 
            : base(message)
        { }

        public DataImportException(string message, Exception ex)
            : base(message, ex)
        { }
    }
}
