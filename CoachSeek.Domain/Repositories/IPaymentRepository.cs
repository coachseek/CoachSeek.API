using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Domain.Repositories
{
    public interface IPaymentRepository
    {
        //PaymentData GetPayment(Guid paymentId);
        PaymentData AddPayment(Payment payment);
        //PaymentData UpdatePayment(Payment payment);
    }
}
