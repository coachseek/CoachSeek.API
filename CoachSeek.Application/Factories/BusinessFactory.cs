//using CoachSeek.Domain.Commands;
//using CoachSeek.Domain.Entities;
//using CoachSeek.Services.Contracts.Builders;

//namespace CoachSeek.Application.Factories
//{
//    public static class BusinessFactory
//    {
//        public static NewBusiness Create(BusinessRegistrationCommand registrationCommand, IBusinessDomainBuilder domainBuilder)
//        {
//            return new NewBusiness
//            {
//                Name = registrationCommand.BusinessName,
//                Domain = domainBuilder.BuildDomain(registrationCommand.BusinessName),
//                Admin = CreateBusinessAdmin(registrationCommand.Registrant)
//            };
//        }


//        private static BusinessAdmin CreateBusinessAdmin(BusinessRegistrant registrant)
//        {
//            return new BusinessAdmin(registrant.FirstName, registrant.LastName, registrant.Email, registrant.Password);
//        }
//    }
//}