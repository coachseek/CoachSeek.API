using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Services
{
    public static class CourseBookingPriceCalculator
    {
        public static decimal CalculatePrice(CourseBookingData booking, RepeatedSessionData course)
        {
            return CalculatePrice(booking.SessionBookings.Count,
                                  course.Sessions.Count, 
                                  course.Pricing.CoursePrice,
                                  course.Pricing.SessionPrice);
        }

        public static decimal CalculatePrice(int sessionBookingCount, RepeatedSessionData course)
        {
            return CalculatePrice(sessionBookingCount,
                                  course.Sessions.Count,
                                  course.Pricing.CoursePrice,
                                  course.Pricing.SessionPrice);
        }

        public static decimal CalculatePrice(int numberOfSessionsInBooking,
                                             int numberOfSessionsInCourse, 
                                             decimal? coursePrice,
                                             decimal? sessionPrice)
        {
            if (!coursePrice.HasValue && !sessionPrice.HasValue)
                throw new ArgumentException("Must have either session or course price.");

            if (numberOfSessionsInBooking == numberOfSessionsInCourse)
            {
                if (!coursePrice.HasValue)
                    coursePrice = numberOfSessionsInCourse * sessionPrice.GetValueOrDefault();
                return coursePrice.Value;
            }

            if (!sessionPrice.HasValue)
                sessionPrice = Math.Round(coursePrice.Value / numberOfSessionsInCourse, 2);
            return numberOfSessionsInBooking * sessionPrice.Value;
        }
    }
}
