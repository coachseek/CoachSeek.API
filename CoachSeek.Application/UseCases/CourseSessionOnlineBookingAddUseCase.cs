using CoachSeek.Application.Contracts.UseCases;
using CoachSeek.Application.Services.Emailing;
using CoachSeek.Domain.Commands;
using CoachSeek.Domain.Entities;
using CoachSeek.Domain.Exceptions;

namespace CoachSeek.Application.UseCases
{
    public class CourseSessionOnlineBookingAddUseCase : CourseSessionBookingAddUseCase, ICourseSessionOnlineBookingAddUseCase
    {
        protected override void ValidateCommandAdditional(BookingAddCommand newBooking, ValidationException errors)
        {
            ValidateIsOnlineBookable(errors);
        }

        private void ValidateIsOnlineBookable(ValidationException errors)
        {
            if (!Course.Booking.IsOnlineBookable)
                errors.Add("The course is not online bookable.");
        }

        protected override void PostProcessing(CourseBooking newBooking)
        {
            var emailer = new OnlineBookingEmailer();
            emailer.Initialise(Context);

            var coach = Context.BusinessContext.BusinessRepository.GetCoach(Business.Id, newBooking.Course.Coach.Id);
            var customer = Context.BusinessContext.BusinessRepository.GetCustomer(Business.Id, newBooking.Customer.Id);

            emailer.SendCourseEmailToCustomer(newBooking, coach, customer);
            emailer.SendCourseEmailToCoach(newBooking, coach, customer);
        }
    }
}
