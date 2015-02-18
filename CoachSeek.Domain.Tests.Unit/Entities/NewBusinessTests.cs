using System;
using CoachSeek.DataAccess.Repositories;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;
using CoachSeek.Domain.Tests.Unit.Fakes;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Entities
{
    [TestFixture]
    public class NewBusinessTests
    {
        private MockBusinessDomainBuilder BusinessDomainBuilder { get; set; }
        private InMemoryBusinessRepository BusinessRepository { get; set; }

        [SetUp]
        public void Setup()
        {
            SetupBusinessDomainBuilder();
            SetupBusinessRepository();
        }

        private void SetupBusinessDomainBuilder()
        {
            BusinessDomainBuilder = new MockBusinessDomainBuilder();
        }

        private void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();
            BusinessRepository.Clear();

            BusinessRepository.Add(new Business(Guid.NewGuid(), 
                "Olaf's Scuba Shoppe", 
                "olafsscubahoppe", 
                null,
                null,
                null,
                null,
                null,
                null));
        }

        //[Test]
        //public void GivenValidRegistration_WhenConstructNewBusiness_ThenCreateValidNewBusiness()
        //{
        //    var command = GivenValidBusinessAddCommand();
        //    var newBusiness = WhenConstructNewBusiness(command);
        //    ThenCreateValidNewBusiness(newBusiness);
        //}

        //[Test]
        //public void GivenDuplicateBusinessAdmin_WhenTryRegisterNewBusiness_ThenWillNotRegisterNewBusiness()
        //{
        //    var newBusiness = GivenDuplicateBusinessAdmin();
        //    var exception = WhenTryRegisterNewBusiness(newBusiness);
        //    ThenWillNotRegisterNewBusiness(exception);
        //}

        //[Test]
        //public void GivenUniqueBusinessAdmin_WhenTryRegisterNewBusiness_ThenWillRegisterNewBusiness()
        //{
        //    var newBusiness = GivenUniqueBusinessAdmin();
        //    var exception = WhenTryRegisterNewBusiness(newBusiness);
        //    ThenWillRegisterNewBusiness(exception);
        //}


        private BusinessAddCommand GivenValidBusinessAddCommand()
        {
            BusinessDomainBuilder.Domain = "ianstenniscoaching";

            return new BusinessAddCommand
            {
                Name = "Ian's Tennis Coaching"
            };
        }

        private BusinessRegistrationCommand GivenUniqueAdminRegistration()
        {
            BusinessDomainBuilder.Domain = "aucklandbrakerepairs";

            return new BusinessRegistrationCommand
            {
                BusinessName = "Auckland Brake Repairs",

                Registrant = new BusinessRegistrantCommand
                {
                    FirstName = "Steven",
                    LastName = "Armstrong",
                    Email = "steve@abr.co.nz",
                    Password = "P@ssword1!"
                }
            };
        }

        private BusinessRegistrationCommand GivenDuplicateAdminRegistration()
        {
            BusinessDomainBuilder.Domain = "olafscafe";

            return new BusinessRegistrationCommand
            {
                BusinessName = "Olaf's Cafe",

                Registrant = new BusinessRegistrantCommand
                {
                    FirstName = "  Olaf ",
                    LastName = " Thielke  ",
                    Email = " Olaf@Gmail.com ",
                    Password = "P@ssword1!"
                }
            };
        }

        //private NewBusiness GivenDuplicateBusinessAdmin()
        //{
        //    return new NewBusiness(GivenDuplicateAdminRegistration(), BusinessDomainBuilder);
        //}

        //private NewBusiness GivenUniqueBusinessAdmin()
        //{
        //    return new NewBusiness(GivenUniqueAdminRegistration(), BusinessDomainBuilder);
        //}


        private NewBusiness WhenConstructNewBusiness(BusinessAddCommand command)
        {
            return new NewBusiness(command, BusinessDomainBuilder);
        }

        private Exception WhenTryRegisterNewBusiness(NewBusiness newBusiness)
        {
            try
            {
                newBusiness.Register(BusinessRepository);
                return null;
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        //private void ThenCreateValidNewBusiness(NewBusiness newBusiness)
        //{
        //    Assert.That(newBusiness, Is.Not.Null);
        //    Assert.That(newBusiness.Name, Is.EqualTo("Auckland Brake Repairs"));
        //    Assert.That(newBusiness.Domain, Is.EqualTo("aucklandbrakerepairs"));
        //    var admin = newBusiness.Admin;
        //    Assert.That(admin, Is.Not.Null);
        //    Assert.That(admin.FirstName, Is.EqualTo("Steven"));
        //    Assert.That(admin.LastName, Is.EqualTo("Armstrong"));
        //    Assert.That(admin.Email, Is.EqualTo("steve@abr.co.nz"));
        //    Assert.That(admin.Username, Is.EqualTo("steve@abr.co.nz"));
        //    Assert.That(admin.PasswordHash, Is.EqualTo("P@ssword1!"));
        //}

        private void ThenWillNotRegisterNewBusiness(Exception ex)
        {
            AssertSaveNewBusinessWasNotCalled();
            AssertDuplicateBusinessAdminException(ex);
        }

        private void ThenWillRegisterNewBusiness(Exception ex)
        {
            AssertSaveNewBusinessWasCalled();
            AssertNoExceptionWasThrown(ex);
        }

        private void AssertSaveNewBusinessWasNotCalled()
        {
            Assert.That(BusinessRepository.WasSaveNewBusinessCalled, Is.False);
        }

        private void AssertSaveNewBusinessWasCalled()
        {
            Assert.That(BusinessRepository.WasSaveNewBusinessCalled, Is.True);
        }

        private void AssertDuplicateBusinessAdminException(Exception ex)
        {
            Assert.That(ex, Is.Not.Null);
            Assert.That(ex, Is.InstanceOf<DuplicateBusinessAdmin>());
        }

        private void AssertNoExceptionWasThrown(Exception ex)
        {
            Assert.That(ex, Is.Null);
        }
    }
}
