using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    // Aggregate Root for booking process.
    public abstract class Booking
    {
        public Guid Id { get; private set; }
        public string PaymentStatus { get; set; }
        public CustomerKeyData Customer { get; private set; }


        protected Booking(CustomerKeyData customer)
        {
            Id = Guid.NewGuid();
            PaymentStatus = Constants.PAYMENT_STATUS_PENDING_INVOICE;
            Customer = customer;
        }

        // Data parameters denote that it's data from inside the application (ie. database).
        protected Booking(Guid id, string paymentStatus, CustomerKeyData customer)
        {
            Id = id;
            PaymentStatus = paymentStatus;
            Customer = customer;
        }

        protected Booking(BookingData data)
            : this(data.Id, data.PaymentStatus, data.Customer)
        { }


        public abstract BookingData ToData();
    }
}
