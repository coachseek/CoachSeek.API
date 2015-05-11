using System.Collections.Generic;
using CoachSeek.Application.Contracts.Services.Emailing;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Repositories;
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
                    "CoachSeek.Domain"
                };

            Scan(x =>
            {
                x.AssembliesFromApplicationBaseDirectory(f => assemblyNames.Contains(f.GetName().Name));
                x.WithDefaultConventions();
            });
            
            For<IReservedDomainRepository>().Use<HardCodedReservedDomainRepository>();
            //For<IBusinessRegistrationEmailer>().Use<StubBusinessRegistrationEmailer>();
            //For<IOnlineBookingEmailer>().Use<StubOnlineBookingEmailer>();
        }
    }
}