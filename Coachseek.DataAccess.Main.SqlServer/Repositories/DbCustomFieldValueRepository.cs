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

        //public async Task UpdateCustomerAsync(Guid businessId, Customer customer)
        //{
        //    SqlConnection connection = null;

        //    try
        //    {
        //        connection = await OpenConnectionAsync();

        //        var command = new SqlCommand("[Customer_Update]", connection)
        //        {
        //            CommandType = CommandType.StoredProcedure
        //        };

        //        command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
        //        command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
        //        command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
        //        command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
        //        command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar));
        //        command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
        //        command.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.Date));

        //        command.Parameters[0].Value = businessId;
        //        command.Parameters[1].Value = customer.Id;
        //        command.Parameters[2].Value = customer.FirstName;
        //        command.Parameters[3].Value = customer.LastName;
        //        command.Parameters[4].Value = customer.Email;
        //        command.Parameters[5].Value = customer.Phone;
        //        command.Parameters[6].Value = customer.DateOfBirth;

        //        await command.ExecuteNonQueryAsync();
        //    }
        //    finally
        //    {
        //        CloseConnection(connection);
        //    }
        //}

        //public void UpdateCustomer(Guid businessId, Customer customer)
        //{
        //    var wasAlreadyOpen = false;

        //    try
        //    {
        //        wasAlreadyOpen = OpenConnectionOld();

        //        var command = new SqlCommand("Customer_Update", Connection) { CommandType = CommandType.StoredProcedure };

        //        command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
        //        command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
        //        command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
        //        command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
        //        command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar));
        //        command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
        //        command.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.Date));

        //        command.Parameters[0].Value = businessId;
        //        command.Parameters[1].Value = customer.Id;
        //        command.Parameters[2].Value = customer.FirstName;
        //        command.Parameters[3].Value = customer.LastName;
        //        command.Parameters[4].Value = customer.Email;
        //        command.Parameters[5].Value = customer.Phone;
        //        command.Parameters[6].Value = customer.DateOfBirth;

        //        command.ExecuteNonQuery();
        //    }
        //    finally
        //    {
        //        CloseConnection(wasAlreadyOpen);
        //    }
        //}

        public async Task DeleteCustomFieldTemplateAsync(Guid businessId, string type, string key)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldTemplate_DeleteByTypeAndKey]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = type;
                command.Parameters[2].Value = key;

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
