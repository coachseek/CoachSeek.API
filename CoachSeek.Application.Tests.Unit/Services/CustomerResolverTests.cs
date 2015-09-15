using System;
using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.Services;
using CoachSeek.Common;
using CoachSeek.DataAccess.Authentication.Repositories;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.Domain.Entities;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.Services
{
    [TestFixture]
    public class CustomerResolverTests
    {
        private ApplicationContext Context { get; set; }
        private Guid BusinessId { get; set; }
        private Guid UserId { get; set; }
        private InMemoryBusinessRepository BusinessRepository { get; set; }
        private InMemoryUserRepository UserRepository { get; set; }


        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ApplicationAutoMapperConfigurator.Configure();
            DbAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            SetupApplicationContext();
        }

        private void SetupApplicationContext()
        {
            SetupBusinessRepository();
            SetupUserRepository();

            var business = new BusinessDetails(BusinessId, "", "", DateTime.UtcNow.AddDays(1));
            var currency = new CurrencyDetails("NZD", "$");
            var businessContext = new BusinessContext(business, currency, BusinessRepository, null, UserRepository);
            Context = new ApplicationContext(null, businessContext, null, null, true);
        }


        [Test]
        public void ResolveTests()
        {
            AssertCustomerMatched("Fred", "Flintstone", "fred@flintstones.net", "123456");
            AssertCustomerMatched("Fred", "Flintstone", "fred@flintstones.net", null);
            AssertCustomerMatched("Fred", "Flintstone", "fred@flintstones.net", "666 999");
            AssertCustomerMatched(" fred", "flintstone ", " Fred@Flintstones.net ", null);
            AssertCustomerMatched(" fred", "flintstone ", " Fred@Flintstones.net ", "888");

            AssertCustomerNotMatched("Fred", "Flintstone", "fred@flintstones.com", "123456");
            AssertCustomerNotMatched("Fred", "Flintstone", null, null);
            AssertCustomerNotMatched("bob", "Flintstone", "fred@flintstones.com", "0800 HOT CHICKS");
            AssertCustomerNotMatched("Fred", "Holder", "fred@flintstones.com", "123456");
        }


        private void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();

            BusinessId = new Guid("111D234C-9627-4F9B-8552-8BE0AB28A558");
            var business = new NewBusiness(BusinessId, 
                                        "Ian's Tennis Academy",
                                        "ianstennisacademy", "NZD");
            BusinessRepository.AddBusiness(business);
            var customer = new Customer(new Guid("8A23F42D-9F6B-46FD-9922-3DA0E05B1A72"), 
                                        "Fred", "Flintstone", 
                                        "fred@flintstones.net", "123456");
            BusinessRepository.AddCustomer(business.Id, customer);
        }

        private void SetupUserRepository()
        {
            UserRepository = new InMemoryUserRepository();

            UserId = new Guid("3CE43207-4A4F-48B0-BED3-F831A3246DF3");
            var user = new User(UserId, 
                                BusinessId, 
                                "Ian's Tennis Academy", 
                                "bob.smith@test.com", 
                                "021 666 999", 
                                "Bob", 
                                "Smith", 
                                "bob.smith@test.com", 
                                "");
            UserRepository.Add(user);
        }


        private void AssertCustomerMatched(string firstName, string lastName, string email, string phone)
        {
            var resolver = new CustomerResolver();
            resolver.Initialise(Context);
            var searchCustomer = new Customer(Guid.Empty, firstName, lastName, email, phone);
            var customer = resolver.Resolve(searchCustomer);
            Assert.That(customer, Is.Not.Null);
        }

        private void AssertCustomerNotMatched(string firstName, string lastName, string email, string phone)
        {
            var resolver = new CustomerResolver();
            resolver.Initialise(Context);
            var searchCustomer = new Customer(Guid.Empty, firstName, lastName, email, phone);
            var customer = resolver.Resolve(searchCustomer);
            Assert.That(customer, Is.Null);
        }
    }
}
