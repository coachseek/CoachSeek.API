using System.Timers;
using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using StructureMap;

namespace Coachseek.Integration.Payments.PaymentsProcessor
{
    class Program
    {
        private const int MILLISECONDS_IN_SECOND = 1000;

        static Timer Timer { get; set; }

        static void Main(string[] args)
        {
            ApplicationAutoMapperConfigurator.Configure();
            DbAutoMapperConfigurator.Configure();

            SetTimer();
            Timer.Start();

            while (true)
            { }
        }


        private static void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            var container = new Container(new TypeRegistry());
            var useCase = container.GetInstance<IProcessPaymentsUseCase>();

            useCase.Process();

            //SetTimer(60);
            //Timer.Start();
        }

        private static void SetTimer(int numberOfSeconds = 1)
        {
            Timer = new Timer { Interval = numberOfSeconds * MILLISECONDS_IN_SECOND, AutoReset = false };
            Timer.Elapsed += TimerElapsed;
        }
    }
}
