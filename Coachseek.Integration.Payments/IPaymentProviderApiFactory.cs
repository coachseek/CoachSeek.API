using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Payments.Interfaces;

namespace Coachseek.Integration.Payments
{
    public interface IPaymentProviderApiFactory
    {
        IPaymentProviderApi GetPaymentProviderApi(PaymentProcessingMessage message, bool isTestMessage);
    }
}
