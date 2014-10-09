using CoachSeek.WebUI.Builders;
using CoachSeek.WebUI.Contracts.Builders;
using CoachSeek.WebUI.Contracts.Email;
using CoachSeek.WebUI.Contracts.Persistence;
using CoachSeek.WebUI.Contracts.UseCases;
using CoachSeek.WebUI.Email;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;
using StructureMap.Configuration.DSL;

namespace CoachSeek.WebUI
{
    public class TypeRegistry : Registry
    {
        public TypeRegistry()
        {
            For<IBusinessRepository>().Use<InMemoryBusinessRepository>();
            For<IReservedDomainRepository>().Use<HardCodedReservedDomainRepository>();

            For<IBusinessDomainBuilder>().Use<BusinessDomainBuilder>();
            For<IBusinessRegistrationEmailer>().Use<StubBusinessRegistrationEmailer>();

            For<IBusinessNewRegistrationUseCase>().Use<BusinessNewRegistrationUseCase>();
            For<ILocationAddUseCase>().Use<LocationAddUseCase>();
            For<ILocationUpdateUseCase>().Use<LocationUpdateUseCase>();
            For<ICoachAddUseCase>().Use<CoachAddUseCase>();
            For<ICoachUpdateUseCase>().Use<CoachUpdateUseCase>();
            For<IBusinessGetByDomainUseCase>().Use<BusinessGetByDomainUseCase>();
        }
    }
}