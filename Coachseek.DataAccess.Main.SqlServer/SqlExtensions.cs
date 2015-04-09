using System;

namespace Coachseek.DataAccess.Main.SqlServer
{
    public static class SqlExtensions
    {
        public static object ConvertNullToDbNull(this object obj)
        {
            return obj ?? DBNull.Value;
        }
    }
}
