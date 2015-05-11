using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.Contracts.Services.Emailing
{
    public interface IOnlineBookingEmailer
    {
        void SendSessionEmailToCoach(SingleSessionBooking booking);
        void SendSessionEmailToCustomer(SingleSessionBooking booking);

        void SendCourseEmailToCoach(CourseBooking booking);
        void SendCourseEmailToCustomer(CourseBooking booking);
    }
}
