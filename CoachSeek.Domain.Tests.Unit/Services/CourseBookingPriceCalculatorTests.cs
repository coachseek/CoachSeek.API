using System;
using System.Collections.Generic;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Services;
using NUnit.Framework;

namespace CoachSeek.Domain.Tests.Unit.Services
{
    [TestFixture]
    public class CourseBookingPriceCalculatorTests
    {
        private const string COURSE_ID = "0D39E9B9-654B-46DA-AC13-BC6B078F60CD";
        private const string SESSION_ONE_ID = "125884B1-F543-4647-AD8B-A18061526E23";
        private const string SESSION_TWO_ID = "24D3E1A5-B425-4CA2-AA90-5C34CA37FDD6";
        private const string SESSION_THREE_ID = "36536C9D-FB8D-4431-9B8F-2D03627E4762";
        private const string SESSION_FOUR_ID = "48B01DFF-2FC2-4A7E-AD56-72BB3FAC8033";
        private const string SESSION_FIVE_ID = "5A194365-476D-4E4F-B56E-0F067BD6CCD7";

        private List<SingleSessionData> CreateSessionsInCourse()
        {
            return new List<SingleSessionData>
            {
                new SingleSessionData
                {
                    Id = new Guid(SESSION_ONE_ID),
                    ParentId = new Guid(COURSE_ID),
                    Pricing = new SingleSessionPricingData(50)
                },
                new SingleSessionData
                {
                    Id = new Guid(SESSION_TWO_ID),
                    ParentId = new Guid(COURSE_ID),
                    Pricing = new SingleSessionPricingData(50)
                },
                new SingleSessionData
                {
                    Id = new Guid(SESSION_THREE_ID),
                    ParentId = new Guid(COURSE_ID),
                    Pricing = new SingleSessionPricingData(50)
                },
                new SingleSessionData
                {
                    Id = new Guid(SESSION_FOUR_ID),
                    ParentId = new Guid(COURSE_ID),
                    Pricing = new SingleSessionPricingData(50)
                },
                new SingleSessionData
                {
                    Id = new Guid(SESSION_FIVE_ID),
                    ParentId = new Guid(COURSE_ID),
                    Pricing = new SingleSessionPricingData(75)
                },
            };
        }

        private void AddMoreSessionsToCourse(List<SingleSessionData> sessionsInCourse)
        {
            sessionsInCourse.Add(new SingleSessionData
                {
                    Id = Guid.NewGuid(),
                    ParentId = new Guid(COURSE_ID),
                    Pricing = new SingleSessionPricingData()
                });
        }


        private List<BookingSessionData> BookNoSessionsInCourse()
        {
            return new List<BookingSessionData>();
        }

        private List<BookingSessionData> BookOneSessionInCourse()
        {
            return new List<BookingSessionData>
            {
                new BookingSessionData { Id = new Guid(SESSION_TWO_ID) }
            };
        }

        private List<BookingSessionData> BookAllSessionsInCourse()
        {
            return new List<BookingSessionData>
            {
                new BookingSessionData { Id = new Guid(SESSION_ONE_ID) },
                new BookingSessionData { Id = new Guid(SESSION_TWO_ID) },
                new BookingSessionData { Id = new Guid(SESSION_THREE_ID) },
                new BookingSessionData { Id = new Guid(SESSION_FOUR_ID) },
                new BookingSessionData { Id = new Guid(SESSION_FIVE_ID) }
            };
        }


        [Test]
        public void GivenNoSessionBookings_WhenTryCalculateBookingPrice_ThenReturnZeroPrice()
        {
            var parameters = GivenNoSessionBookings();
            var price = WhenTryCalculateBookingPrice(parameters);
            ThenReturnZeroPrice(price);
        }

        [Test]
        public void GivenNoCoursePriceAndNoSessionPriceInSession_WhenTryCalculateBookingPrice_ThenThrowException()
        {
            var parameters = GivenNoCoursePriceAndNoSessionPriceInSession();
            try
            {
                WhenTryCalculateBookingPrice(parameters);
                Assert.Fail();
            }
            catch (Exception)
            {
            }
        }

        [Test]
        public void GivenBookingAllSessionsInCourseAndHaveCoursePrice_WhenTryCalculateBookingPrice_ThenReturnCoursePrice()
        {
            var parameters = GivenBookingAllSessionsInCourseAndHaveCoursePrice();
            var price = WhenTryCalculateBookingPrice(parameters);
            ThenReturnCoursePrice(price);
        }

        [Test]
        public void GivenBookingAllSessionsInCourseAndHaveNoCoursePrice_WhenTryCalculateBookingPrice_ThenReturnSumOfAllSessionPrices()
        {
            var parameters = GivenBookingAllSessionsInCourseAndHaveNoCoursePrice();
            var price = WhenTryCalculateBookingPrice(parameters);
            ThenReturnSumOfAllSessionPrices(price);
        }

        [Test]
        public void GivenBookingSomeSessionsInCourseAndHaveAllSessionPrices_WhenTryCalculateBookingPrice_ThenReturnSumOfBookedSessionPrices()
        {
            var parameters = GivenBookingSomeSessionsInCourseAndHaveAllSessionPrices();
            var price = WhenTryCalculateBookingPrice(parameters);
            ThenReturnSumOfBookedSessionPrices(price);
        }

