﻿using System;
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
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_GetByGuid]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters[0].Value = businessId;

                reader = await command.ExecuteReaderAsync();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task<BusinessData> GetBusinessAsync(string domain)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_GetByDomain]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar));
                command.Parameters[0].Value = domain;

                reader = await command.ExecuteReaderAsync();

                if (reader.HasRows && reader.Read())
                    return ReadBusinessData(reader);

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public async Task AddBusinessAsync(NewBusiness business)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_Create]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@sport", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@currency", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@isOnlinePaymentEnabled", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@forceOnlinePayment", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@paymentProvider", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@merchantAccountIdentifier", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@useProRataPricing", SqlDbType.Bit));
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
                command.Parameters[9].Value = business.UseProRataPricing;
                command.Parameters[10].Value = business.CreatedOn;
                command.Parameters[11].Value = business.AuthorisedUntil;
                command.Parameters[12].Value = business.SubscriptionPlan;
                command.Parameters[13].Value = business.IsTesting;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public async Task UpdateBusinessAsync(Business business)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_Update]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@guid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@domain", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@sport", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@currency", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@isOnlinePaymentEnabled", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@forceOnlinePayment", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@paymentProvider", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@merchantAccountIdentifier", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@useProRataPricing", SqlDbType.Bit));

                command.Parameters[0].Value = business.Id;
                command.Parameters[1].Value = business.Name;
                command.Parameters[2].Value = business.Domain;
                command.Parameters[3].Value = business.Sport;
                command.Parameters[4].Value = business.CurrencyCode;
                command.Parameters[5].Value = business.IsOnlinePaymentEnabled;
                command.Parameters[6].Value = business.ForceOnlinePayment;
                command.Parameters[7].Value = business.PaymentProvider;
                command.Parameters[8].Value = business.MerchantAccountIdentifier;
                command.Parameters[9].Value = business.UseProRataPricing;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public async Task SetAuthorisedUntilAsync(Guid businessId, DateTime authorisedUntil)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_UpdateAuthorisedUntil]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@authorisedUntil", SqlDbType.DateTime2));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = authorisedUntil;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public async Task SetUseProRataPricingAsync(Guid businessId, bool useProRataPricing)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Business_UpdateUseProRataPricing]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@businessGuid", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@useProRataPricing", SqlDbType.Bit));

                command.Parameters[0].Value = businessId;
                command.Parameters[1].Value = useProRataPricing;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
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
            var useProRataPricing = reader.GetBoolean(11);
            var totalNumberOfSessions = reader.GetInt32(12);

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
                    MerchantAccountIdentifier = merchantAccountIdentifier,
                    UseProRataPricing = useProRataPricing
                },
                Statistics = new BusinessStatisticsData
                {
                    TotalNumberOfSessions = totalNumberOfSessions
                }
            };
        }
    }
}
