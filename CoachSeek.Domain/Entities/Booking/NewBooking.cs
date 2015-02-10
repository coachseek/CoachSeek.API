using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities.Booking
{
    public class NewBooking : Booking
    {
        public NewBooking(SessionKeyData session, CustomerKeyData customer)
            : base(Guid.NewGuid(), session, customer)
        { }

        //public NewBooking(NewBookingData data)
        //    : this(data.FirstName, 
        //           data.LastName, 
        //           data.Email, 
        //           data.Phone)
        //{ }
    }
}