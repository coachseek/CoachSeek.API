using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbBusinessEntityRepository : DbRepositoryBase
    {
        public DbBusinessEntityRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public async Task<BusinessData> GetBusinessAsync(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                reader = await command.ExecuteReaderAsync();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public BusinessData GetBusiness(Guid businessId)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Business_GetByGuid]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public async Task<BusinessData> GetBusinessAsync(string domain)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_GetByDomain]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar));
                command.Parameters[0].Value = domain;

                reader = await command.ExecuteReaderAsync();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public BusinessData GetBusiness(string domain)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnection();

                var command = new SqlCommand("[Business_GetByDomain]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar));
                command.Parameters[0].Value = domain;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public async Task AddBusinessAsync(NewBusiness business)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_Create]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@sport", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@currency", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@isOnlinePaymentEnabled", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@forceOnlinePayment", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@paymentProvider", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@merchantAccountIdentifier", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@createdOn", SqlDbType.DateTime2));
                command.Parameters.Add(new SqlParameter("@authorisedUntil", SqlDbType.DateTime2));
                command.Parameters.Add(new SqlParameter("@subscription", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@isTesting", SqlDbType.Bit));

                command.Parameters[0].Value = business.Id;
                command.Parameters[1].Value = business.Name;
                command.Parameters[2].Value = business.Domain;
                command.Parameters[3].Value = business.Sport;
                command.Parameters[4].Value = business.CurrencyCode;
                command.Parameters[5].Value = business.IsOnlinePaymentEnabled;
                command.Parameters[6].Value = business.ForceOnlinePayment;
                command.Parameters[7].Value = business.PaymentProvider;
                command.Parameters[8].Value = business.MerchantAccountIdentifier;
                command.Parameters[9].Value = business.CreatedOn;
                command.Parameters[10].Value = business.AuthorisedUntil;
                command.Parameters[11].Value = business.SubscriptionPlan;
                command.Parameters[12].Value = business.IsTesting;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public async Task UpdateBusinessAsync(Business business)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_Update]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@currency", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@isOnlinePaymentEnabled", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@forceOnlinePayment", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@paymentProvider", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@merchantAccountIdentifier", SqlDbType.NVarChar));

                command.Parameters[0].Value = business.Id;
                command.Parameters[1].Value = business.Name;
                command.Parameters[2].Value = business.CurrencyCode;
                command.Parameters[3].Value = business.IsOnlinePaymentEnabled;
                command.Parameters[4].Value = business.ForceOnlinePayment;
                command.Parameters[5].Value = business.PaymentProvider;
                command.Parameters[6].Value = business.MerchantAccountIdentifier;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }

        public async Task SetAuthorisedUntilAsync(Guid businessId, DateTime authorisedUntil)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_UpdateAuthorisedUntil]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@authorisedUntil", SqlDbType.DateTime2));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = authorisedUntil;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }


        private BusinessData ReadBusinessData(SqlDataReader reader)
        {
            var id = reader.GetGuid(0);
            var name = reader.GetString(1);
            var domain = reader.GetString(2);
            var sport = reader.GetNullableString(3);
            var currency = reader.GetNullableString(4);
            var isOnlinePaymentEnabled = reader.GetBoolean(5);
            var forceOnlinePayment = reader.GetBoolean(6);
            var paymentProvider = reader.GetNullableString(7);
            var merchantAccountIdentifier = reader.GetNullableString(8);
            var authorisedUntil = reader.GetDateTime(9);
            var subscription = reader.GetString(10);

            return new BusinessData
            {
                Id = id,
                Name = name,
                Domain = domain,
                Sport = sport,
                AuthorisedUntil = authorisedUntil,
                SubscriptionPlan = subscription,
                Payment = new BusinessPaymentData
                {
                    Currency = currency,
                    IsOnlinePaymentEnabled = isOnlinePaymentEnabled,
                    ForceOnlinePayment = forceOnlinePayment,
                    PaymentProvider = paymentProvider,
                    MerchantAccountIdentifier = merchantAccountIdentifier
                }
            };
        }
    }
}
