using CoachSeek.Common;

namespace Coachseek.Integration.Contracts.Payments.Interfaces
{
    public interface IPaymentProcessorConfiguration
    {
        Environment Environment { get; }
    }
}
