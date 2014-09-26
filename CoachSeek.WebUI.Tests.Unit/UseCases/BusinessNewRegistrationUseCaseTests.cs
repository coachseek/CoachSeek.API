using CoachSeek.WebUI.Builders;
using CoachSeek.WebUI.Email;
using CoachSeek.WebUI.Models;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Models.Responses;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;
using NUnit.Framework;
using System.Linq;

namespace CoachSeek.WebUI.Tests.Unit.UseCases
{
    [TestFixture]
    public class BusinessNewRegistrationUseCaseTests
    {
        private InMemoryBusinessRepository BusinessRepository { get; set; }
        private InMemoryBusinessAdminRepository BusinessAdminRepository { get; set; }
        private BusinessDomainBuilder BusinessDomainBuilder { get; set; }
        private StubBusinessRegistrationEmailer BusinessRegistrationEmailer { get; set; }
        
        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();
            SetupBusinessAdminRepository();
            SetupBusinessDomainBuilder();
            SetupBusinessRegistrationEmailer();
        }

        private void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();
            BusinessRepository.Add(new Business { Id = 1, Name = "Olaf's Bookshoppe", Domain = "olafsbookshoppe" });
            BusinessRepository.WasAddCalled = false;
        }

        private void SetupBusinessAdminRepository()
        {
            BusinessAdminRepository = new InMemoryBusinessAdminRepository();
            BusinessAdminRepository.Add(new BusinessAdmin { Id = 1, 
                                                            BusinessId = 1, 
                                                            FirstName = "Olaf", 
                                                            LastName = "Thielke",
                                                            Email = "olaft@ihug.co.nz",
                                                            Password = "abc123"
                                                          });
            BusinessAdminRepository.WasAddCalled = false;
        }

        private void SetupBusinessDomainBuilder()
        {
            BusinessDomainBuilder = new BusinessDomainBuilder(new HardCodedReservedDomainRepository(), BusinessRepository);
        }

        private void SetupBusinessRegistrationEmailer()
        {
            BusinessRegistrationEmailer = new StubBusinessRegistrationEmailer();
        }


        [Test]
        public void GivenNoRegistration_WhenRegisterANewBusiness_ThenTheBusinessRegistrationFailsWithMissingRegistrationError()
        {
            var registration = GivenNoRegistration();
            var response = WhenRegisterANewBusiness(registration);
            ThenTheBusinessRegistrationFailsWithMissingRegistrationError(response);
        }

        [Test]
        public void GivenAnExistingEmailAddress_WhenRegisterANewBusiness_ThenTheBusinessRegistrationFailsWithDupliateEmailError()
        {
            var registration = GivenAnExistingEmailAddress();
            var response = WhenRegisterANewBusiness(registration);
            ThenTheBusinessRegistrationFailsWithDupliateEmailError(response);
        }

        [Test]
        public void GivenAUniqueEmailAddress_WhenRegisterANewBusiness_ThenTheBusinessRegistrationSucceeds()
        {
            var registration = GivenAUniqueEmailAddress();
            var response = WhenRegisterANewBusiness(registration);
            ThenTheBusinessRegistrationSucceeds(response);
        }

        private BusinessRegistrationRequest GivenNoRegistration()
        {
            return null;
        }

        private BusinessRegistrationRequest GivenAnExistingEmailAddress()
        {
            return new BusinessRegistrationRequest
            {
                BusinessName = "Olaf's Cafe",
                Registrant = new BusinessRegistrant
                {
                    FirstName = "Olaf",
                    LastName = "Thielke",
                    Email = "olaft@ihug.co.nz",
                    Password = "password"
                }
            };
        }

        private BusinessRegistrationRequest GivenAUniqueEmailAddress()
        {
            return new BusinessRegistrationRequest
            {
                BusinessName = "Ian's Cafe",
                Registrant = new BusinessRegistrant
                {
                    FirstName = "Ian",
                    LastName = "Bishop",
                    Email = "ianbish@gmail.com",
                    Password = "password"
                }
            };
        }

        private BusinessRegistrationResponse WhenRegisterANewBusiness(BusinessRegistrationRequest registration)
        {
            var useCase = new BusinessNewRegistrationUseCase(BusinessRepository, 
                BusinessAdminRepository,
                BusinessDomainBuilder,
                BusinessRegistrationEmailer);

            return useCase.RegisterNewBusiness(registration);
        }

        private void ThenTheBusinessRegistrationFailsWithMissingRegistrationError(BusinessRegistrationResponse response)
        {
            AssertMissingRegistrationError(response);
            AssertBusinessRegistrationFails();
        }

        private void ThenTheBusinessRegistrationFailsWithDupliateEmailError(BusinessRegistrationResponse response)
        {
            AssertDuplicateBusinessAdminEmailError(response);
            AssertBusinessRegistrationFails();
        }

        private void AssertMissingRegistrationError(BusinessRegistrationResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1010));
            Assert.That(error.Message, Is.EqualTo("Missing business registration data."));
        }

        private void AssertDuplicateBusinessAdminEmailError(BusinessRegistrationResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1020));
            Assert.That(error.Message, Is.EqualTo("This email address is already in use."));
        }

        private void AssertBusinessRegistrationFails()
        {
            AssertBusinessIsNotRegistered();
            AssertBusinessAdminIsNotRegistered();
            AssertRegistrationEmailIsNotSent();
        }


        private void ThenTheBusinessRegistrationSucceeds(BusinessRegistrationResponse response)
        {
            AssertBusinessIsRegistered();
            AssertBusinessAdminIsRegistered();
            AssertRegistrationEmailIsSent();
        }

        private void AssertBusinessIsNotRegistered()
        {
            Assert.That(BusinessRepository.WasAddCalled, Is.False);
        }

        private void AssertBusinessAdminIsNotRegistered()
        {
            Assert.That(BusinessAdminRepository.WasAddCalled, Is.False);
        }

        private void AssertRegistrationEmailIsNotSent()
        {
            Assert.That(BusinessRegistrationEmailer.WasSendEmailCalled, Is.False);
        }

        private void AssertBusinessIsRegistered()
        {
            Assert.That(BusinessRepository.WasAddCalled, Is.True);
        }

        private void AssertBusinessAdminIsRegistered()
        {
            Assert.That(BusinessAdminRepository.WasAddCalled, Is.True);
        }

        private void AssertRegistrationEmailIsSent()
        {
            Assert.That(BusinessRegistrationEmailer.WasSendEmailCalled, Is.True);
        }
    }
}
