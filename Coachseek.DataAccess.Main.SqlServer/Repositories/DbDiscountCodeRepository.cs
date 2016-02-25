using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbDiscountCodeRepository : DbRepositoryBase
    {
        public DbDiscountCodeRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public async Task AddDiscountCodeAsync(Guid businessId, DiscountCode discountCode)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[DiscountCode_Create]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@discountCodeGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@discountPercent", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.Bit));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = discountCode.Id;
                command.Parameters[2].Value = discountCode.Code;
                command.Parameters[3].Value = discountCode.DiscountPercent;
                command.Parameters[4].Value = discountCode.IsActive;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public async Task UpdateDiscountCodeAsync(Guid businessId, DiscountCode discountCode)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[DiscountCode_Update]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@discountCodeGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@discountPercent", SqlDbType.Int));
                command.Parameters.Add(new SqlParameter("@isActive", SqlDbType.Bit));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = discountCode.Id;
                command.Parameters[2].Value = discountCode.Code;
                command.Parameters[3].Value = discountCode.DiscountPercent;
                command.Parameters[4].Value = discountCode.IsActive;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public DiscountCodeData GetDiscountCode(Guid businessId, string code)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = OpenConnection();

                var command = new SqlCommand("[DiscountCode_GetByCode]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = code;

                reader = command.ExecuteReader();
                if (reader.HasRows && reader.Read())
                    return ReadDiscountCodeData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task<DiscountCodeData> GetDiscountCodeAsync(Guid businessId, string code)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[DiscountCode_GetByCode]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@code", SqlDbType.NChar));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = code;

                reader = await command.ExecuteReaderAsync();
                if (reader.HasRows && reader.Read())
                    return ReadDiscountCodeData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task<DiscountCodeData> GetDiscountCodeAsync(Guid businessId, Guid discountCodeId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[DiscountCode_GetByGuid]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@discountCodeGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = discountCodeId;

                reader = await command.ExecuteReaderAsync();
                if (reader.HasRows && reader.Read())
                    return ReadDiscountCodeData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task<IList<DiscountCodeData>> GetDiscountCodesAsync(Guid businessId)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[DiscountCode_GetAll]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));

                command.Parameters[0].Value = businessId;

                var discountCodes = new List<DiscountCodeData>();
                reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                    discountCodes.Add(ReadDiscountCodeData(reader));

                return discountCodes;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
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


        private DiscountCodeData ReadDiscountCodeData(SqlDataReader reader)
        {
            return new DiscountCodeData
            {
                Id = reader.GetGuid(1),
                Code = reader.GetString(2).Trim(),
                DiscountPercent = reader.GetInt32(3),
                IsActive = reader.GetBoolean(4)
            };
        }
    }
}
