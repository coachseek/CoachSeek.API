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

            var useCase = new ProcessPaymentsUseCase(queueClient, paymentRepository);
            useCase.Process();
        }
    }
}
