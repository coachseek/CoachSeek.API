using CoachSeek.Common;
using Coachseek.Integration.Contracts.Payments.Interfaces;

namespace Coachseek.Integration.Tests.Unit.Payments.Fakes
{
    public class StubPaymentProcessorConfiguration : IPaymentProcessorConfiguration
    {
        public bool WasGetEnvironmentCalled;
        public Environment SetEnvironment;


        public StubPaymentProcessorConfiguration(Environment environment = Environment.Testing)
        {
            SetEnvironment = environment;
        }


        public Environment Environment
        {
            get
            {
                WasGetEnvironmentCalled = true;

                return SetEnvironment;
            }
        }
    }
}
