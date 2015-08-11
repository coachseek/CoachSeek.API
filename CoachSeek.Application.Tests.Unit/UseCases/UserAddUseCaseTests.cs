using CoachSeek.Application.Contracts.Models;
using CoachSeek.Application.UseCases;
using CoachSeek.Common;
using CoachSeek.Domain.Commands;
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
        public void GivenNoUserAddCommand_WhenAddUser_ThenUserAddFailsWithMissingUserError()
        {
            var command = GivenNoUserAddCommand();
            var response = WhenAddUser(command);
            ThenUserAddFailsWithMissingUserError(response);
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


        private UserAddCommand GivenNoUserAddCommand()
        {
            return null;
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


        private IResponse WhenAddUser(UserAddCommand command)
        {
            var useCase = new UserAddUseCase {UserRepository = UserRepository};

            return useCase.AddUser(command);
        }


        private void ThenUserAddFailsWithMissingUserError(IResponse response)
        {
            AssertMissingUserError(response);
            AssertSaveNewUserIsCalled(false);
        }

        private void ThenUserAddFailsWithDuplicateUserError(IResponse response)
        {
            AssertDuplicateUserError(response);
            AssertSaveNewUserIsCalled(false);
        }

        private void ThenUserAddSucceeds(IResponse response)
        {
            AssertSaveNewUserIsCalled(true);
        }

        private void AssertMissingUserError(IResponse response)
        {
            AssertSingleError(response, "Missing data.");
        }

        private void AssertDuplicateUserError(IResponse response)
        {
            AssertSingleError(response, ErrorCodes.UserDuplicate, "The user with email address 'bgates@gmail.com' already exists.", "bgates@gmail.com");
        }


        private void AssertSaveNewUserIsCalled(bool wasCalled)
        {
            Assert.That(UserRepository.WasSaveNewUserCalled, Is.EqualTo(wasCalled));
        }
    }
}
