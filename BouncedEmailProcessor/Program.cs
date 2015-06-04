using Coachseek.API.Client;
using Coachseek.API.Client.Services;

namespace BouncedEmailProcessor
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
