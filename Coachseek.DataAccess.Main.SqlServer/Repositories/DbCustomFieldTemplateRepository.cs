using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbCustomFieldTemplateRepository : DbRepositoryBase
    {
        public DbCustomFieldTemplateRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public async Task<IList<CustomFieldTemplateData>> GetCustomFieldTemplatesAsync(Guid businessId, string type, string key = null)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldTemplate_GetByTypeAndKey]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = type;
                command.Parameters[2].Value = key;

                var templates = new List<CustomFieldTemplateData>();
                reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    templates.Add(ReadCustomerFieldTemplateData(reader));

                return templates;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task<CustomFieldTemplateData> GetCustomFieldTemplateAsync(Guid businessId, Guid templateId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldTemplate_GetByGuid]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@templateGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = templateId;

                reader = await command.ExecuteReaderAsync();
                if (reader.HasRows && reader.Read())
                    return ReadCustomerFieldTemplateData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task AddCustomFieldTemplateAsync(Guid businessId, CustomFieldTemplate template)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldTemplate_Create]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@templateGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@isRequired", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.Bit));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = template.Id;
                command.Parameters[2].Value = template.Type;
                command.Parameters[3].Value = template.Key;
                command.Parameters[4].Value = template.Name;
                command.Parameters[5].Value = template.IsRequired;
                command.Parameters[6].Value = template.IsActive;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public async Task UpdateCustomFieldTemplateAsync(Guid businessId, CustomFieldTemplate template)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldTemplate_Update]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@templateGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@isRequired", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.Bit));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = template.Id;
                command.Parameters[2].Value = template.Type;
                command.Parameters[3].Value = template.Key;
                command.Parameters[4].Value = template.Name;
                command.Parameters[5].Value = template.IsRequired;
                command.Parameters[6].Value = template.IsActive;

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

        public async Task SetCustomFieldTemplateIsActiveAsync(Guid businessId, Guid templateId, bool isActive)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldTemplate_UpdateIsActive]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@templateGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.Bit));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = templateId;
                command.Parameters[2].Value = isActive;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }


        private CustomFieldTemplateData ReadCustomerFieldTemplateData(SqlDataReader reader)
        {
            return new CustomFieldTemplateData
            {
                Id = reader.GetGuid(1),
                Type = reader.GetString(2),
                Key = reader.GetString(3),
                Name = reader.GetString(4),
                IsRequired = reader.GetBoolean(5),
                IsActive = reader.GetBoolean(6)
            };
        }
    }
}
