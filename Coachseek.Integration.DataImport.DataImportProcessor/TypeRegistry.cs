using System.Collections.Generic;
using CoachSeek.Application.Contracts.UseCases.DataImport;
using CoachSeek.Application.UseCases.DataImport;
using Coachseek.Infrastructure.Queueing.Azure;
using Coachseek.Infrastructure.Queueing.Contracts.Import;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Coachseek.Integration.DataImport.DataImportProcessor
{
    public class TypeRegistry : Registry
    {
        public TypeRegistry()
        {
            var assemblyNames = new List<string>
                {
                    "CoachSeek.Application",
                    "CoachSeek.Application.Contracts",
                    "CoachSeek.DataAccess.Main.Memory",
                    "Coachseek.Integration.DataImport",
                    "Coachseek.Integration.DataImport.DataImportProcessor",
                    "Coachseek.Integration.Contracts",
                    "Coachseek.Infrastructure.Queueing.Contracts.Import",
                    "Coachseek.Infrastructure.Queueing.Azure"
                };

            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(f => assemblyNames.Contains(f.GetName().Name));
                x.WithDefaultConventions();
            });

            For<IProcessDataImportUseCase>().Use<ProcessDataImportUseCase>();
            For<IDataImportQueueClient>().Use<AzureDataImportQueueClient>();
        }
    }
}
