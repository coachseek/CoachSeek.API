using CoachSeek.Api.Attributes;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.UseCases;
using CoachSeek.DataAccess.Authentication.Repositories;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;
using CoachSeek.Services.Builders;
using CoachSeek.Services.Contracts.Builders;
using CoachSeek.Services.Contracts.Email;
using CoachSeek.Services.Email;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using StructureMap.Attributes;
using StructureMap.Building;
using StructureMap.Configuration.DSL;

namespace CoachSeek.Api
{
    public class TypeRegistry : Registry
    {
        public TypeRegistry()
        {
            For<IUserRepository>().Use<InMemoryUserRepository>();
            For<IBusinessRepository>().Use<InMemoryBusinessRepository>();
            For<IReservedDomainRepository>().Use<HardCodedReservedDomainRepository>();

            For<IBusinessDomainBuilder>().Use<BusinessDomainBuilder>();
            For<IBusinessRegistrationEmailer>().Use<StubBusinessRegistrationEmailer>();

            For<IBusinessGetByDomainUseCase>().Use<BusinessGetByDomainUseCase>();

            For<IBusinessNewRegistrationUseCase>().Use<BusinessNewRegistrationUseCase>();
            For<ILocationAddUseCase>().Use<LocationAddUseCase>();
            For<ILocationUpdateUseCase>().Use<LocationUpdateUseCase>();
            For<ICoachAddUseCase>().Use<CoachAddUseCase>();
            For<ICoachUpdateUseCase>().Use<CoachUpdateUseCase>();

            For<IServiceAddUseCase>().Use<ServiceAddUseCase>();
            For<IServiceUpdateUseCase>().Use<ServiceUpdateUseCase>();

            For<ISessionAddUseCase>().Use<SessionAddUseCase>();
            For<ISessionUpdateUseCase>().Use<SessionUpdateUseCase>();

            For<IUserStore<User>>().Use<UserStore>();
        }
    }
}