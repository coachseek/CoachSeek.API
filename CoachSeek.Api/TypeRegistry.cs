using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Contracts.UseCases.Factories;
using CoachSeek.Application.UseCases;
using CoachSeek.Application.UseCases.Factories;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Contracts;
using CoachSeek.Domain.Repositories;
using CoachSeek.Domain.Services;
using StructureMap.Configuration.DSL;

namespace CoachSeek.Api
{
    public class TypeRegistry : Registry
    {
        public TypeRegistry()
        {
            For<IReservedDomainRepository>().Use<HardCodedReservedDomainRepository>();

            For<IBusinessDomainBuilder>().Use<BusinessDomainBuilder>();
            For<IBusinessRegistrationEmailer>().Use<StubBusinessRegistrationEmailer>();

            //For<IBusinessGetByDomainUseCase>().Use<BusinessGetByDomainUseCase>();

            For<IBusinessAddUseCase>().Use<BusinessAddUseCase>();

            For<ILocationsGetAllUseCase>().Use<LocationsGetAllUseCase>();
            For<ILocationGetByIdUseCase>().Use<LocationGetByIdUseCase>();
            For<ILocationAddUseCase>().Use<LocationAddUseCase>();
            For<ILocationUpdateUseCase>().Use<LocationUpdateUseCase>();
            For<ILocationDeleteUseCase>().Use<LocationDeleteUseCase>();

            For<ICoachesGetAllUseCase>().Use<CoachesGetAllUseCase>();
            For<ICoachGetByIdUseCase>().Use<CoachGetByIdUseCase>();
            For<ICoachAddUseCase>().Use<CoachAddUseCase>();
            For<ICoachUpdateUseCase>().Use<CoachUpdateUseCase>();
            For<ICoachDeleteUseCase>().Use<CoachDeleteUseCase>();

            For<IServicesGetAllUseCase>().Use<ServicesGetAllUseCase>();
            For<IServiceGetByIdUseCase>().Use<ServiceGetByIdUseCase>();
            For<IServiceAddUseCase>().Use<ServiceAddUseCase>();
            For<IServiceUpdateUseCase>().Use<ServiceUpdateUseCase>();
            For<IServiceDeleteUseCase>().Use<ServiceDeleteUseCase>();

            For<ISessionSearchUseCase>().Use<SessionSearchUseCase>();
            For<ISessionGetByIdUseCase>().Use<SessionGetByIdUseCase>();
            For<ISessionAddUseCase>().Use<SessionAddUseCase>();
            For<ISessionAddUseCase>().Use<SessionAddUseCase>();
            For<ISessionUpdateUseCase>().Use<SessionUpdateUseCase>();
            For<ISessionDeleteUseCase>().Use<SessionDeleteUseCase>();

            For<ICustomersGetAllUseCase>().Use<CustomersGetAllUseCase>();
            For<ICustomerGetByIdUseCase>().Use<CustomerGetByIdUseCase>();
            For<ICustomerAddUseCase>().Use<CustomerAddUseCase>();
            For<ICustomerUpdateUseCase>().Use<CustomerUpdateUseCase>();

            For<IBookingGetByIdUseCase>().Use<BookingGetByIdUseCase>();
            For<IBookingAddUseCase>().Use<BookingAddUseCase>();
            For<IBookingAddUseCaseFactory>().Use<BookingAddUseCaseFactory>();
            For<IBookingDeleteUseCase>().Use<BookingDeleteUseCase>();

            For<IUserAddUseCase>().Use<UserAddUseCase>();
            For<IUserAssociateWithBusinessUseCase>().Use<UserAssociateWithBusinessUseCase>();

            For<IUseCaseFactory>().Use<UseCaseFactory>();
        }
    }
}