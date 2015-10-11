using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Services;

namespace CoachSeek.Application.UseCases
{
    public class BookingGetByIdUseCase : BaseUseCase, IBookingGetByIdUseCase
    {
        public BookingData GetBooking(Guid id)
        {
            var sessionBooking = GetSessionBooking(id);
            if (sessionBooking.IsFound())
                return sessionBooking;
            var courseBooking = GetCourseBooking(id);
            return courseBooking;
        }

        private SingleSessionBookingData GetSessionBooking(Guid bookingId)
        {
            var booking = BusinessRepository.GetSessionBooking(Business.Id, bookingId);
            if (booking.IsNotFound())
                return null;
            var session = BusinessRepository.GetSession(Business.Id, booking.Session.Id);
            booking.Price = session.Pricing.SessionPrice.GetValueOrDefault();
            return booking;
        }

        private CourseBookingData GetCourseBooking(Guid bookingId)
        {
            var booking = BusinessRepository.GetCourseBooking(Business.Id, bookingId);
            var course = BusinessRepository.GetCourse(Business.Id, booking.Course.Id);
            booking.Price = CourseBookingPriceCalculator.CalculatePrice(booking, course);
            return booking;
        }
    }
}
