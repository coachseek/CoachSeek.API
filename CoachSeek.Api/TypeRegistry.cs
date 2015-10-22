using System.Collections.Generic;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Repositories;
using Coachseek.Infrastructure.Queueing.Azure;
using Coachseek.Infrastructure.Queueing.Contracts.Import;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using Coachseek.Integration.Contracts.UserTracking;
using Coachseek.Integration.UserTracking;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace CoachSeek.Api
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
                    "Coachseek.DataAccess.Main.SqlServer",
                    "CoachSeek.DataAccess.TableStorage",
                    "CoachSeek.Domain"
                };

            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(f => assemblyNames.Contains(f.GetName().Name));
                x.WithDefaultConventions();
            });

            For<IReservedDomainRepository>().Use<HardCodedReservedDomainRepository>();
            For<IOnlinePaymentProcessingQueueClient>().Use<AzureOnlinePaymentProcessingQueueClient>();
            For<ISubscriptionPaymentProcessingQueueClient>().Use<AzureSubscriptionPaymentProcessingQueueClient>();
            For<IDataImportQueueClient>().Use<AzureDataImportQueueClient>();
            For<IUserTrackerFactory>().Use<UserTrackerFactory>();
        }
    }
}