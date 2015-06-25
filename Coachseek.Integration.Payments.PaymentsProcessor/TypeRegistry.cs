using System.Collections.Generic;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Repositories;
using Coachseek.Infrastructure.Queueing.Azure;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Coachseek.Integration.Payments.PaymentsProcessor
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
                    "Coachseek.Integration.Payments",
                    "Coachseek.Integration.Payments.PaymentsProcessor",
                    "Coachseek.Integration.Contracts"
                };

            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(f => assemblyNames.Contains(f.GetName().Name));
                x.WithDefaultConventions();
            });

            For<IPaymentProcessingQueueClient>().Use<AzurePaymentProcessingQueueClient>();
        }
    }
}