using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;
using Coachseek.Integration.Payments;

namespace Coachseek.Integration.Tests.Unit.Fakes
{
    public class StubPaymentProviderApiFactory : IPaymentProviderApiFactory
    {
        public StubPaymentProviderApi PaymentProviderApi;
        public bool WasGetPaymentProviderApiCalled;


        public IPaymentProviderApi GetPaymentProviderApi(PaymentProcessingMessage message, bool isTestMessage)
        {
            WasGetPaymentProviderApiCalled = true;

            return PaymentProviderApi;
        }
    }
}
