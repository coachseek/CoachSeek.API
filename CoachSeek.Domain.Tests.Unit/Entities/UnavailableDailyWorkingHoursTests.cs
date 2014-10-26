//using CoachSeek.Domain.Entities;
//using NUnit.Framework;

//namespace CoachSeek.Domain.Tests.Unit.Entities
//{
//    [TestFixture]
//    public class UnavailableDailyWorkingHoursTests
//    {
//        [Test]
//        public void WhenConstruct_ThenCreateValidUnavailableDailyWorkingHours()
//        {
//            var response = WhenConstruct();
//            ThenCreateValidUnavailableDailyWorkingHours(response);
//        }


//        private DailyWorkingHours WhenConstruct()
//        {
//            return new UnavailableDailyWorkingHours();
//        }

//        private void ThenCreateValidUnavailableDailyWorkingHours(DailyWorkingHours workingHours)
//        {
//            Assert.That(workingHours.IsAvailable, Is.False);
//            Assert.That(workingHours.StartTime, Is.Null);
//            Assert.That(workingHours.FinishTime, Is.Null);
//        }
//    }
//}
