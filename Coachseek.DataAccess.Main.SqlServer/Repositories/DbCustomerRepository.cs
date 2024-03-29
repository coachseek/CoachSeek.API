﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbCustomerRepository : DbRepositoryBase
    {
        public DbCustomerRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public async Task<IList<CustomerData>> GetAllCustomersAsync(Guid businessId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Customer_GetAll]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                var customers = new List<CustomerData>();
                reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    customers.Add(ReadCustomerData(reader));

                return customers;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task<CustomerData> GetCustomerAsync(Guid businessId, Guid customerId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Customer_GetByGuid]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = customerId;

                reader = await command.ExecuteReaderAsync();
                if (reader.HasRows && reader.Read())
                    return ReadCustomerData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public CustomerData GetCustomer(Guid businessId, Guid customerId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Customer_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[1].Value = customerId;

                reader = command.ExecuteReader();
                if (reader.HasRows && reader.Read())
                    return ReadCustomerData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public async Task AddCustomerAsync(Guid businessId, Customer customer)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Customer_Create]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@sex", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.Date));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = customer.Id;
                command.Parameters[2].Value = customer.FirstName;
                command.Parameters[3].Value = customer.LastName;
                command.Parameters[4].Value = customer.Email;
                command.Parameters[5].Value = customer.Phone;
                command.Parameters[6].Value = customer.SexType;
                command.Parameters[7].Value = customer.DateOfBirth;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public void AddCustomer(Guid businessId, Customer customer)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Customer_Create]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@sex", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.Date));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = customer.Id;
                command.Parameters[2].Value = customer.FirstName;
                command.Parameters[3].Value = customer.LastName;
                command.Parameters[4].Value = customer.Email;
                command.Parameters[5].Value = customer.Phone;
                command.Parameters[6].Value = customer.SexType;
                command.Parameters[7].Value = customer.DateOfBirth;

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public async Task UpdateCustomerAsync(Guid businessId, Customer customer)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Customer_Update]", connection)
                {
                    CommandType = CommandType.StoredProcedure
                };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@sex", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.Date));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = customer.Id;
                command.Parameters[2].Value = customer.FirstName;
                command.Parameters[3].Value = customer.LastName;
                command.Parameters[4].Value = customer.Email;
                command.Parameters[5].Value = customer.Phone;
                command.Parameters[6].Value = customer.SexType;
                command.Parameters[7].Value = customer.DateOfBirth;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public void UpdateCustomer(Guid businessId, Customer customer)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("Customer_Update", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@customerGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@firstName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@lastName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@email", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@phone", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@sex", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@dateOfBirth", SqlDbType.Date));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = customer.Id;
                command.Parameters[2].Value = customer.FirstName;
                command.Parameters[3].Value = customer.LastName;
                command.Parameters[4].Value = customer.Email;
                command.Parameters[5].Value = customer.Phone;
                command.Parameters[6].Value = customer.SexType;
                command.Parameters[7].Value = customer.DateOfBirth;

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }


        private CustomerData ReadCustomerData(SqlDataReader reader)
        {
            return new CustomerData
            {
                Id = reader.GetGuid(1),
                FirstName = reader.GetString(2),
                LastName = reader.GetString(3),
                Email = reader.GetNullableString(4),
                Phone = reader.GetNullableString(5),
                Sex = reader.GetNullableString(6),
                DateOfBirth = reader.GetNullableDate(7)
            };
        }
    }
}
