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
            var sessionBooking = BusinessRepository.GetSessionBooking(BusinessId, id);
            if (sessionBooking.IsFound())
                return sessionBooking;
            var courseBooking = BusinessRepository.GetCourseBooking(BusinessId, id);
            return courseBooking;
        }
    }
}
