using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    // Aggregate Root for booking process.
    public class Booking
    {
        public Guid Id { get; private set; }

        public SessionKeyData Session { get; set; }
        public CustomerKeyData Customer { get; set; }


        public Booking(BookingAddCommand command)
            : this(command.Session, command.Customer)
        { }

        public Booking(SessionKeyCommand session, CustomerKeyCommand customer)
            : this(Guid.NewGuid(), session, customer)
        { }

        public Booking(Guid id, SessionKeyCommand session, CustomerKeyCommand customer)
        {
            Id = id;
            Session = new SessionKeyData { Id = session.Id };
            Customer = new CustomerKeyData { Id = customer.Id };
        }

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
    }
}
