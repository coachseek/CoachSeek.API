using System.Data.SqlClient;

namespace CoachSeek.Common.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static string SafeGetString(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;

            return reader.GetString(columnIndex);
        }

        public static string SafeGetStringTrimmed(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;

            return reader.GetString(columnIndex).Trim();
        }
    }
}
