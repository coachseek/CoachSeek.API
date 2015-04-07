using System;
using AutoMapper;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    // Aggregate Root for booking process.
    public abstract class Booking
    {
        public Guid Id { get; private set; }

        public CustomerKeyData Customer { get; set; }

        public string PaymentStatus { get; set; }
        public bool? HasAttended { get; set; }

        protected Booking(BookingAddCommand command)
            : this(command.Customer)
        {
            PaymentStatus = command.PaymentStatus;
            HasAttended = command.HasAttended;
        }

        protected Booking(CustomerKeyCommand customer)
            : this(Guid.NewGuid(), customer)
        { }

        protected Booking(Guid id, CustomerKeyCommand customer)
        {
            Id = id;
            Customer = new CustomerKeyData { Id = customer.Id };
        }

        protected Booking(Guid id, CustomerKeyData customer)
        {
            Id = id;
            Customer = customer;
        }


        public BookingData ToData()
        {
            return Mapper.Map<Booking, BookingData>(this);
        }
    }
}