        [Test]
        public void GivenBookingSomeSessionsInCourseAndAreMissingSessionPricesButHaveCoursePrice_WhenTryCalculateBookingPrice_ThenReturnProRataOfCoursePrice()
        {
            var parameters = GivenBookingSomeSessionsInCourseAndAreMissingSessionPricesButHaveCoursePrice();
            var price = WhenTryCalculateBookingPrice(parameters);
            ThenReturnProRataOfCoursePrice(price);
        }

        [Test]
        public void GivenBookingOneSessionInCourseAndHaveSessionPrice_WhenTryCalculateBookingPrice_ThenReturnSessionPrice()
        {
            var parameters = GivenBookingOneSessionInCourseAndHaveSessionPrice();
            var price = WhenTryCalculateBookingPrice(parameters);
            ThenReturnSessionPrice(price);
        }

        [Test]
        public void GivenBookingOneSessionInCourseAndHaveCoursePriceOnly_WhenTryCalculateBookingPrice_ThenReturnProRataCoursePrice()
        {
            var parameters = GivenBookingOneSessionInCourseAndHaveCoursePriceOnly();
            var price = WhenTryCalculateBookingPrice(parameters);
            ThenReturnProRataCoursePrice(price);
        }


        private Parameters GivenNoSessionBookings()
        {
            var sessionsInCourse = CreateSessionsInCourse();
            return new Parameters(BookNoSessionsInCourse(), sessionsInCourse);
        }

        private Parameters GivenBookingAllSessionsInCourseAndHaveCoursePrice()
        {
            var sessionsInCourse = CreateSessionsInCourse();
            return new Parameters(BookAllSessionsInCourse(), sessionsInCourse, 225);
        }

        private Parameters GivenNoCoursePriceAndNoSessionPriceInSession()
        {
            var sessionsInCourse = CreateSessionsInCourse();
            sessionsInCourse[1].Pricing.SessionPrice = null;

            return new Parameters(BookAllSessionsInCourse(), sessionsInCourse);
        }

        private Parameters GivenBookingAllSessionsInCourseAndHaveNoCoursePrice()
        {
            var sessionsInCourse = CreateSessionsInCourse();
            return new Parameters(BookAllSessionsInCourse(), sessionsInCourse);
        }

        private Parameters GivenBookingSomeSessionsInCourseAndHaveAllSessionPrices()
        {
            var bookedSessions = BookAllSessionsInCourse();
            bookedSessions.RemoveAt(1);
            bookedSessions.RemoveAt(2);
            return new Parameters(bookedSessions, CreateSessionsInCourse());
        }

        private Parameters GivenBookingSomeSessionsInCourseAndAreMissingSessionPricesButHaveCoursePrice()
        {
            var bookedSessions = BookAllSessionsInCourse();
            bookedSessions.RemoveAt(1);

            var sessionsInCourse = CreateSessionsInCourse();
            AddMoreSessionsToCourse(sessionsInCourse);
            sessionsInCourse[0].Pricing.SessionPrice = null;
            sessionsInCourse[2].Pricing.SessionPrice = null;

            return new Parameters(bookedSessions, sessionsInCourse, 200);
        }

        private Parameters GivenBookingOneSessionInCourseAndHaveSessionPrice()
        {
            return new Parameters(BookOneSessionInCourse(), CreateSessionsInCourse());
        }

        private Parameters GivenBookingOneSessionInCourseAndHaveCoursePriceOnly()
        {
            var sessionsInCourse = CreateSessionsInCourse();
            sessionsInCourse[1].Pricing.SessionPrice = null;
            return new Parameters(BookOneSessionInCourse(), sessionsInCourse, 200);
        }


        private decimal WhenTryCalculateBookingPrice(Parameters parameters)
        {
            return CourseBookingPriceCalculator.CalculatePrice(parameters.BookedSessions, 
                                                               parameters.CourseSessions,
                                                               parameters.CoursePrice);
        }


        private void ThenReturnZeroPrice(decimal price)
        {
            Assert.That(price, Is.EqualTo(0));
        }

        private void ThenReturnCoursePrice(decimal price)
        {
            Assert.That(price, Is.EqualTo(225));
        }

        private void ThenReturnSumOfAllSessionPrices(decimal price)
        {
            Assert.That(price, Is.EqualTo(275));
        }

        private void ThenReturnSumOfBookedSessionPrices(decimal price)
        {
            Assert.That(price, Is.EqualTo(175));
        }

        private void ThenReturnProRataOfCoursePrice(decimal price)
        {
            // Also checks that session price was rounded to 2 DP before summing up because value is $191.66 and not $191.67.
            Assert.That(price, Is.EqualTo(191.66m));
        }

        private void ThenReturnSessionPrice(decimal price)
        {
            Assert.That(price, Is.EqualTo(50));
        }

        private void ThenReturnProRataCoursePrice(decimal price)
        {
            Assert.That(price, Is.EqualTo(40));
        }


        private class Parameters
        {
            public List<BookingSessionData> BookedSessions { get; private set; }
            public List<SingleSessionData> CourseSessions { get; private set; }
            public decimal? CoursePrice { get; private set; }

            public Parameters(List<BookingSessionData> bookedSessions,
                              List<SingleSessionData> courseSessions,
                              decimal? coursePrice = null)
            {
                BookedSessions = bookedSessions;
                CourseSessions = courseSessions;
                CoursePrice = coursePrice;
            }
        }
    }
}
