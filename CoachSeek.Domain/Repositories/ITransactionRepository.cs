using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Repositories
{
    public interface ITransactionRepository
    {
        TransactionData GetTransaction(string id);
        TransactionData GetTransaction(string id, TransactionType type);
        TransactionData AddTransaction(Transaction transaction);

        PaymentData GetPayment(string id);
        PaymentData AddPayment(Payment payment);
        //PaymentData UpdatePayment(Payment payment);
    }
}
