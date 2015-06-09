using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;

namespace CoachSeek.DataAccess.Main.Memory.Repositories
{
    public class InMemoryPaymentRepository : IPaymentRepository
    {
        public PaymentData AddPayment(Payment payment)
        {
            throw new System.NotImplementedException();
        }
    }
}
