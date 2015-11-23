using System.Collections.Generic;
using Coachseek.Infrastructure.Queueing.Amazon;
using Coachseek.Infrastructure.Queueing.Contracts.Emailing;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Coachseek.Integration.Emailing.BouncedEmailProcessor
{
    public class TypeRegistry : Registry
    {
        public TypeRegistry()
        {
            var assemblyNames = new List<string>
                {
                    "Coachseek.API.Client",
                    "CoachSeek.Application",
                    "CoachSeek.Application.Contracts",
                    "CoachSeek.DataAccess.Main.Memory",
                    "Coachseek.Integration.Contracts",
                };

            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(f => assemblyNames.Contains(f.GetName().Name));
                x.WithDefaultConventions();
            });

            For<IBouncedEmailQueueClient>().Use<AmazonBouncedEmailQueueClient>();
        }
    }
}