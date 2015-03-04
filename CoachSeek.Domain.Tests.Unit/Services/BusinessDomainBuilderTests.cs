using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Main.Memory.Configuration;
using CoachSeek.DataAccess.Main.Memory.Repositories;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Repositories;
using CoachSeek.Domain.Services;
using NUnit.Framework;
using System;

namespace CoachSeek.Domain.Tests.Unit.Services
{
    [TestFixture]
    public class BusinessDomainBuilderTests
    {
        private IReservedDomainRepository ReservedDomainRepository { get; set; }
        private InMemoryBusinessRepository BusinessRepository { get; set; }


        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            DbAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            SetupReservedDomainRepository();
            SetupBusinessRepository();
        }

        private void SetupReservedDomainRepository()
        {
            ReservedDomainRepository = new HardCodedReservedDomainRepository();
        }

        private void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();

            BusinessRepository.AddBusiness(new Business2(Guid.NewGuid(), "Ian's Cafe", "ianscafe"));
            BusinessRepository.AddBusiness(new Business2(Guid.NewGuid(), "Bob's Burgers", "bobsburgers"));
            BusinessRepository.AddBusiness(new Business2(Guid.NewGuid(), "Bobs Burgers", "bobsburgers1"));
            BusinessRepository.AddBusiness(new Business2(Guid.NewGuid(), "Bob's Burgers #1", "bobsburgers2"));
        }

        private BusinessAdminData SetupBusinessAdmin()
        {
            return new BusinessAdminData
            {
                FirstName = "Bob", 
                LastName = "Smith", 
                Email = "bob@smith.com",
                Username = "bob@smith.com",
                PasswordHash = "password"
            };
        }

        [Test]
        public void GivenBusinessNameIsNull_WhenBuildDomain_ThenThrowArgumentNullException()
        {
            var businessName = GivenBusinessNameIsNull();
            var response = WhenBuildDomain(businessName);
            ThenThrowArgumentNullException(response);
        }

        [Test]
        public void GivenBusinessNameIsEmpty_WhenBuildDomain_ThenThrowArgumentNullException()
        {
            var businessName = GivenBusinessNameIsEmpty();
            var response = WhenBuildDomain(businessName);
            ThenThrowArgumentNullException(response);
        }

        [Test]
        public void GivenBusinessNameIsOnlyWhitespace_WhenBuildDomain_ThenThrowArgumentNullException()
        {
            var businessName = GivenBusinessNameIsOnlyWhitespace();
            var response = WhenBuildDomain(businessName);
            ThenThrowArgumentNullException(response);
        }

        [Test]
        public void GivenUpperCaseCharactersInBusinessName_WhenBuildDomain_ThenReturnLowerCaseBusinessName()
        {
            var businessName = GivenUpperCaseCharactersInBusinessName();
            var response = WhenBuildDomain(businessName);
            ThenReturnLowerCaseBusinessName(response);
        }

        [Test]
        public void GivenWhitespaceInBusinessName_WhenBuildDomain_ThenReturnBusinessNameWithWhitespaceRemoved()
        {
            var businessName = GivenWhitespaceInBusinessName();
            var response = WhenBuildDomain(businessName);
            ThenReturnBusinessNameWithWhitespaceRemoved(response);
        }

        [Test]
        public void GivenNonAlphaNumericCharactersInBusinessName_WhenBuildDomain_ThenReturnBusinessNameWithNonAlphaNumericCharactersRemoved()
        {
            var businessName = GivenNonAlphaNumericCharactersInBusinessName();
            var response = WhenBuildDomain(businessName);
            ThenReturnBusinessNameWithNonAlphaNumericCharactersRemoved(response);
        }

        [Test]
        public void GivenABusinessWhereTheDomainNameAlreadyExists_WhenBuildDomain_ThenReturnDomainNameWithAppendedNumberOne()
        {   
            var businessName = GivenABusinessWhereTheDomainNameAlreadyExists();
            var response = WhenBuildDomain(businessName);
            ThenReturnDomainNameWithAppendedNumberOne(response);
        }

        [Test]
        public void GivenABusinessWhereTheDomainNameAlreadyExistsMoreThanOnce_WhenBuildDomain_ThenReturnDomainNameWithAppendedIncrementalNumber()
        {
            var businessName = GivenABusinessWhereTheDomainNameAlreadyExistsMoreThanOnce();
            var response = WhenBuildDomain(businessName);
            ThenReturnDomainNameWithAppendedIncrementalNumber(response);
        }

        [Test]
        public void GivenABusinessWhereTheDomainNameIsReserved_WhenBuildDomain_ThenReturnReservedDomainNameWithAppendedNumberOne()
        {
            var businessName = GivenABusinessWhereTheDomainNameIsReserved();
            var response = WhenBuildDomain(businessName);
            ThenReturnReservedDomainNameWithAppendedNumberOne(response);
        }


        private string GivenBusinessNameIsNull()
        {
            return null;
        }

        private string GivenBusinessNameIsEmpty()
        {
            return "";
        }

        private string GivenBusinessNameIsOnlyWhitespace()
        {
            return "   ";
        }

        private string GivenUpperCaseCharactersInBusinessName()
        {
            return "Amazon";
        }

        private string GivenWhitespaceInBusinessName()
        {
            return "Auckland \tUrgent  Couriers \n";
        }

        private string GivenNonAlphaNumericCharactersInBusinessName()
        {
            return "Olaf's #1 Cafe!";
        }

        private string GivenABusinessWhereTheDomainNameAlreadyExists()
        {
            return "Ian's Cafe!";
        }

        private string GivenABusinessWhereTheDomainNameAlreadyExistsMoreThanOnce()
        {
            return "Bob's Burgers";
        }

        private string GivenABusinessWhereTheDomainNameIsReserved()
        {
            return "Tennis";
        }


        private object WhenBuildDomain(string businessName)
        {
            var builder = new BusinessDomainBuilder(ReservedDomainRepository) {BusinessRepository = BusinessRepository};
            try
            {
                return builder.BuildDomain(businessName);
            }
            catch(Exception ex)
            {
                return ex;
            }
        }

        private void ThenThrowArgumentNullException(object response)
        {
            Assert.That(response, Is.InstanceOf(typeof(ArgumentNullException)));
        }

        private void ThenReturnLowerCaseBusinessName(object response)
        {
            Assert.That(response, Is.InstanceOf(typeof(string)));
            var domain = response as string;
            Assert.That(domain, Is.EqualTo("amazon"));
        }

        private void ThenReturnBusinessNameWithWhitespaceRemoved(object response)
        {
            Assert.That(response, Is.InstanceOf(typeof(string)));
            var domain = response as string;
            Assert.That(domain, Is.EqualTo("aucklandurgentcouriers"));
        }

        private void ThenReturnBusinessNameWithNonAlphaNumericCharactersRemoved(object response)
        {
            Assert.That(response, Is.InstanceOf(typeof(string)));
            var domain = response as string;
            Assert.That(domain, Is.EqualTo("olafs1cafe"));
        }

        private void ThenReturnDomainNameWithAppendedNumberOne(object response)
        {
            Assert.That(response, Is.InstanceOf(typeof(string)));
            var domain = response as string;
            Assert.That(domain, Is.EqualTo("ianscafe1"));
        }

        private void ThenReturnDomainNameWithAppendedIncrementalNumber(object response)
        {
            Assert.That(response, Is.InstanceOf(typeof(string)));
            var domain = response as string;
            Assert.That(domain, Is.EqualTo("bobsburgers3"));
        }

        private void ThenReturnReservedDomainNameWithAppendedNumberOne(object response)
        {
            Assert.That(response, Is.InstanceOf(typeof(string)));
            var domain = response as string;
            Assert.That(domain, Is.EqualTo("tennis1"));
        }
    }
}
