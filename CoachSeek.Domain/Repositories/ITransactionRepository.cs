using System.Threading.Tasks;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Repositories
{
    public interface ITransactionRepository
    {
        //TransactionData GetTransaction(string id);
        //TransactionData GetTransaction(string id, TransactionType type);
        //TransactionData AddTransaction(Transaction transaction);

        Task<Payment> GetPaymentAsync(string paymentProvider, string id);
        Task AddPaymentAsync(NewPayment payment);

        //PaymentData UpdatePayment(Payment payment);
    }
}
