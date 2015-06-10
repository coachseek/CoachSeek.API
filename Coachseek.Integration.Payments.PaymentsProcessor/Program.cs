using CoachSeek.Common.Extensions;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using Coachseek.Infrastructure.Queueing.Azure;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            DbAutoMapperConfigurator.Configure();

            var queueClient = new AzurePaymentProcessingQueueClient();
            var paymentRepository = new InMemoryTransactionRepository();
            var isPaymentEnabled = AppSettings.IsPaymentEnabled.Parse<bool>();

            var useCase = new ProcessPaymentsUseCase(queueClient, paymentRepository, isPaymentEnabled);

            while (true)
                useCase.Process();
        }
    }
}
