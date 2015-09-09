using System.Timers;
using CoachSeek.Application.Contracts.UseCases.DataImport;
using StructureMap;

namespace Coachseek.Integration.DataImport.DataImportProcessor
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
            var container = new Container(new TypeRegistry());
            var useCase = container.GetInstance<IProcessDataImportUseCase>();

            useCase.Process();

            SetTimer(20);
            Timer.Start();
        }

        private static void SetTimer(int numberOfSeconds = 1)
        {
            Timer = new Timer { Interval = numberOfSeconds * MILLISECONDS_IN_SECOND, AutoReset = false };
            Timer.Elapsed += TimerElapsed;
        }
    }
}
