using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Payments
{
    public abstract class PaymentsProviderApiBase : IPaymentsProviderApi
    {
        private bool IsPaymentEnabled { get; set; }

        public abstract string SandboxUrl { get; }
        public abstract string LiveUrl { get; }


        protected PaymentsProviderApiBase(bool isPaymentEnabled)
        {
            IsPaymentEnabled = isPaymentEnabled;
        }


        public string Url
        {
            get { return IsPaymentEnabled ? LiveUrl : SandboxUrl; }
        }


        public abstract void VerifyPayment(PaymentProcessingMessage message);
    }
}
