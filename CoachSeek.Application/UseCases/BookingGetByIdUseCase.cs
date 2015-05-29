using System;
using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Common.Extensions;
using CoachSeek.Data.Model;

namespace CoachSeek.Application.UseCases
{
    public class BookingGetByIdUseCase : BaseUseCase, IBookingGetByIdUseCase
    {
        public BookingData GetBooking(Guid id)
        {
            var sessionBooking = BusinessRepository.GetSessionBooking(Business.Id, id);
            if (sessionBooking.IsFound())
                return sessionBooking;
            var courseBooking = BusinessRepository.GetCourseBooking(Business.Id, id);
            return courseBooking;
        }
    }
}
