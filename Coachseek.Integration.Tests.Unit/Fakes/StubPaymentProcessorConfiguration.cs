using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Tests.Unit.Fakes
{
    public class StubPaymentProcessorConfiguration : IPaymentProcessorConfiguration
    {
        public bool WasIsPaymentEnabledCalled;
        // Use nullable bool to ensure it's being set before being used.
        public bool? SetIsPaymentEnabled;


        public StubPaymentProcessorConfiguration(bool isPaymentEnabled = false)
        {
            SetIsPaymentEnabled = isPaymentEnabled;
        }


        public bool IsPaymentEnabled
        {
            get
            {
                WasIsPaymentEnabledCalled = true;

                return SetIsPaymentEnabled.Value;
            }
        }
    }
}
