using System;
using CoachSeek.Data.Model;

namespace CoachSeek.Domain.Entities
{
    public class CourseSessionOnlineBookingCollection : CourseSessionBookingCollection
    {
        public CourseSessionOnlineBookingCollection(Guid courseBookingId, RepeatedSessionData course, CustomerKeyData customer)
            : base(courseBookingId, course, customer)
        { }


        protected override SingleSessionBooking CreateSessionBooking(BookingSession session)
        {
            return new SingleSessionOnlineBooking(session, Customer, 0, CourseBookingId);
        }

        protected override SingleSessionBooking CreateSessionBooking(SingleSessionBookingData data, BookingSession session)
        {
            return new SingleSessionOnlineBooking(data.Id, session.ToData(), Customer, data.PaymentStatus, data.HasAttended, CourseBookingId);
        }
    }
}
