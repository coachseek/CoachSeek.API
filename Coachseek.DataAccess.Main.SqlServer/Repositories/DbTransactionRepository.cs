using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace Coachseek.DataAccess.Main.SqlServer.Repositories
{
    public class DbTransactionRepository : DbRepositoryBase
    {
        public DbTransactionRepository(string connectionStringKey)
            : base(connectionStringKey) 
        { }


        public async Task<Payment> GetPaymentAsync(string paymentProvider, string id)
        {
            SqlConnection connection = null;
            SqlDataReader reader = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Transaction_GetPaymentByProviderAndId]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@paymentProvider", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar));

                command.Parameters[0].Value = paymentProvider;
                command.Parameters[1].Value = id;

                reader = await command.ExecuteReaderAsync();

                if (reader.HasRows && reader.Read())
                    return new Payment(ReadTransactionData(reader));

                return null;
            }
            finally
            {
                CloseConnection(connection);
                CloseReader(reader);
            }
        }

        public Payment GetPayment(string paymentProvider, string id)
        {
            var wasAlreadyOpen = false;
            SqlDataReader reader = null;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Transaction_GetPaymentByProviderAndId]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@paymentProvider", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar));

                command.Parameters[0].Value = paymentProvider;
                command.Parameters[1].Value = id;

                reader = command.ExecuteReader();

                if (reader.HasRows && reader.Read())
                    return new Payment(ReadTransactionData(reader));

                return null;
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
                if (reader != null)
                    reader.Close();
            }
        }

        public async Task AddPaymentAsync(NewPayment payment)
        {
            SqlConnection connection = null;

            try
            {
                connection = await OpenConnectionAsync();

                var command = new SqlCommand("[Transaction_Create]", connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@paymentProvider", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@status", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@transactionDate", SqlDbType.DateTime2));
                command.Parameters.Add(new SqlParameter("@processedDate", SqlDbType.DateTime2));
                command.Parameters.Add(new SqlParameter("@payerFirstName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@payerLastName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@payerEmail", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@merchantId", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@merchantName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@merchantEmail", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@itemId", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@itemName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@itemCurrency", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@itemAmount", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@isTesting", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@originalMessage", SqlDbType.NVarChar));

                command.Parameters[0].Value = payment.Id;
                command.Parameters[1].Value = payment.PaymentProvider;
                command.Parameters[2].Value = payment.Type;
                command.Parameters[3].Value = payment.Status;
                command.Parameters[4].Value = payment.TransactionDate;
                command.Parameters[5].Value = payment.ProcessedDate;
                command.Parameters[6].Value = payment.PayerFirstName;
                command.Parameters[7].Value = payment.PayerLastName;
                command.Parameters[8].Value = payment.PayerEmail;
                command.Parameters[9].Value = payment.MerchantId;
                command.Parameters[10].Value = payment.MerchantName;
                command.Parameters[11].Value = payment.MerchantEmail;
                command.Parameters[12].Value = payment.ItemId;
                command.Parameters[13].Value = payment.ItemName;
                command.Parameters[14].Value = payment.ItemCurrency;
                command.Parameters[15].Value = payment.ItemAmount;
                command.Parameters[16].Value = payment.IsTesting;
                command.Parameters[17].Value = payment.OriginalMessage;

                await command.ExecuteNonQueryAsync();
            }
            finally
            {
                CloseConnection(connection);
            }
        }

        public void AddPayment(NewPayment payment)
        {
            var wasAlreadyOpen = false;

            try
            {
                wasAlreadyOpen = OpenConnectionOld();

                var command = new SqlCommand("[Transaction_Create]", Connection) { CommandType = CommandType.StoredProcedure };

                command.Parameters.Add(new SqlParameter("@id", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@paymentProvider", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@status", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@transactionDate", SqlDbType.DateTime2));
                command.Parameters.Add(new SqlParameter("@processedDate", SqlDbType.DateTime2));
                command.Parameters.Add(new SqlParameter("@payerFirstName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@payerLastName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@payerEmail", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@merchantId", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@merchantName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@merchantEmail", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@itemId", SqlDbType.UniqueIdentifier));
                command.Parameters.Add(new SqlParameter("@itemName", SqlDbType.NVarChar));
                command.Parameters.Add(new SqlParameter("@itemCurrency", SqlDbType.NChar));
                command.Parameters.Add(new SqlParameter("@itemAmount", SqlDbType.Decimal));
                command.Parameters.Add(new SqlParameter("@isTesting", SqlDbType.Bit));
                command.Parameters.Add(new SqlParameter("@originalMessage", SqlDbType.NVarChar));

                command.Parameters[0].Value = payment.Id;
                command.Parameters[1].Value = payment.PaymentProvider;
                command.Parameters[2].Value = payment.Type;
                command.Parameters[3].Value = payment.Status;
                command.Parameters[4].Value = payment.TransactionDate;
                command.Parameters[5].Value = payment.ProcessedDate;
                command.Parameters[6].Value = payment.PayerFirstName;
                command.Parameters[7].Value = payment.PayerLastName;
                command.Parameters[8].Value = payment.PayerEmail;
                command.Parameters[9].Value = payment.MerchantId;
                command.Parameters[10].Value = payment.MerchantName;
                command.Parameters[11].Value = payment.MerchantEmail;
                command.Parameters[12].Value = payment.ItemId;
                command.Parameters[13].Value = payment.ItemName;
                command.Parameters[14].Value = payment.ItemCurrency;
                command.Parameters[15].Value = payment.ItemAmount;
                command.Parameters[16].Value = payment.IsTesting;
                command.Parameters[17].Value = payment.OriginalMessage;

                command.ExecuteNonQuery();
            }
            finally
            {
                CloseConnection(wasAlreadyOpen);
            }
        }


        private TransactionData ReadTransactionData(SqlDataReader reader)
        {
            return new TransactionData
            {
                Id = reader.GetString(0),
                PaymentProvider = reader.GetString(1),
                Type = reader.GetString(2),
                Status = reader.GetString(3),
                TransactionDate = reader.GetDateTime(4),
                ProcessedDate = reader.GetDateTime(5),
                PayerFirstName = reader.GetString(6),
                PayerLastName = reader.GetString(7),
                PayerEmail = reader.GetString(8),
                MerchantId = reader.GetGuid(9),
                MerchantName = reader.GetString(10),
                MerchantEmail = reader.GetString(11),
                ItemId = reader.GetGuid(12),
                ItemName = reader.GetString(13),
                ItemCurrency = reader.GetString(14),
                ItemAmount = reader.GetDecimal(15),
                IsTesting = reader.GetBoolean(16)
            };
        }
    }
}
