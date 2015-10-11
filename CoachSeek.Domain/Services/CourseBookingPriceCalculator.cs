using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Services
{
    public static class CourseBookingPriceCalculator
    {
        public static decimal CalculatePrice(CourseBookingData booking, RepeatedSessionData course)
        {
            if (IsBookingForWholeCourse(booking, course))
                return CalculateWholeCoursePaymentAmount(course);
         
            return CalculateMultipleSessionPaymentAmount(booking, course);
        }

        private static bool IsBookingForWholeCourse(CourseBookingData booking, RepeatedSessionData course)
        {
            return booking.SessionBookings.Count == course.Sessions.Count;
        }

        private static decimal CalculateWholeCoursePaymentAmount(RepeatedSessionData course)
        {
            return course.Pricing.CoursePrice ?? CalculateCoursePriceFromSessionPrice(course);
        }

        private static decimal CalculateCoursePriceFromSessionPrice(RepeatedSessionData course)
        {
            return course.Repetition.SessionCount * course.Pricing.SessionPrice.GetValueOrDefault();
        }

        private static decimal CalculateMultipleSessionPaymentAmount(CourseBookingData booking, RepeatedSessionData course)
        {
            var sessionPrice = course.Pricing.SessionPrice ??
                               Math.Round(course.Pricing.CoursePrice.GetValueOrDefault() / course.Repetition.SessionCount, 2);
            return sessionPrice * booking.SessionBookings.Count;
        }
    }
}
