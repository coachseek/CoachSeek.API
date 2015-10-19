using System.Threading.Tasks;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Payments.Interfaces;

namespace Coachseek.Integration.Tests.Unit.Payments.Fakes
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


        public async Task<bool> VerifyPaymentAsync(PaymentProcessingMessage message)
        {
            await Task.Delay(10);
            return VerifyPayment(message);
        }
    }
}
