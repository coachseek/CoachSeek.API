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

            var business = new Business(BusinessId, "", "abc123", "NZD", "$", "", DateTime.UtcNow.AddDays(1), "Trial");
            var businessContext = new BusinessContext(business, BusinessRepository, UserRepository);
            Context = new ApplicationContext(null, businessContext, null, null, true);
        }


        [Test]
        public void ResolveTests()
        {
            AssertCustomerMatched("Fred", "Flintstone", "fred@flintstones.net", "123456", "male");
            AssertCustomerMatched("Fred", "Flintstone", "fred@flintstones.net", null, "male");
            AssertCustomerMatched("Fred", "Flintstone", "fred@flintstones.net", "666 999", "male");
            AssertCustomerMatched(" fred", "flintstone ", " Fred@Flintstones.net ", null, "male");
            AssertCustomerMatched(" fred", "flintstone ", " Fred@Flintstones.net ", "888", "male");

            AssertCustomerNotMatched("Fred", "Flintstone", "fred@flintstones.com", "123456", null);
            AssertCustomerNotMatched("Fred", "Flintstone", null, null, null);
            AssertCustomerNotMatched("bob", "Flintstone", "fred@flintstones.com", "0800 HOT CHICKS", null);
            AssertCustomerNotMatched("Fred", "Holder", "fred@flintstones.com", "123456", null);
        }


        private void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();

            BusinessId = new Guid("111D234C-9627-4F9B-8552-8BE0AB28A558");
            var business = new NewBusiness(BusinessId, 
                                        "Ian's Tennis Academy",
                                        "ianstennisacademy", "NZD", "$");
            BusinessRepository.AddBusiness(business);
            var customer = new Customer(new Guid("8A23F42D-9F6B-46FD-9922-3DA0E05B1A72"), 
                                        "Fred", "Flintstone", 
                                        "fred@flintstones.net", "123456", "male");
            BusinessRepository.AddCustomerAsync(business.Id, customer).Wait();
        }

        private void SetupUserRepository()
        {
            UserRepository = new InMemoryUserRepository();

            UserId = new Guid("3CE43207-4A4F-48B0-BED3-F831A3246DF3");
            var user = new User(UserId, 
                                BusinessId, 
                                "Ian's Tennis Academy",
                                Role.BusinessAdmin.ToString(),
                                "bob.smith@test.com", 
                                "021 666 999", 
                                "Bob", 
                                "Smith", 
                                "bob.smith@test.com", 
                                "");
            UserRepository.Add(user);
        }


        private void AssertCustomerMatched(string firstName, string lastName, string email, string phone, string sex)
        {
            var resolver = new CustomerResolver();
            resolver.Initialise(Context);
            var searchCustomer = new Customer(Guid.Empty, firstName, lastName, email, phone, sex);
            var customer = resolver.ResolveAsync(searchCustomer).Result;
            Assert.That(customer, Is.Not.Null);
        }

        private void AssertCustomerNotMatched(string firstName, string lastName, string email, string phone, string sex)
        {
            var resolver = new CustomerResolver();
            resolver.Initialise(Context);
            var searchCustomer = new Customer(Guid.Empty, firstName, lastName, email, phone, sex);
            var customer = resolver.ResolveAsync(searchCustomer).Result;
            Assert.That(customer, Is.Null);
        }
    }
}
