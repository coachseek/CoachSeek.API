using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Tests.Unit.Fakes
{
    public class StubPaymentProviderApi : IPaymentProviderApi
    {
        public bool WasVerifyPaymentCalled;
        public bool VerifyPaymentResponse;

        public string SandboxUrl { get; set; }
        public string LiveUrl { get; set; }


        public StubPaymentProviderApi(bool setVerifyPaymentResponse)
        {
            VerifyPaymentResponse = setVerifyPaymentResponse;
        }

        public bool VerifyPayment(PaymentProcessingMessage message)
        {
            WasVerifyPaymentCalled = true;

            return VerifyPaymentResponse;
        }
    }
}
