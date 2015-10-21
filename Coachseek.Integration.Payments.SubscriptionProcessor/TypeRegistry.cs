using System.Collections.Generic;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Coachseek.Integration.Payments.SubscriptionProcessor
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
                    "Coachseek.Integration.Payments.SubscriptionProcessor",
                    "Coachseek.Integration.Contracts"
                };

            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(f => assemblyNames.Contains(f.GetName().Name));
                x.WithDefaultConventions();
            });

            //For<IPaymentProcessingQueueClient>().Use<AzurePaymentProcessingQueueClient>();
        }
    }
}