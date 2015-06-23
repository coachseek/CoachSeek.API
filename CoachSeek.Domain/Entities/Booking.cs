using System;
using AutoMapper;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    // Aggregate Root for booking process.
    public abstract class Booking
    {
        public Guid Id { get; private set; }
        public string PaymentStatus { get; set; }
        public CustomerKeyData Customer { get; set; }


        // Command parameters denote that it's data from outside the application (ie. user input).
        protected Booking(BookingAddCommand command)
        {
            Id = Guid.NewGuid();
            PaymentStatus = Constants.PAYMENT_STATUS_PENDING_INVOICE;
            Customer = new CustomerKeyData { Id = command.Customer.Id };
        }

        //protected Booking(CustomerKeyCommand customer)
        //    : this(Guid.NewGuid(), customer)
        //{ }

        //protected Booking(Guid id, CustomerKeyCommand customer)
        //{
        //    Id = id;
        //    Customer = new CustomerKeyData { Id = customer.Id };
        //}

        // Data parameters denote that it's data from inside the application (ie. database).
        protected Booking(Guid id, string paymentStatus, CustomerKeyData customer)
        {
            Id = id;
            PaymentStatus = paymentStatus;
            Customer = customer;
        }


        public BookingData ToData()
        {
            return Mapper.Map<Booking, BookingData>(this);
        }
    }
}
