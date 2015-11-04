using System.Timers;
using CoachSeek.Application.Contracts.UseCases.DataImport;
using CoachSeek.Common.Extensions;
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


        protected string EmailSender
        {
            get { return AppSettings.EmailSender; }
        }

        protected bool IsEmailingEnabled
        {
            get { return AppSettings.IsEmailingEnabled.Parse(true); }
        }
    }
}
