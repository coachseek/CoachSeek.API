using System.Collections.Generic;
using Coachseek.API.Client.Interfaces;
using Coachseek.API.Client.Services;
using Coachseek.Infrastructure.Queueing.Amazon;
using Coachseek.Infrastructure.Queueing.Contracts.Emailing;
using Coachseek.Infrastructure.Queueing.Contracts.Payment;
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