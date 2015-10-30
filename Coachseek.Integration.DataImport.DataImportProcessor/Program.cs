using System.Timers;
using CoachSeek.Application.Contracts.UseCases.DataImport;
using StructureMap;

namespace Coachseek.Integration.DataImport.DataImportProcessor
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Container(new TypeRegistry());
            var useCase = container.GetInstance<IProcessDataImportUseCase>();
            useCase.ProcessAsync().Wait();
        }
    }
}
