using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbCustomFieldValueRepository : DbRepositoryBase
    {
        public DbCustomFieldValueRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }

        public async Task<CustomFieldValueData> GetCustomFieldValueAsync(Guid businessId, string type, Guid typeId, string key)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldValue_Get]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@typeGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = type;
                command.Parameters[2].Value = typeId;
                command.Parameters[3].Value = key;

                reader = await command.ExecuteReaderAsync();
                if (reader.HasRows && reader.Read())
                    return ReadCustomerFieldValueData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task<IList<CustomFieldValueData>> GetCustomFieldValuesAsync(Guid businessId, string type, Guid typeId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldValue_GetByTypeGuid]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@typeGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = type;
                command.Parameters[2].Value = typeId;

                var values = new List<CustomFieldValueData>();
                reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    values.Add(ReadCustomerFieldValueData(reader));

                return values;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task AddCustomFieldValueAsync(Guid businessId, CustomFieldValue fieldValue)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldValue_Create]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@typeGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@value", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = fieldValue.Type;
                command.Parameters[2].Value = fieldValue.TypeId;
                command.Parameters[3].Value = fieldValue.Key;
                command.Parameters[4].Value = fieldValue.Value;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public async Task UpdateCustomFieldValueAsync(Guid businessId, CustomFieldValue fieldValue)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldValue_Update]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@typeGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@value", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = fieldValue.Type;
                command.Parameters[2].Value = fieldValue.TypeId;
                command.Parameters[3].Value = fieldValue.Key;
                command.Parameters[4].Value = fieldValue.Value;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public async Task DeleteCustomFieldValueAsync(Guid businessId, CustomFieldValue fieldValue)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldValue_Delete]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@typeGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = fieldValue.Type;
                command.Parameters[2].Value = fieldValue.TypeId;
                command.Parameters[3].Value = fieldValue.Key;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }


        private CustomFieldValueData ReadCustomerFieldValueData(SqlDataReader reader)
        {
            var type = reader.GetString(1);
            var typeId = reader.GetGuid(2);
            var key = reader.GetString(3);
            var value = reader.GetString(4);

            return new CustomFieldValueData
            {
                Type = type,
                TypeId = typeId,
                Key = key,
                Value = value
            };
        }
    }
}
