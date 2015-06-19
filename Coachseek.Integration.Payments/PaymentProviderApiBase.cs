using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Payments
{
    public abstract class PaymentProviderApiBase : IPaymentProviderApi
    {
        private bool IsPaymentEnabled { get; set; }

        public abstract string SandboxUrl { get; }
        public abstract string LiveUrl { get; }


        protected PaymentProviderApiBase(bool isPaymentEnabled)
        {
            IsPaymentEnabled = isPaymentEnabled;
        }


        public string Url
        {
            get { return IsPaymentEnabled ? LiveUrl : SandboxUrl; }
        }


        public abstract bool VerifyPayment(PaymentProcessingMessage message);
    }
}
