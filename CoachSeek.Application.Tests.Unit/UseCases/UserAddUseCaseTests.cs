using System;
using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using NUnit.Framework;

namespace CoachSeek.Application.Tests.Unit.UseCases
{
    [TestFixture]
    public class UserAddUseCaseTests : UseCaseTests
    {
        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            ConfigureAutoMapper();
        }

        [SetUp]
        public void Setup()
        {
            SetupUserRepository();
            SetupBusinessRepository();
        }


        [Test]
        public void GivenExistingUser_WhenAddUser_ThenUserAddFailsWithDuplicateUserError()
        {
            var command = GivenExistingUser();
            var response = WhenAddUser(command);
            ThenUserAddFailsWithDuplicateUserError(response);
        }

        [Test]
        public void GivenAUniqueUserOnEmailAddress_WhenAddUser_ThenUserAddSucceeds()
        {
            var command = GivenAUniqueUserOnEmailAddress();
            var response = WhenAddUser(command);
            ThenUserAddSucceeds(response);
        }


        private UserAddCommand GivenExistingUser()
        {
            return new UserAddCommand
            {
                FirstName = "  Bill",
                LastName = "Gates  ",
                Email = " BGates@Gmail.Com ",
                Password = "Microsoft75"
            };
        }

        private UserAddCommand GivenAUniqueUserOnEmailAddress()
        {
            return new UserAddCommand
            {
                FirstName = " Ian",
                LastName = "Bishop ",
                Email = "  ianbish@GMAIL.com",
                Password = "password"
            };
        }


        private object WhenAddUser(UserAddCommand command)
        {
            var useCase = new UserAddUseCase {UserRepository = UserRepository};

            try
            {
                return useCase.AddUser(command);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private void ThenUserAddFailsWithDuplicateUserError(object response)
        {
            AssertDuplicateUserError(response);
            AssertSaveNewUserIsCalled(false);
        }

        private void ThenUserAddSucceeds(object response)
        {
            Assert.That(response, Is.InstanceOf<Response>());

            AssertSaveNewUserIsCalled(true);
        }

        private void AssertDuplicateUserError(object response)
        {
            Assert.That(response, Is.InstanceOf<UserDuplicate>());
            var duplicateUser = (UserDuplicate)response;

            Assert.That(duplicateUser.ErrorCode, Is.EqualTo(ErrorCodes.UserDuplicate));
            Assert.That(duplicateUser.Message, Is.EqualTo("The user with email address 'bgates@gmail.com' already exists."));
            Assert.That(duplicateUser.Data, Is.EqualTo("bgates@gmail.com"));
        }


        private void AssertSaveNewUserIsCalled(bool wasCalled)
        {
            Assert.That(UserRepository.WasSaveNewUserCalled, Is.EqualTo(wasCalled));
        }
    }
}
