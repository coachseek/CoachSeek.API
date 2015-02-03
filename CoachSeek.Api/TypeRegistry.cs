using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.UseCases;
using CoachSeek.DataAccess.Authentication.Repositories;
using Coachseek.DataAccess.Authentication.TableStorage;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Repositories;
using CoachSeek.Services.Builders;
using CoachSeek.Services.Contracts.Builders;
using CoachSeek.Services.Contracts.Email;
using CoachSeek.Services.Email;
using StructureMap.Configuration.DSL;

namespace CoachSeek.Api
{
    public class TypeRegistry : Registry
    {
        public TypeRegistry()
        {
            For<IUserRepository>().Use<AzureTableUserRepository>();
            //For<IUserRepository>().Use<InMemoryUserRepository>();
            For<IBusinessRepository>().Use<InMemoryBusinessRepository>();
            For<IReservedDomainRepository>().Use<HardCodedReservedDomainRepository>();

            For<IBusinessDomainBuilder>().Use<BusinessDomainBuilder>();
            For<IBusinessRegistrationEmailer>().Use<StubBusinessRegistrationEmailer>();

            For<IBusinessGetByDomainUseCase>().Use<BusinessGetByDomainUseCase>();

            For<IBusinessAddUseCase>().Use<BusinessAddUseCase>();

            For<ILocationsGetAllUseCase>().Use<LocationsGetAllUseCase>();
            For<ILocationGetByIdUseCase>().Use<LocationGetByIdUseCase>();
            For<ILocationAddUseCase>().Use<LocationAddUseCase>();
            For<ILocationUpdateUseCase>().Use<LocationUpdateUseCase>();

            For<ICoachesGetAllUseCase>().Use<CoachesGetAllUseCase>();
            For<ICoachGetByIdUseCase>().Use<CoachGetByIdUseCase>();
            For<ICoachAddUseCase>().Use<CoachAddUseCase>();
            For<ICoachUpdateUseCase>().Use<CoachUpdateUseCase>();

            For<IServicesGetAllUseCase>().Use<ServicesGetAllUseCase>();
            For<IServiceGetByIdUseCase>().Use<ServiceGetByIdUseCase>();
            For<IServiceAddUseCase>().Use<ServiceAddUseCase>();
            For<IServiceUpdateUseCase>().Use<ServiceUpdateUseCase>();

            For<ISessionSearchUseCase>().Use<SessionSearchUseCase>();
            For<ISessionAddUseCase>().Use<SessionAddUseCase>();
            For<ISessionUpdateUseCase>().Use<SessionUpdateUseCase>();

            For<ICustomerGetByIdUseCase>().Use<CustomerGetByIdUseCase>();
            For<ICustomerAddUseCase>().Use<CustomerAddUseCase>();
            For<ICustomerUpdateUseCase>().Use<CustomerUpdateUseCase>();

            For<IUserAddUseCase>().Use<UserAddUseCase>();
            For<IUserAssociateWithBusinessUseCase>().Use<UserAssociateWithBusinessUseCase>();
        }
    }
}