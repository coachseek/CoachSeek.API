using CoachSeek.Common.Extensions;
using Coachseek.Integration.Contracts.Interfaces;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    public class PaymentProcessorConfiguration : IPaymentProcessorConfiguration
    {
        public bool IsPaymentEnabled
        {
            get { return AppSettings.IsPaymentEnabled.Parse<bool>(); ; }
        }
    }
}
