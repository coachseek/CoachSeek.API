using AutoMapper;
using CoachSeek.Api.Models.Api.Booking;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class BookingAddCommandConverter
    {
        public static BookingAddCommand Convert(ApiBookingSaveCommand apiCommand)
        {
            return Mapper.Map<ApiBookingSaveCommand, BookingAddCommand>(apiCommand);
        }
    }
}