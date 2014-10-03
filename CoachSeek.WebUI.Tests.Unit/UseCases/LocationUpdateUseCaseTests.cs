using System.Linq;
using CoachSeek.WebUI.Models.Requests;
using CoachSeek.WebUI.Models.Responses;
using CoachSeek.WebUI.Persistence;
using CoachSeek.WebUI.UseCases;
using NUnit.Framework;

namespace CoachSeek.WebUI.Tests.Unit.UseCases
{
    [TestFixture]
    public class LocationUpdateUseCaseTests
    {
        private InMemoryBusinessRepository BusinessRepository { get; set; }

        [SetUp]
        public void Setup()
        {
            SetupBusinessRepository();
        }

        private void SetupBusinessRepository()
        {
            BusinessRepository = new InMemoryBusinessRepository();
        }

        
        [Test]
        public void GivenNoLocationUpdateRequest_WhenUpdateLocation_ThenLocationUpdateFailsWithMissingLocationError()
        {
            var request = GivenNoLocationUpdateRequest();
            var response = WhenUpdateLocation(request);
            ThenLocationUpdateFailsWithMissingLocationError(response);
        }

        private LocationUpdateRequest GivenNoLocationUpdateRequest()
        {
            return null;
        }

        private LocationUpdateResponse WhenUpdateLocation(LocationUpdateRequest request)
        {
            var useCase = new LocationUpdateUseCase(BusinessRepository);

            return useCase.UpdateLocation(request);
        }

        private void ThenLocationUpdateFailsWithMissingLocationError(LocationUpdateResponse response)
        {
            AssertMissingLocationError(response);
            AssertSaveBusinessIsNotCalled();
        }

        private void AssertMissingLocationError(LocationUpdateResponse response)
        {
            Assert.That(response.Business, Is.Null);
            Assert.That(response.Errors, Is.Not.Null);
            Assert.That(response.Errors.Count, Is.EqualTo(1));
            var error = response.Errors.First();
            Assert.That(error.Code, Is.EqualTo(1130));
            Assert.That(error.Message, Is.EqualTo("Missing location data."));
            Assert.That(error.Field, Is.Null);
        }

        private void AssertSaveBusinessIsNotCalled()
        {
            Assert.That(BusinessRepository.WasSaveBusinessCalled, Is.False);
        }
    }
}
