using System;
using System.Data.SqlClient;

namespace CoachSeek.Common.Extensions
{
    public static class SqlDataReaderExtensions
    {
        public static bool? GetNullableBool(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;
            return reader.GetBoolean(columnIndex);
        }

        public static Guid? GetNullableGuid(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;
            return reader.GetGuid(columnIndex);
        }

        public static byte? GetNullableByte(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;
            return reader.GetByte(columnIndex);
        }

        public static short? GetNullableInt16(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;
            return reader.GetInt16(columnIndex);
        }

        public static decimal? GetNullableDecimal(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;
            return reader.GetDecimal(columnIndex);
        }

        public static string GetNullableString(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;
            return reader.GetString(columnIndex);
        }

        public static char? GetNullableChar(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;
            return reader.GetChar(columnIndex);
        }

        public static string GetNullableStringTrimmed(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;
            return reader.GetString(columnIndex).Trim();
        }

        public static string GetDate(this SqlDataReader reader, int columnIndex)
        {
            return reader.GetDateTime(columnIndex).ToString("yyyy-MM-dd");
        }

        public static string GetNullableDate(this SqlDataReader reader, int columnIndex)
        {
            return reader.IsDBNull(columnIndex) ? null : GetDate(reader, columnIndex);
        }
    }
}
