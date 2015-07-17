using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Payments
{
    public abstract class PaymentProviderApiBase : IPaymentProviderApi
    {
        private bool IsTestMessage { get; set; }

        public abstract string SandboxUrl { get; }
        public abstract string LiveUrl { get; }


        protected PaymentProviderApiBase(bool isTestMessage)
        {
            IsTestMessage = isTestMessage;
        }


        public string Url
        {
            get { return IsTestMessage ? SandboxUrl : LiveUrl; }
        }


        public abstract bool VerifyPayment(PaymentProcessingMessage message);
    }
}
