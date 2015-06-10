using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Models;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {

        public static List<DbTransaction> Transactions { get; private set; }


        static InMemoryTransactionRepository()
        {
            Transactions = new List<DbTransaction>();
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
            var dbTransaction = Mapper.Map<Transaction, DbTransaction>(transaction);
            Transactions.Add(dbTransaction);

            return GetTransaction(transaction.Id);
        }


        public PaymentData GetPayment(string id)
        {
            var payment = Transactions.SingleOrDefault(x => x.Id == id && x.Type == Constants.TRANSACTION_PAYMENT);
            return Mapper.Map<DbTransaction, PaymentData>(payment);
        }

        public PaymentData AddPayment(Payment payment)
        {
            AddTransaction(payment);

            return GetPayment(payment.Id);
        }
    }
}
