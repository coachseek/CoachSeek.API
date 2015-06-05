using Coachseek.API.Client.Services;
using CoachSeek.Application.UseCases.Emailing;
using Coachseek.Infrastructure.Queueing.Amazon;

namespace Coachseek.Integration.Emailing.BouncedEmailProcessor
{
    /// <summary>
    /// Prototype bounced email processor.
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            var queueClient = new AmazonBouncedEmailQueueClient();
            var apiClient = new CoachseekAdminApiClient(new AdminApiClient());

            var useCase = new ProcessBouncedEmailMessagesUseCase(queueClient, apiClient);
            useCase.Process();
        }
    }
}
