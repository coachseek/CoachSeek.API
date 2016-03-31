using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    // Aggregate Root for booking process.
    public abstract class Booking
    {
        public Guid Id { get; private set; }
        public CustomerKeyData Customer { get; private set; }
        public abstract bool IsOnlineBooking { get; }
        public int DiscountPercent { get; set; }


        protected Booking(CustomerKeyData customer)
        {
            Id = Guid.NewGuid();
            Customer = customer;
        }

        // Data parameters denote that it's data from inside the application (ie. database).
        protected Booking(Guid id, CustomerKeyData customer)
        {
            Id = id;
            Customer = customer;
        }

        protected Booking(BookingData data)
            : this(data.Id, data.Customer)
        { }


        public abstract BookingData ToData();
    }
}
