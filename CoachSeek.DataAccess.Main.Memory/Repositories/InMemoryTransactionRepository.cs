using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoachSeek.Common;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        // Spy behaviour is included
        public bool WasAddPaymentCalled;
        public bool WasVerifyPaymentCalled;

        public static List<DbTransaction> Transactions { get; private set; }


        static InMemoryTransactionRepository()
        {
            Transactions = new List<DbTransaction>();
        }

        public InMemoryTransactionRepository()
        {
        }

        public InMemoryTransactionRepository(IEnumerable<DbTransaction> transactions)
        {
            Transactions = new List<DbTransaction>(transactions);
        }


        public void Clear()
        {
            Transactions.Clear();
        }

        public TransactionData GetTransaction(string id)
        {
            var transaction = Transactions.SingleOrDefault(x => x.Id == id);
            return Mapper.Map<DbTransaction, TransactionData>(transaction);
        }

        public TransactionData GetTransaction(string id, TransactionType type)
        {
            var transaction = Transactions.SingleOrDefault(x => x.Id == id && x.Type == type.ToString());
            return Mapper.Map<DbTransaction, TransactionData>(transaction);
        }

        public TransactionData AddTransaction(Transaction transaction)
        {
            WasAddPaymentCalled = true;

            var dbTransaction = Mapper.Map<Transaction, DbTransaction>(transaction);
            Transactions.Add(dbTransaction);

            return GetTransaction(transaction.Id);
        }

        public void VerifyTransaction(Transaction transaction)
        {
            var dbTransaction = Transactions.Single(x => x.Id == transaction.Id);
            if (dbTransaction.IsNotFound())
                return;
            dbTransaction.IsVerified = transaction.IsVerified;
        }


        public Payment GetPayment(string id)
        {
            var payment = Transactions.SingleOrDefault(x => x.Id == id && x.Type == Constants.TRANSACTION_PAYMENT);
            if (payment.IsNotFound())
                return null;
            var data = Mapper.Map<DbTransaction, PaymentData>(payment);
            return new Payment(data, true);
        }

        public void AddPayment(NewPayment newPayment)
        {
            WasAddPaymentCalled = true;

            var dbTransaction = Mapper.Map<NewPayment, DbTransaction>(newPayment);
            Transactions.Add(dbTransaction);
        }

        public void VerifyPayment(Payment payment)
        {
            WasVerifyPaymentCalled = true;

            VerifyTransaction(payment);
        }
    }
}
