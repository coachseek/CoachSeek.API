using CoachSeek.Common.Extensions;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using Coachseek.Infrastructure.Queueing.Azure;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueClient = new AzurePaymentProcessingQueueClient();
            var paymentRepository = new InMemoryPaymentRepository();
            var isPaymentEnabled = AppSettings.IsPaymentEnabled.Parse<bool>();

            var useCase = new ProcessPaymentsUseCase(queueClient, paymentRepository, isPaymentEnabled);
            useCase.Process();
        }
    }
}
