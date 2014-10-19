using CoachSeek.Application.Configuration;
using CoachSeek.Application.Contracts.Models.Responses;
using CoachSeek.Application.UseCases;
using CoachSeek.Data.Model;
using CoachSeek.DataAccess.Configuration;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Exceptions;
using CoachSeek.WebUI.Controllers;
using CoachSeek.WebUI.Models.Api;
using CoachSeek.WebUI.Tests.Unit.Fakes;
using NUnit.Framework;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CoachSeek.WebUI.Tests.Unit.Controllers
{
    [TestFixture]
    public class CoachesControllerTests
    {
        private const string BUSINESS_ID = "12345678-B10E-4C0E-B46F-1386B98CE567";
        private const string COACH_ID = "90ABCDEF-AAAA-429B-8972-EAB6E00C732B";

        private CoachesController Controller { get; set; }

        [TestFixtureSetUp]
        public void SetupAllTests()
        {
            WebApiAutoMapperConfigurator.Configure();
            ApplicationAutoMapperConfigurator.Configure();
            DbAutoMapperConfigurator.Configure();
        }

        [SetUp]
        public void Setup()
        {
            Controller = new CoachesController
            {
                Request = new HttpRequestMessage(),
                Configuration = new HttpConfiguration()
            };
        }

        private CoachData SetupCoach()
        {
            return new CoachData
            {
                Id = new Guid(COACH_ID),
                FirstName = "",
                LastName = "",
                Email = "",
                Phone = "",
                WorkingHours = new WeeklyWorkingHoursData
                {
                    Monday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Tuesday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Wednesday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Thursday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Friday = new DailyWorkingHoursData(true, "9:00", "17:00"),
                    Saturday = new DailyWorkingHoursData(false),
                    Sunday = new DailyWorkingHoursData(false),
                }
            };
        }

        //private Business SetupBusiness()
        //{
        //    return new Business(new Guid(BUSINESS_ID),
        //        "Olaf's Cafe",
        //        "olafscafe",
        //        new BusinessAdminData
        //        {
        //            FirstName = "Bobby",
        //            LastName = "Tables",
        //            Email = "bobby@tables.hack",
        //            Username = "bobby@tables.hack",
        //        },
        //        null,
        //        null);
        //}

        private MockCoachAddUseCase AddUseCase
        {
            get { return (MockCoachAddUseCase)Controller.CoachAddUseCase; }
        }

        private MockCoachUpdateUseCase UpdateUseCase
        {
            get { return (MockCoachUpdateUseCase)Controller.CoachUpdateUseCase; }
        }


        [Test]
        public void WhenConstruct_ThenSetDependencies()
        {
            var controller = WhenConstruct();
            ThenSetDependencies(controller);
        }

        [Test]
        public void GivenCoachAddCommandWithError_WhenPost_ThenCallAddUseCaseAndReturnErrorResponse()
        {
            var useCase = GivenCoachAddCommandWithError();
            var response = WhenPost(useCase);
            ThenCallAddUseCaseAndReturnErrorResponse(response);
        }

        [Test]
        public void GivenCoachAddCommandWithoutError_WhenPost_ThenCallAddUseCaseAndReturnSuccessResponse()
        {
            var useCase = GivenCoachAddCommandWithoutError();
            var response = WhenPost(useCase);
            ThenCallAddUseCaseAndReturnSuccessResponse(response);
        }

        [Test]
        public void GivenCoachUpdateCommandWithError_WhenPost_ThenCallUpdateUseCaseAndReturnErrorResponse()
        {
            var useCase = GivenCoachUpdateCommandWithError();
            var response = WhenPost(useCase);
            ThenCallUpdateUseCaseAndReturnErrorResponse(response);
        }

        [Test]
        public void GivenCoachUpdateCommandWithoutError_WhenPost_ThenCallUpdateUseCaseAndReturnSuccessResponse()
        {
            var useCase = GivenCoachUpdateCommandWithoutError();
            var response = WhenPost(useCase);
            ThenCallUpdateUseCaseAndReturnSuccessResponse(response);
        }

        private MockCoachAddUseCase GivenCoachAddCommandWithError()
        {
            return new MockCoachAddUseCase
            {
                Response = new Response<CoachData>(new ValidationException(2, "Error!"))
            };
        }

        private MockCoachAddUseCase GivenCoachAddCommandWithoutError()
        {
            return new MockCoachAddUseCase
            {
                Response = new Response<CoachData>(SetupCoach())
            };
        }

        private MockCoachUpdateUseCase GivenCoachUpdateCommandWithError()
        {
            return new MockCoachUpdateUseCase
            {
                Response = new Response<CoachData>(new ValidationException(2, "Error!"))
            };
        }

        private MockCoachUpdateUseCase GivenCoachUpdateCommandWithoutError()
        {
            return new MockCoachUpdateUseCase
            {
                Response = new Response<CoachData>(SetupCoach())
            };
        }


        private CoachesController WhenConstruct()
        {
            var addUseCase = new CoachAddUseCase(null);
            var updateUseCase = new CoachUpdateUseCase(null);

            return new CoachesController(addUseCase, updateUseCase);
        }

        private HttpResponseMessage WhenPost(MockCoachAddUseCase useCase)
        {
            var apiCoachSaveCommand = new ApiCoachSaveCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                FirstName = "John",
                LastName = "Smith",
                Email = "john@smith.co.nz",
                Phone = "0987654321",
                WorkingHours = new ApiWeeklyWorkingHours
                {
                    Monday = new ApiDailyWorkingHours
                    {
                        IsAvailable = true,
                        StartTime = "9:30",
                        FinishTime = "15:30"
                    },
                    Tuesday = new ApiDailyWorkingHours
                    {
                        IsAvailable = true,
                        StartTime = "7:45",
                        FinishTime = "18:15"
                    }
                }
            };

            Controller.CoachAddUseCase = useCase;

            return Controller.Post(apiCoachSaveCommand);
        }

        private HttpResponseMessage WhenPost(MockCoachUpdateUseCase useCase)
        {
            var apiCoachSaveCommand = new ApiCoachSaveCommand
            {
                BusinessId = new Guid(BUSINESS_ID),
                Id = new Guid(COACH_ID),
                FirstName = "John",
                LastName = "Smith",
                Email = "john@smith.co.nz",
                Phone = "0987654321"
            };

            Controller.CoachUpdateUseCase = useCase;

            return Controller.Post(apiCoachSaveCommand);
        }


        private void ThenSetDependencies(CoachesController controller)
        {
            Assert.That(controller.CoachAddUseCase, Is.Not.Null);
            Assert.That(controller.CoachUpdateUseCase, Is.Not.Null);
        }

        private void ThenCallAddUseCaseAndReturnErrorResponse(HttpResponseMessage response)
        {
            AssertWasAddCoachCalled();
            AssertPassRelevantInfoIntoAddCoach();
            AssertErrorResponse(response);
        }

        private void ThenCallAddUseCaseAndReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertWasAddCoachCalled();
            AssertPassRelevantInfoIntoAddCoach();
            AssertSuccessResponse(response);
        }

        private void ThenCallUpdateUseCaseAndReturnErrorResponse(HttpResponseMessage response)
        {
            AssertWasUpdateCoachCalled();
            AssertPassRelevantInfoIntoUpdateCoach();
            AssertErrorResponse(response);
        }

        private void ThenCallUpdateUseCaseAndReturnSuccessResponse(HttpResponseMessage response)
        {
            AssertWasUpdateCoachCalled();
            AssertPassRelevantInfoIntoUpdateCoach();
            AssertSuccessResponse(response);
        }

        private void AssertWasAddCoachCalled()
        {
            Assert.That(AddUseCase.WasAddCoachCalled, Is.True);
        }

        private void AssertWasUpdateCoachCalled()
        {
            Assert.That(UpdateUseCase.WasUpdateCoachCalled, Is.True);
        }

        private void AssertPassRelevantInfoIntoAddCoach()
        {
            var command = ((MockCoachAddUseCase)Controller.CoachAddUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.BusinessId, Is.EqualTo(new Guid(BUSINESS_ID)));
            Assert.That(command.FirstName, Is.EqualTo("John"));
            Assert.That(command.LastName, Is.EqualTo("Smith"));
            Assert.That(command.Email, Is.EqualTo("john@smith.co.nz"));
            Assert.That(command.Phone, Is.EqualTo("0987654321"));
            var workingHours = command.WorkingHours;
            AssertValidWorkingHours(workingHours.Monday, true, "9:30", "15:30");
            AssertValidWorkingHours(workingHours.Tuesday, true, "7:45", "18:15");
            Assert.That(workingHours.Wednesday, Is.Null);
        }

        private void AssertValidWorkingHours(DailyWorkingHoursCommand workingHours, 
            bool isAvailable, 
            string startTime,
            string finishTime)
        {
            Assert.That(workingHours.IsAvailable, Is.EqualTo(isAvailable));
            Assert.That(workingHours.StartTime, Is.EqualTo(startTime));
            Assert.That(workingHours.FinishTime, Is.EqualTo(finishTime));
        }

        private void AssertPassRelevantInfoIntoUpdateCoach()
        {
            var command = ((MockCoachUpdateUseCase)Controller.CoachUpdateUseCase).Command;
            Assert.That(command, Is.Not.Null);
            Assert.That(command.BusinessId, Is.EqualTo(new Guid(BUSINESS_ID)));
            Assert.That(command.CoachId, Is.EqualTo(new Guid(COACH_ID)));
            Assert.That(command.FirstName, Is.EqualTo("John"));
            Assert.That(command.LastName, Is.EqualTo("Smith"));
            Assert.That(command.Email, Is.EqualTo("john@smith.co.nz"));
            Assert.That(command.Phone, Is.EqualTo("0987654321"));
        }

        private void AssertErrorResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Error error;
            Assert.That(response.TryGetContentValue(out error), Is.True);
            Assert.That(error.Code, Is.EqualTo(2));
            Assert.That(error.Message, Is.EqualTo("Error!"));
        }

        private void AssertSuccessResponse(HttpResponseMessage response)
        {
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            CoachData coach;
            Assert.That(response.TryGetContentValue(out coach), Is.True);
            Assert.That(coach.Id, Is.EqualTo(new Guid(COACH_ID)));
        }
    }
}
