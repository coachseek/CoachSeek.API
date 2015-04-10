using System.Collections.Generic;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;
using CoachSeek.Domain.Services;
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
            For<IBusinessRegistrationEmailer>().Use<StubBusinessRegistrationEmailer>();
        }
    }
}