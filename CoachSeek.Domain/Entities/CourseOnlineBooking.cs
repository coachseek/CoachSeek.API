using System;
using CoachSeek.Common;
using CoachSeek.Data.Model;
using CoachSeek.Domain.Commands;

namespace CoachSeek.Domain.Entities
{
    public class CourseOnlineBooking : CourseBooking
    {
        public CourseOnlineBooking(BookingAddCommand command, RepeatedSessionData course)
            : base(command, course)
        {
            PaymentStatus = Constants.PAYMENT_STATUS_PENDING_PAYMENT;
        }


        protected override SingleSessionBooking CreateSingleSessionBooking(SingleSessionData session, CustomerKeyCommand customer, Guid parentId)
        {
            return new SingleSessionOnlineBooking(new SessionKeyCommand(session.Id), customer, parentId);
        }
    }
}
