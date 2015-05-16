using CoachSeek.Data.Model;
using CoachSeek.Domain.Entities;

namespace CoachSeek.Application.Contracts.Services.Emailing
{
    public interface IOnlineBookingEmailer
    {
        void SendSessionEmailToCoach(SingleSessionBooking booking, SingleSessionData session, CoachData coach,
            CustomerData customer);
        void SendSessionEmailToCustomer(SingleSessionBooking booking, SingleSessionData session, CoachData coach,
            CustomerData customer);

        void SendCourseEmailToCoach(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer);
        void SendCourseEmailToCustomer(CourseBooking booking, RepeatedSessionData course, CoachData coach, CustomerData customer);
    }
}
