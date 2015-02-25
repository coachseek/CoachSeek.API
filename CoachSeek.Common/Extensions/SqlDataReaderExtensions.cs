﻿using System.Data.SqlClient;

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

        public static string GetNullableStringTrimmed(this SqlDataReader reader, int columnIndex)
        {
            if (reader.IsDBNull(columnIndex))
                return null;

            return reader.GetString(columnIndex).Trim();
        }
    }
}
