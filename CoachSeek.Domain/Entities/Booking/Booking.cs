using System;
using AutoMapper;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities.Booking
{
    // Aggregate Root for booking process.
    public class Booking
    {
        public Guid Id { get; private set; }

        public SessionKeyData Session { get; set; }
        public CustomerKeyData Customer { get; set; }


        public Booking(Guid id, SessionKeyData session, CustomerKeyData customer)
        {
            Id = id;
            Session = session;
            Customer = customer;
        }


        public BookingData ToData()
        {
            return Mapper.Map<Booking, BookingData>(this);
        }

        //public BookingKeyData ToKeyData()
        //{
        //    return Mapper.Map<Customer, CustomerKeyData>(this);
        //}
    }
}
