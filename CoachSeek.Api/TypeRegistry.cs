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

            For<IBusinessAddUseCase>().Use<BusinessAddUseCase>();
            For<ILocationAddUseCase>().Use<LocationAddUseCase>();
            For<ILocationUpdateUseCase>().Use<LocationUpdateUseCase>();
            For<ICoachAddUseCase>().Use<CoachAddUseCase>();
            For<ICoachUpdateUseCase>().Use<CoachUpdateUseCase>();

            For<IServiceAddUseCase>().Use<ServiceAddUseCase>();
            For<IServiceUpdateUseCase>().Use<ServiceUpdateUseCase>();

            For<ISessionSearchUseCase>().Use<SessionSearchUseCase>();
            For<ISessionAddUseCase>().Use<SessionAddUseCase>();
            For<ISessionUpdateUseCase>().Use<SessionUpdateUseCase>();

            For<IUserAddUseCase>().Use<UserAddUseCase>();
            For<IUserAssociateWithBusinessUseCase>().Use<UserAssociateWithBusinessUseCase>();
        }
    }
}