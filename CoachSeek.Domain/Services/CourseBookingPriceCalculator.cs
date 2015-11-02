using System;
using System.Collections.Generic;
using System.Linq;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Services
{
    public static class CourseBookingPriceCalculator
    {
        public static decimal CalculatePrice(CourseBookingData booking, RepeatedSessionData course)
        {
            return CalculatePrice(booking.SessionBookings,
                                  course.Sessions, 
                                  course.Pricing.CoursePrice);
        }

        public static decimal CalculatePrice(IList<SessionKeyCommand> sessions, RepeatedSessionData course)
        {
            var sessionBookings = sessions.Select(x => new SingleSessionBookingData {Session = new SessionKeyData(x.Id)});
            return CalculatePrice(sessionBookings.ToList(),
                                  course.Sessions,
                                  course.Pricing.CoursePrice);
        }

        public static decimal CalculatePrice(IList<SingleSessionBookingData> sessionBookings,
                                             IList<SingleSessionData> sessionsInCourse,
                                             decimal? coursePrice = null)
        {
            if (!sessionBookings.Any())
                return 0;
            ValidateCanCalculatePrice(sessionBookings, sessionsInCourse, coursePrice);
            if (sessionBookings.Count == sessionsInCourse.Count)
                return CalculateWholeCoursePrice(sessionBookings, sessionsInCourse, coursePrice);
            return CalculatePartialCoursePrice(sessionBookings, sessionsInCourse, coursePrice);
        }

        private static void ValidateCanCalculatePrice(IList<SingleSessionBookingData> sessionBookings,
                                                      IList<SingleSessionData> sessionsInCourse,
                                                      decimal? coursePrice)
        {
            if (coursePrice.HasValue)
                return;
            foreach (var sessionBooking in sessionBookings)
            {
                var sessionPrice = sessionsInCourse.Single(x => x.Id == sessionBooking.Session.Id).Pricing.SessionPrice;
                if (!sessionPrice.HasValue)
                    throw new ArgumentException("Must have either session or course price.");
            }
        }

        private static decimal CalculateWholeCoursePrice(IList<SingleSessionBookingData> sessionBookings,
                                                         IList<SingleSessionData> sessionsInCourse,
                                                         decimal? coursePrice)
        {
            if (coursePrice.HasValue)
                return coursePrice.Value;
            return SumUpSessionPricesForWholeCourse(sessionBookings, sessionsInCourse);
        }

        private static decimal CalculatePartialCoursePrice(IList<SingleSessionBookingData> sessionBookings,
                                                           IList<SingleSessionData> sessionsInCourse,
                                                           decimal? coursePrice)
        {
            return SumUpSessionPricesForPartialCourse(sessionBookings, sessionsInCourse, coursePrice);
        }

        private static decimal SumUpSessionPricesForWholeCourse(IList<SingleSessionBookingData> sessionBookings,
                                                                IList<SingleSessionData> sessionsInCourse)
        {
            return sessionBookings.Sum(sessionBooking => sessionsInCourse.Single(x => x.Id == sessionBooking.Session.Id).Pricing.SessionPrice.GetValueOrDefault());
        }

        private static decimal SumUpSessionPricesForPartialCourse(IList<SingleSessionBookingData> sessionBookings,
                                                                  IList<SingleSessionData> sessionsInCourse,
                                                                  decimal? coursePrice)
        {
            decimal price = 0;
            foreach (var sessionBooking in sessionBookings)
            {
                var sessionPrice = sessionsInCourse.Single(x => x.Id == sessionBooking.Session.Id).Pricing.SessionPrice;
                if (!sessionPrice.HasValue)
                    sessionPrice = ProRataCoursePrice(coursePrice.Value, sessionsInCourse);
                price += sessionPrice.Value;
            }
            return price;
        }

        private static decimal ProRataCoursePrice(decimal coursePrice, IList<SingleSessionData> sessionsInCourse)
        {
            return Math.Round(coursePrice / sessionsInCourse.Count, 2);
        }
    }
}
