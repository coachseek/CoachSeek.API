﻿using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Commands
{
    public class BookingAddCommand
    {
        public SessionKeyCommand Session { get; set; }
        public CustomerKeyCommand Customer { get; set; }


        public NewBookingData ToData()
        {
            return Mapper.Map<BookingAddCommand, NewBookingData>(this);
        }
    }
}