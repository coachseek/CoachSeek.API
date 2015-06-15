using System.Timers;
using Coachseek.API.Client.Services;
using CoachSeek.Application.UseCases.Emailing;
using Coachseek.Infrastructure.Queueing.Amazon;

namespace Coachseek.Integration.Emailing.BouncedEmailProcessor
{
    class Program
    {
        private const int MILLISECONDS_IN_SECOND = 1000;

        static Timer Timer { get; set; }

        static void Main(string[] args)
        {
            SetTimer();
            Timer.Start();

            while (true)
            { }
        }


        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var queueClient = new AmazonBouncedEmailQueueClient();
            var apiClient = new CoachseekAdminApiClient(new AdminApiClient());

            var useCase = new ProcessBouncedEmailMessagesUseCase(queueClient, apiClient);
            useCase.Process();

            SetTimer(60);
            Timer.Start();
        }

        private static void SetTimer(int numberOfSeconds = 1)
        {
            Timer = new Timer { Interval = numberOfSeconds * MILLISECONDS_IN_SECOND, AutoReset = false };
            Timer.Elapsed += TimerElapsed;
        }
    }
}
