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
            var container = new Container(new TypeRegistry());
            var useCase = container.GetInstance<IProcessDataImportUseCase>();
            useCase.Process();
        }
    }
}
