using System;
using AutoMapper;
using CoachSeek.Api.Models.Api.Booking;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Api.Conversion
{
    public static class BookingAddCommandConverter
    {
        public static BookingAddCommand Convert(Guid businessId, ApiBookingSaveCommand apiCommand)
        {
            var command = Mapper.Map<ApiBookingSaveCommand, BookingAddCommand>(apiCommand);
            command.BusinessId = businessId;

            return command;
        }
    }
}