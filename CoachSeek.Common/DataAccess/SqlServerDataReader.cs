using System;
using System.Data;
using System.Data.SqlClient;

namespace CoachSeek.Common.DataAccess
{
    public class SqlServerDataReader
    {
        public static object Read()
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;
            try
            {
                connection = new SqlConnection("Server=REDDWARF;Database=Coachseek;User Id=sa;Password=C0@ch5eek;");
                connection.Open();

                var command = new SqlCommand("Business_Create", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier, 0, "Guid"));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, 100, "Name"));
                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar, 100, "Domain"));

                command.Parameters[0].Value = Guid.NewGuid();
                command.Parameters[1].Value = "Hello World!";
                command.Parameters[2].Value = "helloworld";

                reader = command.ExecuteReader(CommandBehavior.SchemaOnly);
                var schemaTable = reader.GetSchemaTable();
                if (schemaTable == null)
                    return null;

                string columns = "";
                foreach (DataRow row in schemaTable.Rows)
                {
                    foreach (DataColumn column in schemaTable.Columns)
                    {
                        var z = row.HasErrors;
                        columns = string.Format("{0}, {1}", columns, column.ColumnName);
                    }
                }

                var x = columns;

                //if (reader.HasRows && reader.Read())
                //{
                //    return new Business2Data
                //    {
                //        Id = reader.GetGuid(1),
                //        Name = reader.GetString(2),
                //        Domain = reader.GetString(3)
                //    };
                //}

                return null;
            }
            finally
            {
                if (connection != null)
                    connection.Close();
                if (reader != null)
                    reader.Close();
            }
        }
    }
}
