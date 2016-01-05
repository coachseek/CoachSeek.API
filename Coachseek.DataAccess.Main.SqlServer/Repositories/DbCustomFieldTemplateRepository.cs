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

        //public async Task<IList<CustomerData>> GetAllCustomersAsync(Guid businessId)
        //{
        //    SqlConnection connection = null;
        //    SqlDataReader reader = null;

        //    try
        //    {
        //        connection = await OpenConnectionAsync();

        //        var command = new SqlCommand("[Customer_GetAll]", connection) { CommandType = CommandType.StoredProcedure };

        //        command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
        //        command.Parameters[0].Value = businessId;

        //        var customers = new List<CustomerData>();
        //        reader = await command.ExecuteReaderAsync();
        //        while (reader.Read())
        //            customers.Add(ReadCustomerData(reader));

        //        return customers;
        //    }
        //    finally
        //    {
        //        CloseConnection(connection);
        //        CloseReader(reader);
        //    }
        //}

        public async Task<IList<CustomFieldTemplateData>> GetCustomFieldTemplatesAsync(Guid businessId, string type, string key)
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

        //public CustomerData GetCustomer(Guid businessId, Guid customerId)
        //{
        //    var wasAlreadyOpen = false;
        //    SqlDataReader reader = null;

        //    try
        //    {
        //        wasAlreadyOpen = OpenConnectionOld();

        //        var command = new SqlCommand("[Customer_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

        //        command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
        //        command.Parameters[0].Value = businessId;
        //        command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
        //        command.Parameters[1].Value = customerId;

        //        reader = command.ExecuteReader();
        //        if (reader.HasRows && reader.Read())
        //            return ReadCustomerData(reader);

        //        return null;
        //    }
        //    finally
        //    {
        //        CloseConnection(wasAlreadyOpen);
        //        if (reader != null)
        //            reader.Close();
        //    }
        //}

        public async Task AddCustomFieldTemplateAsync(Guid businessId, CustomFieldTemplate template)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[CustomFieldTemplate_Create]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@key", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@isRequired", SqlDbType.Bit));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = template.Type;
                command.Parameters[2].Value = template.Key;
                command.Parameters[3].Value = template.Name;
                command.Parameters[4].Value = template.IsRequired;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        //public void AddCustomer(Guid businessId, Customer customer)
        //{
        //    var wasAlreadyOpen = false;

        //    try
        //    {
        //        wasAlreadyOpen = OpenConnectionOld();

        //        var command = new SqlCommand("[Customer_Create]", Connection) { CommandType = CommandType.StoredProcedure };

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


        private CustomFieldTemplateData ReadCustomerFieldTemplateData(SqlDataReader reader)
        {
            return new CustomFieldTemplateData
            {
                Type = reader.GetString(1),
                Key = reader.GetString(2),
                Name = reader.GetString(3),
                IsRequired = reader.GetBoolean(4)
            };
        }
    }
}
